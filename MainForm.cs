using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision_Master.Core;
using Vision_Master.Forms;
using VM.Core;
using VmSolutionManager = Vision_Master.Core.VmSolutionManager;

namespace Vision_Master
{
    public partial class MainForm : Form
    {
        #region 私有成員變數

        // 核心管理器
        private FX3GPLCController _plcController;
        private StatisticsManager _statistics;

        // 狀態
        private bool _isDetectionRunning = false;
        private bool _isLoggedIn = false;
        private DateTime _startTime;

        // 產品資訊
        private Product _currentProduct;

        #endregion

        #region 建構函式

        public MainForm()
        {
            InitializeComponent();

            // 設定窗體屬性
            this.Text = "VisionMaster 日期檢測系統 v1.0";
            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // 註冊事件
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;

            // 註冊 TabControl 切換事件（權限控制）
            tabControl1.Selecting += TabControl1_Selecting;
        }

        #endregion

        #region 初始化

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("========================================");
                Logger.Write("[MainForm] 主窗體載入開始");
                Logger.Write("========================================");

                // 1. 初始化統計管理器
                _statistics = new StatisticsManager();
                _statistics.OnOKIncremented += Statistics_OnOKIncremented;
                _statistics.OnNGIncremented += Statistics_OnNGIncremented;
                _statistics.OnCleared += Statistics_OnCleared;

