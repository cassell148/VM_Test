using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vision_Master.Core
{
    /// <summary>
    /// 日誌系統 - 負責記錄應用程式運行日誌
    /// </summary>
    public static class Logger
    {
        private static string _logBasePath;
        private static RichTextBox _boundRichTextBox;
        private static readonly object _uiLock = new object();
        private static readonly object _fileLock = new object();

        /// <summary>
        /// 日誌等級
        /// </summary>
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// 初始化日誌系統
        /// </summary>
        public static void Initialize(string logBasePath)
        {
            _logBasePath = logBasePath;

            // 確保日誌目錄存在
            if (!Directory.Exists(_logBasePath))
            {
                Directory.CreateDirectory(_logBasePath);
            }
        }

        /// <summary>
        /// 綁定 RichTextBox 以同步顯示日誌
        /// </summary>
        public static void BindRichTextBox(RichTextBox richTextBox)
        {
            lock (_uiLock)
            {
                _boundRichTextBox = richTextBox;
                if (_boundRichTextBox != null)
                {
                    _boundRichTextBox.ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// 寫入日誌
        /// </summary>
        public static void Write(string message, LogLevel level = LogLevel.Info)
        {
            if (string.IsNullOrEmpty(message))
                return;

            // 同時寫入檔案和 UI
            WriteToFile(message, level);
            WriteToUI(message, level);
        }

        /// <summary>
        /// 寫入日誌檔案
        /// </summary>
        private static void WriteToFile(string message, LogLevel level)
        {
            if (string.IsNullOrEmpty(_logBasePath))
                return;

            try
            {
                // 日誌檔案路徑：Logs\yyyyMMdd\log_yyyyMMddHH.txt
                string dateFolder = DateTime.Now.ToString("yyyyMMdd");
                string folderPath = Path.Combine(_logBasePath, dateFolder);

                // 確保資料夾存在
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = $"log_{DateTime.Now:yyyyMMddHH}.txt";
                string filePath = Path.Combine(folderPath, fileName);

                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

                // 使用鎖確保執行緒安全
                lock (_fileLock)
                {
                    File.AppendAllText(filePath, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // 避免日誌系統本身造成程式崩潰
                MessageBox.Show($"寫入日誌時發生錯誤: {ex.Message}",
                    "日誌錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 寫入 UI 顯示
        /// </summary>
        private static void WriteToUI(string message, LogLevel level)
        {
            RichTextBox rtb;

            lock (_uiLock)
            {
                rtb = _boundRichTextBox;
            }

            if (rtb == null || rtb.IsDisposed)
                return;

            string text = $"[{DateTime.Now:HH:mm:ss}] {message}";

            Action append = () =>
            {
                if (rtb.IsDisposed)
                    return;

                try
                {
                    // 根據等級設定顏色
                    Color color;
                    switch (level)
                    {
                        case LogLevel.Error:
                            color = Color.Red;
                            break;
                        case LogLevel.Warning:
                            color = Color.DarkOrange;
                            break;
                        default:
                            color = Color.Black;
                            break;
                    }

                    // 插入到第一行（頂部）
                    rtb.SelectionStart = 0;
                    rtb.SelectionLength = 0;
                    rtb.SelectionColor = color;
                    rtb.SelectedText = text + Environment.NewLine;
                    rtb.SelectionColor = rtb.ForeColor;

                    // 限制最多 1000 行
                    if (rtb.Lines.Length > 1000)
                    {
                        int lastLineIndex = rtb.Lines.Length - 1;
                        int start = rtb.GetFirstCharIndexFromLine(lastLineIndex);
                        if (start >= 0)
                        {
                            rtb.SelectionStart = start;
                            rtb.SelectionLength = rtb.TextLength - start;
                            rtb.SelectedText = string.Empty;
                        }
                    }

                    // 保持在頂部
                    rtb.SelectionStart = 0;
                    rtb.SelectionLength = 0;
                    rtb.ScrollToCaret();
                }
                catch
                {
                    // 忽略 UI 操作異常
                }
            };

            try
            {
                if (rtb.InvokeRequired)
                    rtb.BeginInvoke(append);
                else
                    append();
            }
            catch
            {
                // 忽略 UI 關閉造成的異常
            }
        }

        /// <summary>
        /// 取得日誌基礎路徑
        /// </summary>
        public static string GetLogBasePath()
        {
            return _logBasePath;
        }
    }
}
