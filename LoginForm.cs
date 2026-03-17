using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision_Master.Forms
{
    /// <summary>
    /// 登入視窗
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly TextBox _txtAccount;
        private readonly TextBox _txtPassword;
        private const int BorderWidth = 4;

        public LoginForm()
        {
            InitializeComponent();

            this.Text = "權限登入";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(380, 220);
            this.BackColor = SystemColors.ControlLight;
            this.Padding = new Padding(BorderWidth);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(15)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            var lblAccount = new Label
            {
                Font = new Font("微軟正黑體", 12, FontStyle.Bold),
                Text = "帳號：",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblPassword = new Label
            {
                Font = new Font("微軟正黑體", 12, FontStyle.Bold),
                Text = "密碼：",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };

            _txtAccount = new TextBox
            {
                Font = new Font("微軟正黑體", 12),
                Dock = DockStyle.Fill
            };

            _txtPassword = new TextBox
            {
                PasswordChar = '●',
                Font = new Font("微軟正黑體", 12),
                Dock = DockStyle.Fill
            };

            var buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var btnLogin = new Button
            {
                Text = "登入",
                Font = new Font("微軟正黑體", 12, FontStyle.Bold),
                Dock = DockStyle.Fill
            };

            var btnCancel = new Button
            {
                Text = "取消",
                Font = new Font("微軟正黑體", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.Cancel
            };

            buttonPanel.Controls.Add(btnLogin, 0, 0);
            buttonPanel.Controls.Add(btnCancel, 1, 0);

            table.Controls.Add(lblAccount, 0, 0);
            table.Controls.Add(lblPassword, 0, 1);
            table.Controls.Add(_txtAccount, 1, 0);
            table.Controls.Add(_txtPassword, 1, 1);
            table.Controls.Add(buttonPanel, 0, 2);
            table.SetColumnSpan(buttonPanel, 2);

            this.Controls.Add(table);
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;

            // 事件處理
            btnLogin.Click += BtnLogin_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            _txtPassword.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    btnLogin.PerformClick();
                    e.Handled = true;
                }
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = _txtAccount.Text.Trim();
            string password = _txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("請輸入帳號和密碼。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 驗證登入
            if (Program.Config.ValidateLogin(username, password))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("帳號或密碼錯誤，請重新輸入。", "登入失敗",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _txtPassword.Clear();
                _txtPassword.Focus();
            }
        }

        /// <summary>
        /// 開啟視窗後自動聚焦到帳號欄位
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.BeginInvoke(new Action(() =>
            {
                if (!_txtAccount.IsDisposed && _txtAccount.CanFocus)
                {
                    _txtAccount.Focus();
                    _txtAccount.SelectAll();
                }
            }));
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
    }
}