                // 2. 載入產品配置
                if (!ProductManager.LoadConfig())
                {
                    MessageBox.Show("產品配置載入失敗！\n將使用預設產品。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                _currentProduct = ProductManager.CurrentProduct;

                // 3. 初始化 UI
                InitializeDetectionPage();
                InitializeVisionMasterPage();
                InitializeProductPage();
                InitializeSystemPage();

                // 4. 初始化 PLC
                await InitializePLCAsync();

                // 5. 載入 VisionMaster 方案
                bool vmLoaded = await VmSolutionManager.LoadSolution();
                if (!vmLoaded)
                {
                    Logger.Write("[MainForm] VisionMaster 方案載入失敗", Logger.LogLevel.Warning);
                }

                // 6. 更新 UI 狀態
                UpdateDetectionPageUI();
                UpdateUIState(false);

                Logger.Write("[MainForm] 主窗體載入完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[MainForm] 載入失敗: {ex.Message}", Logger.LogLevel.Error);
                Logger.Write($"[MainForm] 堆疊追蹤: {ex.StackTrace}", Logger.LogLevel.Error);
                MessageBox.Show($"程式初始化失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ===== 檢測頁面 =====

        private void InitializeDetectionPage()
        {
            try
            {
                // 設定 DataGridView
                dgvDetectionLog.Columns.Clear();
                dgvDetectionLog.Columns.Add("Time", "時間");
                dgvDetectionLog.Columns.Add("Product", "產品");
                dgvDetectionLog.Columns.Add("Result", "結果");
                dgvDetectionLog.Columns.Add("PrintedDate", "印刷日期");
                dgvDetectionLog.Columns.Add("StandardDate", "標準日期");
                dgvDetectionLog.Columns.Add("NGReason", "NG 原因");

                dgvDetectionLog.Columns["Time"].Width = 150;
                dgvDetectionLog.Columns["Product"].Width = 100;
                dgvDetectionLog.Columns["Result"].Width = 80;
                dgvDetectionLog.Columns["PrintedDate"].Width = 120;
                dgvDetectionLog.Columns["StandardDate"].Width = 120;
                dgvDetectionLog.Columns["NGReason"].Width = 300;

                dgvDetectionLog.AllowUserToAddRows = false;
                dgvDetectionLog.AllowUserToDeleteRows = false;
                dgvDetectionLog.ReadOnly = true;
                dgvDetectionLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // 清空圖像
                picLiveImage.Image = null;
                picNGImage.Image = null;

                // 清空日誌
                txtDetectionLog.Clear();

                Logger.Write("[Detection] 檢測頁面初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[Detection] 初始化失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void UpdateDetectionPageUI()
        {
            try
            {
                // 更新產品資訊
                if (_currentProduct != null)
                {
                    lblCurrentProduct.Text = $"當前產品: {_currentProduct.Name}";
                    lblProductCode.Text = $"產品代碼: {_currentProduct.Code}";
                    lblStandardDate.Text = $"標準日期: {_currentProduct.GetFormattedStandardDate()}";
                    lblDaysOffset.Text = $"保質期: {_currentProduct.DaysOffset} 天";
                }

                // 更新統計
                UpdateStatisticsDisplay();

                // 更新運行時間
                if (_isDetectionRunning)
                {
                    TimeSpan runTime = DateTime.Now - _startTime;
                    lblRunTime.Text = $"運行時間: {runTime.Hours:D2}:{runTime.Minutes:D2}:{runTime.Seconds:D2}";
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Detection] 更新UI失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void UpdateStatisticsDisplay()
        {
            lblOKCount.Text = _statistics.GetOKDisplayText();
            lblNGCount.Text = _statistics.GetNGDisplayText();
            lblTotalCount.Text = $"總計: {_statistics.TotalCount}";

            double okPercentage = _statistics.OKPercentage;
            lblYieldRate.Text = $"良率: {okPercentage:F2}%";

            // 良率顏色
            if (okPercentage >= 95)
                lblYieldRate.ForeColor = Color.Green;
            else if (okPercentage >= 90)
                lblYieldRate.ForeColor = Color.Orange;
            else
                lblYieldRate.ForeColor = Color.Red;
        }

        // ===== 檢測頁面按鈕事件 =====

        private async void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                if (VmSolution.Instance == null)
                {
                    MessageBox.Show("方案未載入");
                    return;
                }

                // 只測試能不能取得流程
                VmProcedure proc = (VmProcedure)VmSolution.Instance["流程一"];

                if (proc == null)
                {
                    MessageBox.Show("proc 是 null，流程名稱可能不對");
                    return;
                }

                // 測試流程屬性
                string info = $"流程資訊：\n";
                info += $"名稱：{proc.Name}\n";
                info += $"是否啟用：{proc.IsEnabled}\n";
                info += $"連續執行：{proc.ContinuousRunEnable}\n";

                MessageBox.Show(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"錯誤:\n{ex.Message}");
            }

            try
            {
                if (_isDetectionRunning)
                {
                    Logger.Write("[Detection] 檢測已在運行中");
                    return;
                }

                Logger.Write("[Detection] 開始檢測...");
                AppendDetectionLog("開始檢測...");

                // 確認 PLC 連接
                if (_plcController != null && !_plcController.IsConnected)
                {
                    Logger.Write("[PLC] PLC 未連接，嘗試重新連接...");
                    await _plcController.ConnectAsync();
                }

                // TODO: 啟動 VisionMaster 檢測
                // 這裡需要整合實際的檢測邏輯

                _isDetectionRunning = true;
                _startTime = DateTime.Now;

                UpdateUIState(true);
                lblStatus.Text = "運行中";
                lblStatus.BackColor = Color.LightGreen;
                AppendDetectionLog("檢測已啟動");

                Logger.Write("[Detection] 檢測已啟動");
            }
            catch (Exception ex)
            {
                Logger.Write($"[Detection] 啟動失敗: {ex.Message}", Logger.LogLevel.Error);
                AppendDetectionLog($"啟動失敗: {ex.Message}");
                MessageBox.Show($"啟動檢測失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isDetectionRunning)
                {
                    Logger.Write("[Detection] 檢測未運行");
                    return;
                }

                Logger.Write("[Detection] 停止檢測...");
                AppendDetectionLog("停止檢測...");

                // TODO: 停止 VisionMaster 檢測

                _isDetectionRunning = false;

                UpdateUIState(false);
                lblStatus.Text = "已停止";
                lblStatus.BackColor = Color.LightCoral;
                AppendDetectionLog("檢測已停止");

                // PLC 保持連接

                Logger.Write("[Detection] 檢測已停止");
            }
            catch (Exception ex)
            {
                Logger.Write($"[Detection] 停止失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"停止檢測失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isDetectionRunning)
                {
                    MessageBox.Show("請先停止檢測後再清零統計數據。",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("確定要清零所有統計數據嗎？",
                    "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _statistics.Clear();
                    dgvDetectionLog.Rows.Clear();
                    AppendDetectionLog("統計數據已清零");
                    Logger.Write("[Detection] 統計數據已清零");
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Detection] 清零失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void btnSwitchProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isDetectionRunning)
                {
                    MessageBox.Show("請先停止檢測後再切換產品。",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 切換到下一個產品
                _currentProduct = ProductManager.SwitchToNextProduct();

                if (_currentProduct != null)
                {
                    UpdateDetectionPageUI();
                    AppendDetectionLog($"已切換至產品: {_currentProduct.Name}");
                    Logger.Write($"[Product] 切換至產品: {_currentProduct.Name}");
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Product] 切換產品失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"切換產品失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== 統計事件 =====

        private void Statistics_OnOKIncremented(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Statistics_OnOKIncremented(sender, e)));
                return;
            }
            UpdateStatisticsDisplay();
        }

        private void Statistics_OnNGIncremented(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Statistics_OnNGIncremented(sender, e)));
                return;
            }
            UpdateStatisticsDisplay();
        }

        private void Statistics_OnCleared(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Statistics_OnCleared(sender, e)));
                return;
            }
            UpdateStatisticsDisplay();
        }

        // ===== 輔助方法 =====

        private void AppendDetectionLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendDetectionLog), message);
                return;
            }

            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";
            txtDetectionLog.AppendText(logMessage);
            txtDetectionLog.SelectionStart = txtDetectionLog.Text.Length;
            txtDetectionLog.ScrollToCaret();
        }

