using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vision_Master.Core;

namespace Vision_Master
{
    /// <summary>
    /// 三菱 FX3G PLC 通訊控制器
    /// </summary>
    public class FX3GPLCController : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly string _ip;
        private readonly int _port;
        private bool _isConnected;
        private readonly object _lockObj = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        // NG 訊號位址 (M1041)
        private const string NG_SIGNAL_ADDRESS = "M1041";

        public bool IsConnected
        {
            get
            {
                lock (_lockObj)
                {
                    return _isConnected && _client != null && _client.Connected;
                }
            }
        }

        public FX3GPLCController(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _isConnected = false;
        }

        /// <summary>
        /// 連接 PLC
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (IsConnected)
                {
                    Logger.Write($"[PLC] 已連接到 {_ip}:{_port}");
                    return true;
                }

                Logger.Write($"[PLC] 正在連接 {_ip}:{_port}...");

                _client = new TcpClient();
                await _client.ConnectAsync(_ip, _port);
                _stream = _client.GetStream();

                lock (_lockObj)
                {
                    _isConnected = true;
                }

                Logger.Write($"[PLC] 連接成功！");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 連接失敗: {ex.Message}");
                lock (_lockObj)
                {
                    _isConnected = false;
                }
                return false;
            }
        }

        /// <summary>
        /// 斷開 PLC 連接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                lock (_lockObj)
                {
                    if (_stream != null)
                    {
                        _stream.Close();
                        _stream.Dispose();
                        _stream = null;
                    }

                    if (_client != null)
                    {
                        _client.Close();
                        _client.Dispose();
                        _client = null;
                    }

                    _isConnected = false;
                }

                Logger.Write("[PLC] 已斷開連接");
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 斷開連接時發生錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// 發送 NG 訊號到 PLC (非同步)
        /// </summary>
        public async Task<bool> SendNGSignalAsync()
        {
            // 使用信號量確保同時只有一個操作
            await _semaphore.WaitAsync();

            try
            {
                if (!IsConnected)
                {
                    Logger.Write("[PLC] 未連接，嘗試重新連接...");
                    bool reconnected = await ConnectAsync();
                    if (!reconnected)
                    {
                        Logger.Write("[PLC] 重新連接失敗，無法發送 NG 訊號");
                        return false;
                    }
                }

                // 發送 NG 訊號 - 將 M1041 設為 ON
                Logger.Write($"[PLC] 發送 NG 訊號到 {NG_SIGNAL_ADDRESS}...");

                bool result = await WriteBitAsync(NG_SIGNAL_ADDRESS, true);

                if (result)
                {
                    Logger.Write($"[PLC] NG 訊號發送成功");

                    // 延遲後關閉訊號 (模擬脈衝)
                    await Task.Delay(100);
                    await WriteBitAsync(NG_SIGNAL_ADDRESS, false);
                    Logger.Write($"[PLC] NG 訊號已復位");
                }
                else
                {
                    Logger.Write($"[PLC] NG 訊號發送失敗");
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 發送 NG 訊號時發生錯誤: {ex.Message}");
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 寫入位元到 PLC
        /// </summary>
        private async Task<bool> WriteBitAsync(string address, bool value)
        {
            try
            {
                // 建立三菱 MC 協議命令
                // 格式: 子指令 + 起始位址 + 設備代碼 + 點數 + 資料
                byte[] command = BuildMCCommand(address, value);

                // 發送命令
                await _stream.WriteAsync(command, 0, command.Length);

                // 讀取回應
                byte[] response = new byte[11]; // MC 協議回應長度
                int bytesRead = await _stream.ReadAsync(response, 0, response.Length);

                if (bytesRead >= 11)
                {
                    // 檢查回應代碼 (D0 00 表示成功)
                    if (response[9] == 0x00 && response[10] == 0x00)
                    {
                        return true;
                    }
                    else
                    {
                        Logger.Write($"[PLC] 寫入失敗，錯誤碼: {response[9]:X2}{response[10]:X2}");
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Write($"[PLC] 寫入位元時發生錯誤: {ex.Message}");

                // 連接可能已斷開
                lock (_lockObj)
                {
                    _isConnected = false;
                }

                return false;
            }
        }

        /// <summary>
        /// 建立三菱 MC 協議命令 (用於寫入位元)
        /// </summary>
        private byte[] BuildMCCommand(string address, bool value)
        {
            // MC 協議格式 (3E 幀)
            // 50 00 (副標頭) + 00 FF (網路號/站號) + FF 03 (目標模組/單元)
            // 00 (I/O編號) + 0C 00 (監視定時器) + 0C 00 (命令長度)
            // 01 14 (子指令: 批次寫入) + 00 00 (起始位址) + 90 (設備代碼 M)
            // 01 (點數) + 10 或 00 (資料: ON/OFF)

            byte[] command = new byte[22];

            // 副標頭
            command[0] = 0x50;
            command[1] = 0x00;

            // 網路號 + 站號
            command[2] = 0x00;
            command[3] = 0xFF;

            // 目標模組 + 單元
            command[4] = 0xFF;
            command[5] = 0x03;

            // I/O 編號
            command[6] = 0x00;

            // 監視定時器 (預設 12 * 250ms = 3秒)
            command[7] = 0x0C;
            command[8] = 0x00;

            // 命令長度 (12 bytes)
            command[9] = 0x0C;
            command[10] = 0x00;

            // 子指令 (1401: 位元單位批次寫入)
            command[11] = 0x01;
            command[12] = 0x14;

            // 解析位址 (M1041)
            int deviceNumber = int.Parse(address.Substring(1)); // 移除 'M' 取得數字
            command[13] = (byte)(deviceNumber & 0xFF);
            command[14] = (byte)((deviceNumber >> 8) & 0xFF);
            command[15] = (byte)((deviceNumber >> 16) & 0xFF);

            // 設備代碼 (M = 0x90)
            command[16] = 0x90;

            // 點數 (1 點)
            command[17] = 0x01;
            command[18] = 0x00;

            // 資料 (ON=0x10, OFF=0x00)
            command[19] = (byte)(value ? 0x10 : 0x00);
            command[20] = 0x00;
            command[21] = 0x00;

            return command;
        }

        /// <summary>
        /// 檢查連接狀態
        /// </summary>
        public bool CheckConnection()
        {
            try
            {
                if (_client == null || !_client.Connected)
                {
                    lock (_lockObj)
                    {
                        _isConnected = false;
                    }
                    return false;
                }

                // 嘗試發送測試數據
                bool canWrite = _client.Client.Poll(0, SelectMode.SelectWrite);
                bool hasError = _client.Client.Poll(0, SelectMode.SelectError);

                bool connected = canWrite && !hasError;

                lock (_lockObj)
                {
                    _isConnected = connected;
                }

                return connected;
            }
            catch
            {
                lock (_lockObj)
                {
                    _isConnected = false;
                }
                return false;
            }
        }

        public void Dispose()
        {
            Disconnect();
            _semaphore?.Dispose();
        }
    }
}