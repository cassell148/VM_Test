using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision_Master.Forms
{
    /// <summary>
    /// 載入畫面 - 顯示應用程式初始化進度
    /// </summary>
    public partial class LoadingForm : Form
    {
        private readonly Label _label;
        private readonly ProgressBar _progressBar;
        private const int BorderWidth = 4;
        private bool _allowClose;

        public LoadingForm()
        {
            InitializeComponent();

            // 視窗設定
            this.Text = "載入中...";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 140);
            this.UseWaitCursor = true;
            this.DoubleBuffered = true;
            this.Padding = new Padding(BorderWidth);

            // 建立版面配置
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(15)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            _label = new Label
            {
                ForeColor = SystemColors.ControlText,
                Font = new Font("微軟正黑體", 12, FontStyle.Bold),
                Text = "正在載入，請稍候...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseWaitCursor = true
            };

            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Fill,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                UseWaitCursor = true
            };

            table.Controls.Add(_label, 0, 0);
            table.Controls.Add(_progressBar, 0, 1);
            this.Controls.Add(table);
        }

        /// <summary>
        /// 更新顯示訊息（執行緒安全）
        /// </summary>
        public void UpdateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdateMessage), message);
                return;
            }

            _label.Text = message;
        }

        /// <summary>
        /// 安全關閉視窗
        /// </summary>
        public void CloseSafely()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(CloseSafely));
                return;
            }

            _allowClose = true;
            this.Close();
        }

        /// <summary>
        /// 繪製自訂邊框
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(Color.DarkBlue, BorderWidth))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                var rect = new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        /// <summary>
        /// 阻止使用者手動關閉
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }
    }
}