        #endregion

        #region ===== VisionMaster 設定頁面 =====

        private void InitializeVisionMasterPage()
        {
            try
            {
                // 初始化 VmMainViewConfigControl
                if (vmMainViewConfigControl1 != null)
                {
                    vmMainViewConfigControl1.Dock = DockStyle.Fill;
                }

                Logger.Write("[VisionMaster] VisionMaster 設定頁面初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[VisionMaster] 初始化失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private async void btnLoadSolution_Click(object sender, EventArgs e)
        {
            try
            {
                bool loaded = await VmSolutionManager.LoadSolutionFromFile();
                if (loaded)
                {
                    MessageBox.Show("方案載入成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[VisionMaster] 載入方案失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"載入方案失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveSolution_Click(object sender, EventArgs e)
        {
            try
            {
                VmSolutionManager.SaveSolution();
            }
            catch (Exception ex)
            {
                Logger.Write($"[VisionMaster] 儲存方案失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void btnSaveSolutionAs_Click(object sender, EventArgs e)
        {
            try
            {
                VmSolutionManager.SaveSolutionAs();
            }
            catch (Exception ex)
            {
                Logger.Write($"[VisionMaster] 另存方案失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        #endregion

        #region ===== 產品設定頁面 =====

        private void InitializeProductPage()
        {
            try
            {
                // 設定 DataGridView
                dgvProducts.Columns.Clear();
                dgvProducts.Columns.Add("Name", "產品名稱");
                dgvProducts.Columns.Add("Code", "產品代碼");
                dgvProducts.Columns.Add("DaysOffset", "保質期(天)");
                dgvProducts.Columns.Add("StandardDate", "標準日期");

                dgvProducts.Columns["Name"].Width = 150;
                dgvProducts.Columns["Code"].Width = 120;
                dgvProducts.Columns["DaysOffset"].Width = 100;
                dgvProducts.Columns["StandardDate"].Width = 150;

                dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProducts.MultiSelect = false;

                // 載入產品列表
                LoadProductList();

                Logger.Write("[Product] 產品設定頁面初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[Product] 初始化失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void LoadProductList()
        {
            try
            {
                dgvProducts.Rows.Clear();

                if (ProductManager.AllProducts != null)
                {
                    foreach (var product in ProductManager.AllProducts)
                    {
                        int rowIndex = dgvProducts.Rows.Add();
                        DataGridViewRow row = dgvProducts.Rows[rowIndex];

                        row.Cells["Name"].Value = product.Name;
                        row.Cells["Code"].Value = product.Code;
                        row.Cells["DaysOffset"].Value = product.DaysOffset;
                        row.Cells["StandardDate"].Value = product.GetFormattedStandardDate();

                        // 標記當前產品
                        if (product == ProductManager.CurrentProduct)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Product] 載入產品列表失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            // TODO: 實作新增產品對話框
            MessageBox.Show("新增產品功能開發中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            // TODO: 實作刪除產品功能
            MessageBox.Show("刪除產品功能開發中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveProducts_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProductManager.SaveConfig())
                {
                    MessageBox.Show("產品配置已儲存！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Product] 儲存產品配置失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"儲存失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ===== 系統設定頁面 =====

        private void InitializeSystemPage()
        {
            try
            {
                // 載入 PLC 設定
                var config = Program.Config;
                txtPlcIP.Text = config.PlcIp;
                txtPlcPort.Text = config.PlcPort.ToString();
                chkPlcEnabled.Checked = config.PlcEnabled;

                // 載入路徑設定
                txtSolutionPath.Text = config.SolutionPath;
                txtNGImagePath.Text = config.SaveNGImagePath;

                Logger.Write("[System] 系統設定頁面初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[System] 初始化失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        private async void btnTestPLC_Click(object sender, EventArgs e)
        {
            try
            {
                if (_plcController == null)
                {
                    MessageBox.Show("PLC 控制器未初始化", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnTestPLC.Enabled = false;
                btnTestPLC.Text = "測試中...";

                bool sent = await _plcController.SendNGSignalAsync();

                if (sent)
                {
                    MessageBox.Show("PLC 測試訊號發送成功！", "測試結果",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("PLC 測試訊號發送失敗，請檢查連接。", "測試結果",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 測試失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"測試失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestPLC.Enabled = true;
                btnTestPLC.Text = "測試連接";
            }
        }

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            try
            {
                // 更新配置
                var config = Program.Config;
                // TODO: 更新配置值並儲存

                MessageBox.Show("設定已套用！\n部分設定需要重新啟動程式才能生效。",
                    "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Write($"[System] 套用設定失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"套用設定失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            try
            {
                // 開啟日誌查看器
                var logViewer = new LogViewerForm();
                logViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Write($"[System] 開啟日誌查看器失敗: {ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"開啟日誌查看器失敗:\n{ex.Message}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region PLC 初始化

        private async Task InitializePLCAsync()
        {
            try
            {
                var config = Program.Config;

                if (!config.PlcEnabled)
                {
                    Logger.Write("[PLC] PLC 功能已停用");
                    return;
                }

                Logger.Write($"[PLC] 初始化 PLC: {config.PlcIp}:{config.PlcPort}");

                _plcController = new FX3GPLCController(config.PlcIp, config.PlcPort);
                bool connected = await _plcController.ConnectAsync();

                if (connected)
                {
                    Logger.Write("[PLC] PLC 連接成功");
                    AppendDetectionLog("PLC 連接成功");
                }
                else
                {
                    Logger.Write("[PLC] PLC 連接失敗", Logger.LogLevel.Warning);
                    AppendDetectionLog("PLC 連接失敗（程式繼續運行）");
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 初始化失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        #endregion

        #region 權限控制

        private void TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                // 檢測頁面（index 0）所有人都可以訪問
                if (e.TabPageIndex == 0)
                    return;

                // 其他頁面需要登入
                if (!_isLoggedIn)
                {
                    e.Cancel = true;

                    using (var loginForm = new LoginForm())
                    {
                        if (loginForm.ShowDialog() == DialogResult.OK)
                        {
                            _isLoggedIn = true;
                            Logger.Write($"[Auth] 使用者登入成功，訪問: {e.TabPage.Text}");

                            // 登入成功後切換到目標頁面
                            tabControl1.SelectedTab = e.TabPage;
                        }
                        else
                        {
                            Logger.Write("[Auth] 使用者取消登入");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Auth] 權限檢查失敗: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        #endregion

        #region UI 狀態控制

        private void UpdateUIState(bool isRunning)
        {
            btnStart.Enabled = !isRunning;
            btnStop.Enabled = isRunning;
            btnReset.Enabled = !isRunning;
            btnSwitchProduct.Enabled = !isRunning;
        }

        #endregion

        #region 清理資源

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Logger.Write("[MainForm] 正在關閉程式...");

                if (_isDetectionRunning)
                {
                    DialogResult result = MessageBox.Show(
                        "檢測正在運行中，確定要關閉程式嗎？",
                        "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }

                    // TODO: 停止檢測
                }

                // 斷開 PLC
                if (_plcController != null)
                {
                    Logger.Write("[PLC] 正在斷開 PLC...");
                    _plcController.Disconnect();
                    _plcController.Dispose();
                }

                // 清理 VisionMaster
                VmSolutionManager.Dispose();

                // 釋放圖像資源
                if (picLiveImage.Image != null)
                {
                    picLiveImage.Image.Dispose();
                }
                if (picNGImage.Image != null)
                {
                    picNGImage.Image.Dispose();
                }

                Logger.Write("[MainForm] 程式關閉完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"[MainForm] 關閉時發生錯誤: {ex.Message}", Logger.LogLevel.Error);
            }
        }

        #endregion
    }
}
