using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision_Master.Core
{
    /// <summary>
    /// TabPage 控制器 - 動態顯示/隱藏 Tab 並保持原始順序
    /// </summary>
    public class TabPageController
    {
        private readonly TabControl _tabControl;
        private readonly Dictionary<TabPage, int> _tabIndexMap = new Dictionary<TabPage, int>();

        public TabPageController(TabControl control)
        {
            _tabControl = control ?? throw new ArgumentNullException(nameof(control));

            // 建立初始索引對照表
            for (int i = 0; i < _tabControl.TabPages.Count; i++)
            {
                _tabIndexMap[_tabControl.TabPages[i]] = i;
            }
        }

        /// <summary>
        /// 設定 TabPage 的可見性
        /// </summary>
        public void SetVisible(TabPage tabPage, bool visible)
        {
            if (tabPage == null)
                throw new ArgumentNullException(nameof(tabPage));

            if (visible)
            {
                // 顯示 Tab
                if (!_tabControl.TabPages.Contains(tabPage))
                {
                    int index = _tabIndexMap.ContainsKey(tabPage) 
                        ? _tabIndexMap[tabPage] 
                        : _tabControl.TabPages.Count;
                    
                    _tabControl.TabPages.Insert(Math.Min(index, _tabControl.TabPages.Count), tabPage);
                }
            }
            else
            {
                // 隱藏 Tab
                if (_tabControl.TabPages.Contains(tabPage))
                {
                    _tabControl.TabPages.Remove(tabPage);
                }
            }
        }

        /// <summary>
        /// 檢查 TabPage 是否可見
        /// </summary>
        public bool IsVisible(TabPage tabPage)
        {
            if (tabPage == null)
                throw new ArgumentNullException(nameof(tabPage));

            return _tabControl.TabPages.Contains(tabPage);
        }

        /// <summary>
        /// 批次設定多個 TabPage 的可見性
        /// </summary>
        public void SetVisibleBatch(bool visible, params TabPage[] tabPages)
        {
            if (tabPages == null || tabPages.Length == 0)
                return;

            foreach (var tabPage in tabPages)
            {
                if (tabPage != null)
                {
                    SetVisible(tabPage, visible);
                }
            }
        }
    }
}
