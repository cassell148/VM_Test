using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision_Master.Core;

namespace Vision_Master.Forms
{
    /// <summary>
    /// 日誌檢視器
    /// </summary>
    public partial class LogViewerForm : Form
    {
        private readonly DateTimePicker _datePicker;
        private readonly ComboBox _comboFiles;
        private readonly RichTextBox _richTextLog;

        public LogViewerForm()
        {
            InitializeComponent();

            this.Text = "日誌檢視器";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _datePicker = new DateTimePicker
            {
                Font = new Font("微軟正黑體", 11F),
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy/MM/dd",
                Value = DateTime.Now
            };
            _datePicker.ValueChanged += DatePicker_ValueChanged;

            _comboFiles = new ComboBox
            {
                Font = new Font("微軟正黑體", 11F),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _comboFiles.SelectedIndexChanged += ComboFiles_SelectedIndexChanged;

            _richTextLog = new RichTextBox
            {
                Font = new Font("Consolas", 10F),
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both
            };

            table.Controls.Add(_datePicker, 0, 0);
            table.Controls.Add(_comboFiles, 1, 0);
            table.Controls.Add(_richTextLog, 0, 1);
            table.SetColumnSpan(_richTextLog, 2);

            this.Controls.Add(table);

            // 初始載入
            DatePicker_ValueChanged(null, null);
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            _richTextLog.Clear();
            string dateString = _datePicker.Value.ToString("yyyyMMdd");

            string logBasePath = Logger.GetLogBasePath();
            if (string.IsNullOrEmpty(logBasePath))
            {
                _comboFiles.Items.Clear();
                _comboFiles.Text = "日誌系統未初始化";
                return;
            }

            string logPath = Path.Combine(logBasePath, dateString);

            try
            {
                if (Directory.Exists(logPath))
                {
                    var files = Directory
                        .GetFiles(logPath, "*.txt", SearchOption.TopDirectoryOnly)
                        .OrderBy(f => File.GetLastWriteTime(f))
                        .ToList();

                    _comboFiles.Items.Clear();

                    foreach (var file in files)
                    {
                        _comboFiles.Items.Add(Path.GetFileName(file));
                    }

                    if (_comboFiles.Items.Count > 0)
                    {
                        // 預設選擇最新的檔案
                        _comboFiles.SelectedIndex = _comboFiles.Items.Count - 1;
                    }
                    else
                    {
                        _comboFiles.Text = "無日誌檔案";
                    }
                }
                else
                {
                    _comboFiles.Items.Clear();
                    _comboFiles.Text = "無日誌檔案";
                }
            }
            catch (Exception ex)
            {
                _comboFiles.Items.Clear();
                _comboFiles.Text = $"載入失敗：{ex.Message}";
            }
        }

        private async void ComboFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_comboFiles.SelectedItem == null)
                return;

            string fileName = _comboFiles.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string dateString = _datePicker.Value.ToString("yyyyMMdd");
            string logBasePath = Logger.GetLogBasePath();
            string logPath = Path.Combine(logBasePath, dateString, fileName);

            if (File.Exists(logPath))
            {
                try
                {
                    _richTextLog.Text = "正在載入...";

                    // 非同步讀取檔案（共享讀取模式）
                    string content = await Task.Run(() =>
                    {
                        using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var sr = new StreamReader(fs, Encoding.UTF8, true))
                        {
                            return sr.ReadToEnd();
                        }
                    });

                    _richTextLog.Text = content;

                    // 捲動到底部
                    _richTextLog.SelectionStart = _richTextLog.TextLength;
                    _richTextLog.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    _richTextLog.Text = $"讀取日誌檔案失敗：{ex.Message}";
                }
            }
            else
            {
                _richTextLog.Text = "日誌檔案不存在";
            }
        }
    }
}
