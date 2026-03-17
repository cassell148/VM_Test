using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision_Master.Core;
using Vision_Master.Forms;
using VM.Core;              // 加入 VM SDK 命名空間
using VM.PlatformSDKCS;     // 加入 VM SDK 命名空間

namespace Vision_Master
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式版本號
        /// </summary>
        public static readonly string Version = "1.0.0";

        /// <summary>
        /// 配置管理器
        /// </summary>
        public static ConfigManager Config { get; private set; }

        private static Mutex _mutex = null;
        private static volatile bool _exitApp = false;

        /// <summary>
        /// 應用程式的主要進入點
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string appName = "Vision_Master_UniqueApp";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("程式已在執行中，無法重複啟動。",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 顯示載入畫面進行初始化
            using (var loadingForm = new LoadingForm())
            {
                // 在背景執行緒進行初始化
                Task.Run(() => InitializeApplication(loadingForm));

                // 運行載入畫面（獨立訊息迴圈）
                Application.Run(loadingForm);
            }

            // 檢查是否因初始化失敗而需要退出
            if (_exitApp)
            {
                return;
            }

            // 初始化成功，啟動主視窗
            Logger.Write($"程式啟動，版本號: {Version}");
            Application.Run(new MainForm());

            // 程式結束時釋放 Mutex
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }

        /// <summary>
        /// 初始化應用程式（在背景執行緒執行）
        /// </summary>
        private static void InitializeApplication(LoadingForm loadingForm)
        {
            try
            {
                // 步驟 1: 檢查配置檔案
                loadingForm.UpdateMessage("正在檢查配置檔案...");
                Thread.Sleep(500);

                string configPath = Path.Combine(Application.StartupPath, "Config.ini");
                if (!File.Exists(configPath))
                {
                    MessageBox.Show($"找不到配置檔案：{configPath}\r\n程式即將關閉。",
                        "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _exitApp = true;
                    return;
                }

                // 步驟 2: 載入配置
                loadingForm.UpdateMessage("正在讀取配置檔案...");
                Thread.Sleep(500);

                Config = new ConfigManager(configPath);
                if (!Config.Load())
                {
                    MessageBox.Show("讀取配置檔案失敗，請檢查檔案格式。\r\n程式即將關閉。",
                        "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _exitApp = true;
                    return;
                }

                loadingForm.UpdateMessage($"配置讀取完成\r\nPLC IP: {Config.PlcIp}:{Config.PlcPort}");
                Thread.Sleep(800);

                // 步驟 3: 初始化日誌系統
                loadingForm.UpdateMessage("正在初始化日誌系統...");
                Thread.Sleep(500);

                Logger.Initialize(Path.Combine(Application.StartupPath, "Logs"));
                Logger.Write("應用程式初始化開始");

                // ========== 關鍵修改：步驟 4 - 初始化 VisionMaster SDK ==========
                loadingForm.UpdateMessage("正在初始化 VisionMaster SDK...");
                Thread.Sleep(500);



                // 步驟 5: 檢查 VM Solution 檔案
                loadingForm.UpdateMessage("正在檢查視覺方案檔案...");
                Thread.Sleep(500);

                string solutionPath = Path.Combine(Application.StartupPath, "solution.solw");
                if (!File.Exists(solutionPath))
                {
                    Logger.Write($"找不到視覺方案檔案: {solutionPath}", Logger.LogLevel.Warning);
                    loadingForm.UpdateMessage("視覺方案檔案不存在\r\n請稍後手動載入");
                    Thread.Sleep(1500);
                }
                else
                {
                    Logger.Write($"找到視覺方案檔案: {solutionPath}");
                }

                // 步驟 6: 完成初始化
                loadingForm.UpdateMessage("初始化完成，正在啟動主視窗...");
                Thread.Sleep(800);

                Logger.Write("應用程式初始化完成");
            }
            catch (Exception ex)
            {
                Logger.Write($"初始化失敗：{ex.Message}", Logger.LogLevel.Error);
                MessageBox.Show($"初始化失敗：{ex.Message}\r\n程式即將關閉。",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _exitApp = true;
            }
            finally
            {
                // 關閉載入畫面
                loadingForm.CloseSafely();
            }
        }
    }
}