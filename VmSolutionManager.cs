using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision_Master.Forms;
using VM.Core;
using VM.PlatformSDKCS;

namespace Vision_Master.Core
{
    /// <summary>
    /// 視覺方案管理器 - 負責管理 VisionMaster Solution
    /// </summary>
    public static class VmSolutionManager
    {
        #region === 載入固定路徑的視覺方案 ===

        /// <summary>
        /// 載入固定路徑的視覺方案
        /// </summary>
        public static async Task<bool> LoadSolution()
        {
            try
            {
                var solutionPath = Path.Combine(Application.StartupPath, "solution.solw");

                Logger.Write($"[LoadSolution] 準備載入方案");
                Logger.Write($"[LoadSolution] 方案路徑: {solutionPath}");

                // 檢查檔案是否存在
                if (!File.Exists(solutionPath))
                {
                    Logger.Write($"[LoadSolution] 找不到視覺方案檔案: {solutionPath}", Logger.LogLevel.Warning);
                    MessageBox.Show($"找不到視覺方案檔案！\r\n路徑：{solutionPath}\r\n\r\n請將 .solw 檔案放入程式相同目錄。",
                        "檔案遺失", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                Logger.Write($"[LoadSolution] 開始載入方案...");

                // 直接載入，不做任何預先檢查
                await Task.Run(() => VmSolution.Load(solutionPath));

                Logger.Write($"[LoadSolution] VmSolution.Load() 執行完成");

                // Load 完成後才檢查 Instance
                if (VmSolution.Instance == null)
                {
                    Logger.Write("[LoadSolution] 載入後 Instance 仍為 null", Logger.LogLevel.Error);
                    MessageBox.Show("載入視覺方案失敗：Instance 為 null\r\n\r\n可能原因：\r\n1. 方案檔案損壞\r\n2. SDK 版本不相容\r\n3. 缺少必要的依賴項",
                        "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Logger.Write($"[LoadSolution] 載入成功");
                Logger.Write($"[LoadSolution] 方案路徑: {VmSolution.Instance.SolutionPath}");
                return true;
            }
            catch (VmException ex)
            {
                string error = $"載入視覺方案失敗 (VmException: 0x{ex.errorCode:X}): {ex.errorMessage}";
                Logger.Write($"[LoadSolution] {error}", Logger.LogLevel.Error);
                Logger.Write($"[LoadSolution] 堆疊追蹤: {ex.StackTrace}", Logger.LogLevel.Error);
                MessageBox.Show($"{error}\r\n\r\n請檢查：\r\n1. 方案檔案是否正確\r\n2. SDK 授權是否有效\r\n3. 相關硬體是否連接",
                    "VmException 錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                string error = $"載入視覺方案失敗: {ex.Message}";
                Logger.Write($"[LoadSolution] {error}", Logger.LogLevel.Error);
                Logger.Write($"[LoadSolution] 例外類型: {ex.GetType().FullName}", Logger.LogLevel.Error);
                Logger.Write($"[LoadSolution] 堆疊追蹤: {ex.StackTrace}", Logger.LogLevel.Error);
                if (ex.InnerException != null)
                {
                    Logger.Write($"[LoadSolution] 內部例外: {ex.InnerException.Message}", Logger.LogLevel.Error);
                }
                MessageBox.Show($"{error}\r\n\r\n詳細資訊請查看日誌檔案。",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region === 選擇檔案並載入視覺方案 ===

        /// <summary>
        /// 選擇檔案並載入視覺方案
        /// </summary>
        public static async Task<bool> LoadSolutionFromFile()
        {
            string solutionPath = string.Empty;
            var tcs = new TaskCompletionSource<uint>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "視覺方案檔案 (*.solw)|*.solw|所有檔案 (*.*)|*.*",
                Title = "開啟 VM 視覺方案檔案",
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                solutionPath = openFileDialog.FileName;
                Logger.Write($"使用者選擇載入視覺方案: {solutionPath}");

                VmSolution.OnSolutionLoadEndEvent += OnSolutionLoaded;

                try
                {
                    Logger.Write("[LoadSolutionFromFile] 開始非同步載入方案");

                    await Task.Run(() => VmSolution.Load(solutionPath));

                    Logger.Write("[LoadSolutionFromFile] Load 方法執行完成，等待載入完成事件");

                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
                    var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                    if (completedTask != tcs.Task)
                    {
                        Logger.Write("載入視覺方案逾時 (30秒)", Logger.LogLevel.Warning);
                        MessageBox.Show("載入視覺方案逾時。", "逾時", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    uint result = await tcs.Task;
                    Logger.Write($"[LoadSolutionFromFile] 載入完成事件回傳，狀態碼: {result}");

                    if (result == 0)
                    {
                        Logger.Write("視覺方案載入成功");
                        MessageBox.Show("視覺方案載入成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        Logger.Write($"載入視覺方案失敗，狀態碼: {result}", Logger.LogLevel.Error);
                        MessageBox.Show($"載入視覺方案失敗 (狀態碼: {result})。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (VmException ex)
                {
                    string error = $"載入視覺方案失敗 (VmException: 0x{ex.errorCode:X}): {ex.errorMessage}";
                    Logger.Write($"[LoadSolutionFromFile] {error}", Logger.LogLevel.Error);
                    MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (Exception ex)
                {
                    string error = $"載入視覺方案失敗: {ex.Message}";
                    Logger.Write($"[LoadSolutionFromFile] {error}", Logger.LogLevel.Error);
                    Logger.Write($"[LoadSolutionFromFile] 堆疊追蹤: {ex.StackTrace}", Logger.LogLevel.Error);
                    MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    VmSolution.OnSolutionLoadEndEvent -= OnSolutionLoaded;
                }
            }

            void OnSolutionLoaded(ImvsSdkDefine.IMVS_SOLUTION_LOAD_END_INFO solutionLoadEndInfo)
            {
                Logger.Write($"[OnSolutionLoaded] 收到載入完成事件，狀態: {solutionLoadEndInfo.nStatus}");
                tcs.TrySetResult(solutionLoadEndInfo.nStatus);
            }
        }

        #endregion

        #region === 儲存與另存方案 ===

        public static bool SaveSolution()
        {
            try
            {
                if (VmSolution.Instance == null)
                {
                    Logger.Write("尚未載入視覺方案，無法儲存", Logger.LogLevel.Warning);
                    MessageBox.Show("尚未載入視覺方案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                VmSolution.Save();
                Logger.Write("視覺方案儲存成功");
                MessageBox.Show("視覺方案儲存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (VmException ex)
            {
                string error = $"儲存視覺方案失敗 (VmException: 0x{ex.errorCode:X}): {ex.errorMessage}";
                Logger.Write(error, Logger.LogLevel.Error);
                MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                string error = $"儲存視覺方案失敗: {ex.Message}";
                Logger.Write(error, Logger.LogLevel.Error);
                MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool SaveSolutionAs()
        {
            try
            {
                if (VmSolution.Instance == null)
                {
                    Logger.Write("尚未載入視覺方案，無法另存", Logger.LogLevel.Warning);
                    MessageBox.Show("尚未載入視覺方案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Title = "另存視覺方案檔案",
                    Filter = "視覺方案檔案 (*.solw)|*.solw|所有檔案 (*.*)|*.*",
                    DefaultExt = "solw",
                    FileName = "solution.solw",
                    AddExtension = true,
                    RestoreDirectory = true
                })
                {
                    if (saveDialog.ShowDialog() != DialogResult.OK)
                        return false;

                    string filePath = saveDialog.FileName;
                    if (!string.Equals(Path.GetExtension(filePath), ".solw", StringComparison.OrdinalIgnoreCase))
                    {
                        filePath = Path.ChangeExtension(filePath, ".solw");
                    }

                    VmSolution.SaveAs(filePath);
                    Logger.Write($"視覺方案另存成功: {filePath}");
                    MessageBox.Show("視覺方案另存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (VmException ex)
            {
                string error = $"另存視覺方案失敗 (VmException: 0x{ex.errorCode:X}): {ex.errorMessage}";
                Logger.Write(error, Logger.LogLevel.Error);
                MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                string error = $"另存視覺方案失敗: {ex.Message}";
                Logger.Write(error, Logger.LogLevel.Error);
                MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region === 狀態檢查與資源釋放 ===

        public static bool IsLoaded()
        {
            return VmSolution.Instance != null;
        }

        public static void Dispose()
        {
            try
            {
                if (VmSolution.Instance != null)
                {
                    VmSolution.Instance.Dispose();
                    Logger.Write("視覺方案資源已釋放");
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"釋放視覺方案資源時發生錯誤: {ex.Message}", Logger.LogLevel.Warning);
            }
        }

        #endregion
    }
}