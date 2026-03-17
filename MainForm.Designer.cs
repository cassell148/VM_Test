namespace Vision_Master
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDetection = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.picNGImage = new System.Windows.Forms.PictureBox();
            this.lblNGTitle = new System.Windows.Forms.Label();
            this.picLiveImage = new System.Windows.Forms.PictureBox();
            this.lblLiveTitle = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.txtDetectionLog = new System.Windows.Forms.TextBox();
            this.dgvDetectionLog = new System.Windows.Forms.DataGridView();
            this.panelControl = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.lblYieldRate = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblNGCount = new System.Windows.Forms.Label();
            this.lblOKCount = new System.Windows.Forms.Label();
            this.panelProduct = new System.Windows.Forms.Panel();
            this.btnSwitchProduct = new System.Windows.Forms.Button();
            this.lblDaysOffset = new System.Windows.Forms.Label();
            this.lblStandardDate = new System.Windows.Forms.Label();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblCurrentProduct = new System.Windows.Forms.Label();
            this.tabPageVisionMaster = new System.Windows.Forms.TabPage();
            this.vmMainViewConfigControl1 = new VMControls.Winform.Release.VmMainViewConfigControl();
            this.vmGlobalToolControl1 = new VMControls.Winform.Release.VmGlobalToolControl();
            this.panelVMButtons = new System.Windows.Forms.Panel();
            this.btnSaveSolutionAs = new System.Windows.Forms.Button();
            this.btnSaveSolution = new System.Windows.Forms.Button();
            this.btnLoadSolution = new System.Windows.Forms.Button();
            this.tabPageProduct = new System.Windows.Forms.TabPage();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.panelProductButtons = new System.Windows.Forms.Panel();
            this.btnSaveProducts = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.groupBoxPaths = new System.Windows.Forms.GroupBox();
            this.txtNGImagePath = new System.Windows.Forms.TextBox();
            this.lblNGImagePath = new System.Windows.Forms.Label();
            this.txtSolutionPath = new System.Windows.Forms.TextBox();
            this.lblSolutionPath = new System.Windows.Forms.Label();
            this.groupBoxPLC = new System.Windows.Forms.GroupBox();
            this.btnTestPLC = new System.Windows.Forms.Button();
            this.chkPlcEnabled = new System.Windows.Forms.CheckBox();
            this.txtPlcPort = new System.Windows.Forms.TextBox();
            this.lblPlcPort = new System.Windows.Forms.Label();
            this.txtPlcIP = new System.Windows.Forms.TextBox();
            this.lblPlcIP = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageDetection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNGImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLiveImage)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetectionLog)).BeginInit();
            this.panelControl.SuspendLayout();
            this.panelStatistics.SuspendLayout();
            this.panelProduct.SuspendLayout();
            this.tabPageVisionMaster.SuspendLayout();
            this.panelVMButtons.SuspendLayout();
            this.tabPageProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.panelProductButtons.SuspendLayout();
            this.tabPageSystem.SuspendLayout();
            this.groupBoxPaths.SuspendLayout();
            this.groupBoxPLC.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDetection);
            this.tabControl1.Controls.Add(this.tabPageVisionMaster);
            this.tabControl1.Controls.Add(this.tabPageProduct);
            this.tabControl1.Controls.Add(this.tabPageSystem);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1920, 1080);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageDetection
            // 
            this.tabPageDetection.Controls.Add(this.splitContainer1);
            this.tabPageDetection.Location = new System.Drawing.Point(4, 39);
            this.tabPageDetection.Name = "tabPageDetection";
            this.tabPageDetection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetection.Size = new System.Drawing.Size(1912, 1037);
            this.tabPageDetection.TabIndex = 0;
            this.tabPageDetection.Text = "檢測";
            this.tabPageDetection.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelRight);
            this.splitContainer1.Size = new System.Drawing.Size(1906, 1031);
            this.splitContainer1.SplitterDistance = 900;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.picNGImage);
            this.panelLeft.Controls.Add(this.lblNGTitle);
            this.panelLeft.Controls.Add(this.picLiveImage);
            this.panelLeft.Controls.Add(this.lblLiveTitle);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(900, 1031);
            this.panelLeft.TabIndex = 0;
            // 
            // picNGImage
            // 
            this.picNGImage.BackColor = System.Drawing.Color.Black;
            this.picNGImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNGImage.Location = new System.Drawing.Point(10, 595);
            this.picNGImage.Name = "picNGImage";
            this.picNGImage.Size = new System.Drawing.Size(880, 430);
            this.picNGImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNGImage.TabIndex = 3;
            this.picNGImage.TabStop = false;
            // 
            // lblNGTitle
            // 
            this.lblNGTitle.BackColor = System.Drawing.Color.DarkRed;
            this.lblNGTitle.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblNGTitle.ForeColor = System.Drawing.Color.White;
            this.lblNGTitle.Location = new System.Drawing.Point(10, 555);
            this.lblNGTitle.Name = "lblNGTitle";
            this.lblNGTitle.Size = new System.Drawing.Size(880, 35);
            this.lblNGTitle.TabIndex = 2;
            this.lblNGTitle.Text = "NG 圖像";
            this.lblNGTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picLiveImage
            // 
            this.picLiveImage.BackColor = System.Drawing.Color.Black;
            this.picLiveImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLiveImage.Location = new System.Drawing.Point(10, 50);
            this.picLiveImage.Name = "picLiveImage";
            this.picLiveImage.Size = new System.Drawing.Size(880, 495);
            this.picLiveImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLiveImage.TabIndex = 1;
            this.picLiveImage.TabStop = false;
            // 
            // lblLiveTitle
            // 
            this.lblLiveTitle.BackColor = System.Drawing.Color.Navy;
            this.lblLiveTitle.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblLiveTitle.ForeColor = System.Drawing.Color.White;
            this.lblLiveTitle.Location = new System.Drawing.Point(10, 10);
            this.lblLiveTitle.Name = "lblLiveTitle";
            this.lblLiveTitle.Size = new System.Drawing.Size(880, 35);
            this.lblLiveTitle.TabIndex = 0;
            this.lblLiveTitle.Text = "即時檢測";
            this.lblLiveTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.txtDetectionLog);
            this.panelRight.Controls.Add(this.dgvDetectionLog);
            this.panelRight.Controls.Add(this.panelControl);
            this.panelRight.Controls.Add(this.panelStatistics);
            this.panelRight.Controls.Add(this.panelProduct);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(1002, 1031);
            this.panelRight.TabIndex = 0;
            // 
            // txtDetectionLog
            // 
            this.txtDetectionLog.BackColor = System.Drawing.Color.Black;
            this.txtDetectionLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtDetectionLog.ForeColor = System.Drawing.Color.Lime;
            this.txtDetectionLog.Location = new System.Drawing.Point(10, 830);
            this.txtDetectionLog.Multiline = true;
            this.txtDetectionLog.Name = "txtDetectionLog";
            this.txtDetectionLog.ReadOnly = true;
            this.txtDetectionLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetectionLog.Size = new System.Drawing.Size(982, 195);
            this.txtDetectionLog.TabIndex = 4;
            // 
            // dgvDetectionLog
            // 
            this.dgvDetectionLog.AllowUserToAddRows = false;
            this.dgvDetectionLog.AllowUserToDeleteRows = false;
            this.dgvDetectionLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetectionLog.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetectionLog.ColumnHeadersHeight = 34;
            this.dgvDetectionLog.Location = new System.Drawing.Point(10, 370);
            this.dgvDetectionLog.MultiSelect = false;
            this.dgvDetectionLog.Name = "dgvDetectionLog";
            this.dgvDetectionLog.ReadOnly = true;
            this.dgvDetectionLog.RowHeadersWidth = 51;
            this.dgvDetectionLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetectionLog.Size = new System.Drawing.Size(982, 450);
            this.dgvDetectionLog.TabIndex = 3;
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.LightGray;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControl.Controls.Add(this.btnReset);
            this.panelControl.Controls.Add(this.btnStop);
            this.panelControl.Controls.Add(this.btnStart);
            this.panelControl.Location = new System.Drawing.Point(10, 270);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(982, 90);
            this.panelControl.TabIndex = 2;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Orange;
            this.btnReset.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(660, 10);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(310, 70);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "⟲ 清零統計";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(340, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(310, 70);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "■ 停止檢測";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStart.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(10, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(320, 70);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "▶ 開始檢測";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panelStatistics
            // 
            this.panelStatistics.BackColor = System.Drawing.Color.LightYellow;
            this.panelStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatistics.Controls.Add(this.lblStatus);
            this.panelStatistics.Controls.Add(this.lblRunTime);
            this.panelStatistics.Controls.Add(this.lblYieldRate);
            this.panelStatistics.Controls.Add(this.lblTotalCount);
            this.panelStatistics.Controls.Add(this.lblNGCount);
            this.panelStatistics.Controls.Add(this.lblOKCount);
            this.panelStatistics.Location = new System.Drawing.Point(10, 140);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Size = new System.Drawing.Size(982, 120);
            this.panelStatistics.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.LightGray;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(740, 65);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(230, 45);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "就緒";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRunTime
            // 
            this.lblRunTime.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblRunTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRunTime.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblRunTime.Location = new System.Drawing.Point(500, 65);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(230, 45);
            this.lblRunTime.TabIndex = 4;
            this.lblRunTime.Text = "運行時間: 00:00:00";
            this.lblRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYieldRate
            // 
            this.lblYieldRate.BackColor = System.Drawing.Color.LightGreen;
            this.lblYieldRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblYieldRate.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblYieldRate.ForeColor = System.Drawing.Color.Green;
            this.lblYieldRate.Location = new System.Drawing.Point(10, 65);
            this.lblYieldRate.Name = "lblYieldRate";
            this.lblYieldRate.Size = new System.Drawing.Size(480, 45);
            this.lblYieldRate.TabIndex = 3;
            this.lblYieldRate.Text = "良率: 0.00%";
            this.lblYieldRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalCount.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalCount.Location = new System.Drawing.Point(660, 10);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(310, 45);
            this.lblTotalCount.TabIndex = 2;
            this.lblTotalCount.Text = "總計: 0";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNGCount
            // 
            this.lblNGCount.BackColor = System.Drawing.Color.LightCoral;
            this.lblNGCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNGCount.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblNGCount.Location = new System.Drawing.Point(340, 10);
            this.lblNGCount.Name = "lblNGCount";
            this.lblNGCount.Size = new System.Drawing.Size(310, 45);
            this.lblNGCount.TabIndex = 1;
            this.lblNGCount.Text = "NG: 0 (0.0%)";
            this.lblNGCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOKCount
            // 
            this.lblOKCount.BackColor = System.Drawing.Color.LightGreen;
            this.lblOKCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOKCount.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblOKCount.Location = new System.Drawing.Point(10, 10);
            this.lblOKCount.Name = "lblOKCount";
            this.lblOKCount.Size = new System.Drawing.Size(320, 45);
            this.lblOKCount.TabIndex = 0;
            this.lblOKCount.Text = "OK: 0 (0.0%)";
            this.lblOKCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProduct
            // 
            this.panelProduct.BackColor = System.Drawing.Color.LightBlue;
            this.panelProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProduct.Controls.Add(this.btnSwitchProduct);
            this.panelProduct.Controls.Add(this.lblDaysOffset);
            this.panelProduct.Controls.Add(this.lblStandardDate);
            this.panelProduct.Controls.Add(this.lblProductCode);
            this.panelProduct.Controls.Add(this.lblCurrentProduct);
            this.panelProduct.Location = new System.Drawing.Point(10, 10);
            this.panelProduct.Name = "panelProduct";
            this.panelProduct.Size = new System.Drawing.Size(982, 120);
            this.panelProduct.TabIndex = 0;
            // 
            // btnSwitchProduct
            // 
            this.btnSwitchProduct.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSwitchProduct.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnSwitchProduct.ForeColor = System.Drawing.Color.White;
            this.btnSwitchProduct.Location = new System.Drawing.Point(720, 10);
            this.btnSwitchProduct.Name = "btnSwitchProduct";
            this.btnSwitchProduct.Size = new System.Drawing.Size(250, 100);
            this.btnSwitchProduct.TabIndex = 4;
            this.btnSwitchProduct.Text = "切換產品";
            this.btnSwitchProduct.UseVisualStyleBackColor = false;
            this.btnSwitchProduct.Click += new System.EventHandler(this.btnSwitchProduct_Click);
            // 
            // lblDaysOffset
            // 
            this.lblDaysOffset.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblDaysOffset.Location = new System.Drawing.Point(420, 45);
            this.lblDaysOffset.Name = "lblDaysOffset";
            this.lblDaysOffset.Size = new System.Drawing.Size(300, 25);
            this.lblDaysOffset.TabIndex = 3;
            this.lblDaysOffset.Text = "保質期: 365 天";
            // 
            // lblStandardDate
            // 
            this.lblStandardDate.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblStandardDate.Location = new System.Drawing.Point(10, 75);
            this.lblStandardDate.Name = "lblStandardDate";
            this.lblStandardDate.Size = new System.Drawing.Size(400, 25);
            this.lblStandardDate.TabIndex = 2;
            this.lblStandardDate.Text = "標準日期: 2025.11.17";
            // 
            // lblProductCode
            // 
            this.lblProductCode.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblProductCode.Location = new System.Drawing.Point(10, 45);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(400, 25);
            this.lblProductCode.TabIndex = 1;
            this.lblProductCode.Text = "產品代碼: M1216XX";
            // 
            // lblCurrentProduct
            // 
            this.lblCurrentProduct.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCurrentProduct.Location = new System.Drawing.Point(10, 10);
            this.lblCurrentProduct.Name = "lblCurrentProduct";
            this.lblCurrentProduct.Size = new System.Drawing.Size(700, 30);
            this.lblCurrentProduct.TabIndex = 0;
            this.lblCurrentProduct.Text = "當前產品: 巷口乾麵";
            // 
            // tabPageVisionMaster
            // 
            this.tabPageVisionMaster.Controls.Add(this.vmMainViewConfigControl1);
            this.tabPageVisionMaster.Controls.Add(this.vmGlobalToolControl1);
            this.tabPageVisionMaster.Controls.Add(this.panelVMButtons);
            this.tabPageVisionMaster.Location = new System.Drawing.Point(4, 39);
            this.tabPageVisionMaster.Name = "tabPageVisionMaster";
            this.tabPageVisionMaster.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVisionMaster.Size = new System.Drawing.Size(1912, 1037);
            this.tabPageVisionMaster.TabIndex = 1;
            this.tabPageVisionMaster.Text = "VisionMaster 設定";
            this.tabPageVisionMaster.UseVisualStyleBackColor = true;
            // 
            // vmMainViewConfigControl1
            // 
            this.vmMainViewConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmMainViewConfigControl1.Location = new System.Drawing.Point(63, 63);
            this.vmMainViewConfigControl1.Name = "vmMainViewConfigControl1";
            this.vmMainViewConfigControl1.Size = new System.Drawing.Size(1846, 971);
            this.vmMainViewConfigControl1.TabIndex = 2;
// TODO: 無法產生 '' 的程式碼 (原因為發生例外狀況 '無效的基本類型: System.IntPtr。請考慮使用 CodeObjectCreateExpression。')。
            // 
            // vmGlobalToolControl1
            // 
            this.vmGlobalToolControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.vmGlobalToolControl1.Location = new System.Drawing.Point(3, 63);
            this.vmGlobalToolControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vmGlobalToolControl1.Name = "vmGlobalToolControl1";
            this.vmGlobalToolControl1.Size = new System.Drawing.Size(60, 971);
            this.vmGlobalToolControl1.TabIndex = 1;
            // 
            // panelVMButtons
            // 
            this.panelVMButtons.Controls.Add(this.btnSaveSolutionAs);
            this.panelVMButtons.Controls.Add(this.btnSaveSolution);
            this.panelVMButtons.Controls.Add(this.btnLoadSolution);
            this.panelVMButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVMButtons.Location = new System.Drawing.Point(3, 3);
            this.panelVMButtons.Name = "panelVMButtons";
            this.panelVMButtons.Size = new System.Drawing.Size(1906, 60);
            this.panelVMButtons.TabIndex = 0;
            // 
            // btnSaveSolutionAs
            // 
            this.btnSaveSolutionAs.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnSaveSolutionAs.Location = new System.Drawing.Point(390, 10);
            this.btnSaveSolutionAs.Name = "btnSaveSolutionAs";
            this.btnSaveSolutionAs.Size = new System.Drawing.Size(180, 40);
            this.btnSaveSolutionAs.TabIndex = 2;
            this.btnSaveSolutionAs.Text = "另存方案...";
            this.btnSaveSolutionAs.UseVisualStyleBackColor = true;
            this.btnSaveSolutionAs.Click += new System.EventHandler(this.btnSaveSolutionAs_Click);
            // 
            // btnSaveSolution
            // 
            this.btnSaveSolution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnSaveSolution.Location = new System.Drawing.Point(200, 10);
            this.btnSaveSolution.Name = "btnSaveSolution";
            this.btnSaveSolution.Size = new System.Drawing.Size(180, 40);
            this.btnSaveSolution.TabIndex = 1;
            this.btnSaveSolution.Text = "儲存方案";
            this.btnSaveSolution.UseVisualStyleBackColor = true;
            this.btnSaveSolution.Click += new System.EventHandler(this.btnSaveSolution_Click);
            // 
            // btnLoadSolution
            // 
            this.btnLoadSolution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnLoadSolution.Location = new System.Drawing.Point(10, 10);
            this.btnLoadSolution.Name = "btnLoadSolution";
            this.btnLoadSolution.Size = new System.Drawing.Size(180, 40);
            this.btnLoadSolution.TabIndex = 0;
            this.btnLoadSolution.Text = "載入方案...";
            this.btnLoadSolution.UseVisualStyleBackColor = true;
            this.btnLoadSolution.Click += new System.EventHandler(this.btnLoadSolution_Click);
            // 
            // tabPageProduct
            // 
            this.tabPageProduct.Controls.Add(this.dgvProducts);
            this.tabPageProduct.Controls.Add(this.panelProductButtons);
            this.tabPageProduct.Location = new System.Drawing.Point(4, 39);
            this.tabPageProduct.Name = "tabPageProduct";
            this.tabPageProduct.Size = new System.Drawing.Size(1912, 1037);
            this.tabPageProduct.TabIndex = 2;
            this.tabPageProduct.Text = "產品設定";
            this.tabPageProduct.UseVisualStyleBackColor = true;
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.ColumnHeadersHeight = 34;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.Location = new System.Drawing.Point(0, 60);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1912, 977);
            this.dgvProducts.TabIndex = 1;
            // 
            // panelProductButtons
            // 
            this.panelProductButtons.Controls.Add(this.btnSaveProducts);
            this.panelProductButtons.Controls.Add(this.btnDeleteProduct);
            this.panelProductButtons.Controls.Add(this.btnAddProduct);
            this.panelProductButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProductButtons.Location = new System.Drawing.Point(0, 0);
            this.panelProductButtons.Name = "panelProductButtons";
            this.panelProductButtons.Size = new System.Drawing.Size(1912, 60);
            this.panelProductButtons.TabIndex = 0;
            // 
            // btnSaveProducts
            // 
            this.btnSaveProducts.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnSaveProducts.Location = new System.Drawing.Point(330, 10);
            this.btnSaveProducts.Name = "btnSaveProducts";
            this.btnSaveProducts.Size = new System.Drawing.Size(150, 40);
            this.btnSaveProducts.TabIndex = 2;
            this.btnSaveProducts.Text = "儲存配置";
            this.btnSaveProducts.UseVisualStyleBackColor = true;
            this.btnSaveProducts.Click += new System.EventHandler(this.btnSaveProducts_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteProduct.Location = new System.Drawing.Point(170, 10);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteProduct.TabIndex = 1;
            this.btnDeleteProduct.Text = "刪除產品";
            this.btnDeleteProduct.UseVisualStyleBackColor = true;
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddProduct.Location = new System.Drawing.Point(10, 10);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(150, 40);
            this.btnAddProduct.TabIndex = 0;
            this.btnAddProduct.Text = "新增產品";
            this.btnAddProduct.UseVisualStyleBackColor = true;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // tabPageSystem
            // 
            this.tabPageSystem.Controls.Add(this.btnViewLog);
            this.tabPageSystem.Controls.Add(this.btnApplySettings);
            this.tabPageSystem.Controls.Add(this.groupBoxPaths);
            this.tabPageSystem.Controls.Add(this.groupBoxPLC);
            this.tabPageSystem.Location = new System.Drawing.Point(4, 39);
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.Size = new System.Drawing.Size(1912, 1037);
            this.tabPageSystem.TabIndex = 3;
            this.tabPageSystem.Text = "系統設定";
            this.tabPageSystem.UseVisualStyleBackColor = true;
            // 
            // btnViewLog
            // 
            this.btnViewLog.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnViewLog.Location = new System.Drawing.Point(240, 410);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(200, 50);
            this.btnViewLog.TabIndex = 3;
            this.btnViewLog.Text = "查看日誌";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnApplySettings.Location = new System.Drawing.Point(20, 410);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(200, 50);
            this.btnApplySettings.TabIndex = 2;
            this.btnApplySettings.Text = "套用設定";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // groupBoxPaths
            // 
            this.groupBoxPaths.Controls.Add(this.txtNGImagePath);
            this.groupBoxPaths.Controls.Add(this.lblNGImagePath);
            this.groupBoxPaths.Controls.Add(this.txtSolutionPath);
            this.groupBoxPaths.Controls.Add(this.lblSolutionPath);
            this.groupBoxPaths.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxPaths.Location = new System.Drawing.Point(20, 240);
            this.groupBoxPaths.Name = "groupBoxPaths";
            this.groupBoxPaths.Size = new System.Drawing.Size(800, 150);
            this.groupBoxPaths.TabIndex = 1;
            this.groupBoxPaths.TabStop = false;
            this.groupBoxPaths.Text = "路徑設定";
            // 
            // txtNGImagePath
            // 
            this.txtNGImagePath.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txtNGImagePath.Location = new System.Drawing.Point(150, 82);
            this.txtNGImagePath.Name = "txtNGImagePath";
            this.txtNGImagePath.ReadOnly = true;
            this.txtNGImagePath.Size = new System.Drawing.Size(630, 34);
            this.txtNGImagePath.TabIndex = 3;
            // 
            // lblNGImagePath
            // 
            this.lblNGImagePath.AutoSize = true;
            this.lblNGImagePath.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblNGImagePath.Location = new System.Drawing.Point(20, 85);
            this.lblNGImagePath.Name = "lblNGImagePath";
            this.lblNGImagePath.Size = new System.Drawing.Size(145, 28);
            this.lblNGImagePath.TabIndex = 2;
            this.lblNGImagePath.Text = "NG 圖像路徑:";
            // 
            // txtSolutionPath
            // 
            this.txtSolutionPath.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txtSolutionPath.Location = new System.Drawing.Point(150, 37);
            this.txtSolutionPath.Name = "txtSolutionPath";
            this.txtSolutionPath.ReadOnly = true;
            this.txtSolutionPath.Size = new System.Drawing.Size(630, 34);
            this.txtSolutionPath.TabIndex = 1;
            // 
            // lblSolutionPath
            // 
            this.lblSolutionPath.AutoSize = true;
            this.lblSolutionPath.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblSolutionPath.Location = new System.Drawing.Point(20, 40);
            this.lblSolutionPath.Name = "lblSolutionPath";
            this.lblSolutionPath.Size = new System.Drawing.Size(105, 28);
            this.lblSolutionPath.TabIndex = 0;
            this.lblSolutionPath.Text = "方案路徑:";
            // 
            // groupBoxPLC
            // 
            this.groupBoxPLC.Controls.Add(this.btnTestPLC);
            this.groupBoxPLC.Controls.Add(this.chkPlcEnabled);
            this.groupBoxPLC.Controls.Add(this.txtPlcPort);
            this.groupBoxPLC.Controls.Add(this.lblPlcPort);
            this.groupBoxPLC.Controls.Add(this.txtPlcIP);
            this.groupBoxPLC.Controls.Add(this.lblPlcIP);
            this.groupBoxPLC.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxPLC.Location = new System.Drawing.Point(20, 20);
            this.groupBoxPLC.Name = "groupBoxPLC";
            this.groupBoxPLC.Size = new System.Drawing.Size(600, 200);
            this.groupBoxPLC.TabIndex = 0;
            this.groupBoxPLC.TabStop = false;
            this.groupBoxPLC.Text = "PLC 設定";
            // 
            // btnTestPLC
            // 
            this.btnTestPLC.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.btnTestPLC.Location = new System.Drawing.Point(400, 37);
            this.btnTestPLC.Name = "btnTestPLC";
            this.btnTestPLC.Size = new System.Drawing.Size(180, 120);
            this.btnTestPLC.TabIndex = 5;
            this.btnTestPLC.Text = "測試連接";
            this.btnTestPLC.UseVisualStyleBackColor = true;
            this.btnTestPLC.Click += new System.EventHandler(this.btnTestPLC_Click);
            // 
            // chkPlcEnabled
            // 
            this.chkPlcEnabled.AutoSize = true;
            this.chkPlcEnabled.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.chkPlcEnabled.Location = new System.Drawing.Point(24, 130);
            this.chkPlcEnabled.Name = "chkPlcEnabled";
            this.chkPlcEnabled.Size = new System.Drawing.Size(127, 32);
            this.chkPlcEnabled.TabIndex = 4;
            this.chkPlcEnabled.Text = "啟用 PLC";
            this.chkPlcEnabled.UseVisualStyleBackColor = true;
            // 
            // txtPlcPort
            // 
            this.txtPlcPort.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.txtPlcPort.Location = new System.Drawing.Point(150, 82);
            this.txtPlcPort.Name = "txtPlcPort";
            this.txtPlcPort.Size = new System.Drawing.Size(200, 37);
            this.txtPlcPort.TabIndex = 3;
            // 
            // lblPlcPort
            // 
            this.lblPlcPort.AutoSize = true;
            this.lblPlcPort.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblPlcPort.Location = new System.Drawing.Point(20, 85);
            this.lblPlcPort.Name = "lblPlcPort";
            this.lblPlcPort.Size = new System.Drawing.Size(83, 28);
            this.lblPlcPort.TabIndex = 2;
            this.lblPlcPort.Text = "通訊埠:";
            // 
            // txtPlcIP
            // 
            this.txtPlcIP.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.txtPlcIP.Location = new System.Drawing.Point(150, 37);
            this.txtPlcIP.Name = "txtPlcIP";
            this.txtPlcIP.Size = new System.Drawing.Size(200, 37);
            this.txtPlcIP.TabIndex = 1;
            // 
            // lblPlcIP
            // 
            this.lblPlcIP.AutoSize = true;
            this.lblPlcIP.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.lblPlcIP.Location = new System.Drawing.Point(20, 40);
            this.lblPlcIP.Name = "lblPlcIP";
            this.lblPlcIP.Size = new System.Drawing.Size(86, 28);
            this.lblPlcIP.TabIndex = 0;
            this.lblPlcIP.Text = "IP 位址:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisionMaster 日期檢測系統";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.tabPageDetection.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picNGImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLiveImage)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetectionLog)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            this.panelProduct.ResumeLayout(false);
            this.tabPageVisionMaster.ResumeLayout(false);
            this.panelVMButtons.ResumeLayout(false);
            this.tabPageProduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.panelProductButtons.ResumeLayout(false);
            this.tabPageSystem.ResumeLayout(false);
            this.groupBoxPaths.ResumeLayout(false);
            this.groupBoxPaths.PerformLayout();
            this.groupBoxPLC.ResumeLayout(false);
            this.groupBoxPLC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // TabControl
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDetection;
        private System.Windows.Forms.TabPage tabPageVisionMaster;
        private System.Windows.Forms.TabPage tabPageProduct;
        private System.Windows.Forms.TabPage tabPageSystem;

        // 檢測頁面
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.PictureBox picLiveImage;
        private System.Windows.Forms.PictureBox picNGImage;
        private System.Windows.Forms.Label lblLiveTitle;
        private System.Windows.Forms.Label lblNGTitle;

        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelProduct;
        private System.Windows.Forms.Label lblCurrentProduct;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblStandardDate;
        private System.Windows.Forms.Label lblDaysOffset;
        private System.Windows.Forms.Button btnSwitchProduct;

        private System.Windows.Forms.Panel panelStatistics;
        private System.Windows.Forms.Label lblOKCount;
        private System.Windows.Forms.Label lblNGCount;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblYieldRate;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;

        private System.Windows.Forms.DataGridView dgvDetectionLog;
        private System.Windows.Forms.TextBox txtDetectionLog;

        // VisionMaster 設定頁面
        private System.Windows.Forms.Panel panelVMButtons;
        private System.Windows.Forms.Button btnLoadSolution;
        private System.Windows.Forms.Button btnSaveSolution;
        private System.Windows.Forms.Button btnSaveSolutionAs;
        private VMControls.Winform.Release.VmGlobalToolControl vmGlobalToolControl1;
        private VMControls.Winform.Release.VmMainViewConfigControl vmMainViewConfigControl1;

        // 產品設定頁面
        private System.Windows.Forms.Panel panelProductButtons;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Button btnSaveProducts;
        private System.Windows.Forms.DataGridView dgvProducts;

        // 系統設定頁面
        private System.Windows.Forms.GroupBox groupBoxPLC;
        private System.Windows.Forms.Label lblPlcIP;
        private System.Windows.Forms.TextBox txtPlcIP;
        private System.Windows.Forms.Label lblPlcPort;
        private System.Windows.Forms.TextBox txtPlcPort;
        private System.Windows.Forms.CheckBox chkPlcEnabled;
        private System.Windows.Forms.Button btnTestPLC;

        private System.Windows.Forms.GroupBox groupBoxPaths;
        private System.Windows.Forms.Label lblSolutionPath;
        private System.Windows.Forms.TextBox txtSolutionPath;
        private System.Windows.Forms.Label lblNGImagePath;
        private System.Windows.Forms.TextBox txtNGImagePath;

        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.Button btnViewLog;
    }
}