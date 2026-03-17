using System;

namespace Vision_Master.Core
{
    /// <summary>
    /// 統計管理器
    /// </summary>
    public class StatisticsManager
    {
        private int _okCount = 0;
        private int _ngCount = 0;
        private readonly object _lockObj = new object();

        /// <summary>
        /// OK數量
        /// </summary>
        public int OKCount
        {
            get { lock (_lockObj) { return _okCount; } }
        }

        /// <summary>
        /// NG數量
        /// </summary>
        public int NGCount
        {
            get { lock (_lockObj) { return _ngCount; } }
        }

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount
        {
            get { lock (_lockObj) { return _okCount + _ngCount; } }
        }

        /// <summary>
        /// OK百分比
        /// </summary>
        public double OKPercentage
        {
            get
            {
                lock (_lockObj)
                {
                    if (TotalCount == 0) return 0;
                    return (_okCount * 100.0) / TotalCount;
                }
            }
        }

        /// <summary>
        /// NG百分比
        /// </summary>
        public double NGPercentage
        {
            get
            {
                lock (_lockObj)
                {
                    if (TotalCount == 0) return 0;
                    return (_ngCount * 100.0) / TotalCount;
                }
            }
        }

        /// <summary>
        /// OK數量增加事件
        /// </summary>
        public event EventHandler OnOKIncremented;

        /// <summary>
        /// NG數量增加事件
        /// </summary>
        public event EventHandler OnNGIncremented;

        /// <summary>
        /// 統計清零事件
        /// </summary>
        public event EventHandler OnCleared;

        /// <summary>
        /// 增加OK計數
        /// </summary>
        public void IncrementOK()
        {
            lock (_lockObj)
            {
                _okCount++;
            }
            OnOKIncremented?.Invoke(this, EventArgs.Empty);
            Logger.Write($"統計更新: OK={_okCount}, NG={_ngCount}");
        }

        /// <summary>
        /// 增加NG計數
        /// </summary>
        public void IncrementNG()
        {
            lock (_lockObj)
            {
                _ngCount++;
            }
            OnNGIncremented?.Invoke(this, EventArgs.Empty);
            Logger.Write($"統計更新: OK={_okCount}, NG={_ngCount}");
        }

        /// <summary>
        /// 清除統計
        /// </summary>
        public void Clear()
        {
            lock (_lockObj)
            {
                _okCount = 0;
                _ngCount = 0;
            }
            OnCleared?.Invoke(this, EventArgs.Empty);
            Logger.Write("統計已清零");
        }

        /// <summary>
        /// 取得統計文字（用於顯示）
        /// </summary>
        public string GetOKDisplayText()
        {
            return $"OK: {OKCount} ({OKPercentage:F1}%)";
        }

        /// <summary>
        /// 取得統計文字（用於顯示）
        /// </summary>
        public string GetNGDisplayText()
        {
            return $"NG: {NGCount} ({NGPercentage:F1}%)";
        }
    }
}