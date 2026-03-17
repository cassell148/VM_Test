using System;
using System.Drawing;
using VM.Core;

namespace Vision_Master.Core
{
    /// <summary>
    /// 檢測結果
    /// </summary>
    public class InspectionResult
    {
        public bool IsOK { get; set; }
        public string OCRDate { get; set; }
        public string StandardDate { get; set; }
        public string NGReason { get; set; }
        public DateTime Timestamp { get; set; }
        public Image CapturedImage { get; set; }

        public InspectionResult()
        {
            Timestamp = DateTime.Now;
        }
    }

    /// <summary>
    /// 檢測管理器
    /// </summary>
    public class InspectionManager
    {
        private VmProcedure _procedure;
        private bool _isRunning = false;
        private StatisticsManager _statistics;

        /// <summary>
        /// 檢測完成事件
        /// </summary>
        public event EventHandler<InspectionResult> OnInspectionCompleted;

        public InspectionManager(StatisticsManager statistics)
        {
            _statistics = statistics;
        }

        /// <summary>
        /// 初始化檢測流程
        /// </summary>
        public bool Initialize(VmProcedure procedure)
        {
            try
            {
                if (procedure == null)
                {
                    Logger.Write("檢測流程為 null", Logger.LogLevel.Error);
                    return false;
                }

                _procedure = procedure;

                // 綁定檢測完成事件
                _procedure.OnWorkEndStatusCallBack -= Procedure_OnWorkEnd;
                _procedure.OnWorkEndStatusCallBack += Procedure_OnWorkEnd;

                Logger.Write($"檢測管理器初始化成功: {_procedure.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"檢測管理器初始化失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 開始連續檢測
        /// </summary>
        public void Start()
        {
            _isRunning = true;
            Logger.Write("開始連續檢測模式");
        }

        /// <summary>
        /// 停止連續檢測
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            Logger.Write("停止連續檢測模式");
        }

        /// <summary>
        /// 是否正在運行
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// 流程檢測完成回調
        /// </summary>
        private void Procedure_OnWorkEnd(object sender, EventArgs e)
        {
            if (!_isRunning)
                return;

            try
            {
                Logger.Write("檢測完成，開始分析結果");

                var result = new InspectionResult();

                // 1. 讀取OCR結果
                result.OCRDate = GetOCRResult();
                Logger.Write($"OCR讀取結果: {result.OCRDate}");

                // 2. 取得標準日期
                result.StandardDate = ProductManager.CurrentProduct?.GetStandardDate() ?? "";
                Logger.Write($"標準日期: {result.StandardDate}");

                // 3. 比對日期
                if (string.IsNullOrEmpty(result.OCRDate))
                {
                    result.IsOK = false;
                    result.NGReason = "OCR讀取失敗";
                }
                else if (result.OCRDate != result.StandardDate)
                {
                    result.IsOK = false;
                    result.NGReason = $"日期不符 (讀取: {FormatDate(result.OCRDate)}, 標準: {FormatDate(result.StandardDate)})";
                }
                else
                {
                    result.IsOK = true;
                    result.NGReason = "";
                }

                // 4. 更新統計
                if (result.IsOK)
                {
                    _statistics.IncrementOK();
                }
                else
                {
                    _statistics.IncrementNG();
                }

                // 5. 觸發檢測完成事件
                OnInspectionCompleted?.Invoke(this, result);

                Logger.Write($"檢測結果: {(result.IsOK ? "OK" : "NG")} - {result.NGReason}");
            }
            catch (Exception ex)
            {
                Logger.Write($"檢測結果處理失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// 從VisionMaster讀取OCR結果
        /// </summary>
        private string GetOCRResult()
        {
            try
            {
                if (_procedure == null)
                    return "";

                // 從 ModuResult 讀取 OCR 輸出
                // 這裡需要根據您的VisionMaster流程中OCR工具的實際輸出名稱調整
                var ocrOutput = _procedure.ModuResult.GetOutputString("OCRDate");

                if (ocrOutput.astStringVal != null && ocrOutput.astStringVal.Length > 0)
                {
                    string rawResult = ocrOutput.astStringVal[0].strValue;

                    // 清理結果（移除空格、特殊字元等）
                    string cleanedResult = rawResult.Replace(" ", "")
                                                    .Replace(".", "")
                                                    .Replace("/", "")
                                                    .Replace("-", "")
                                                    .Trim();

                    // 驗證格式（應該是8位數字）
                    if (cleanedResult.Length == 8 && long.TryParse(cleanedResult, out _))
                    {
                        return cleanedResult;
                    }
                    else
                    {
                        Logger.Write($"OCR結果格式錯誤: {rawResult}", Logger.LogLevel.Warning);
                        return "";
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                Logger.Write($"讀取OCR結果失敗: {ex.Message}", Logger.LogLevel.Error);
                return "";
            }
        }

        /// <summary>
        /// 格式化日期顯示（20250715 → 2025.07.15）
        /// </summary>
        private string FormatDate(string date)
        {
            if (string.IsNullOrEmpty(date) || date.Length != 8)
                return date;

            try
            {
                return $"{date.Substring(0, 4)}.{date.Substring(4, 2)}.{date.Substring(6, 2)}";
            }
            catch
            {
                return date;
            }
        }
    }
}