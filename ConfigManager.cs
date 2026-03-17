using System;
using System.IO;
using Newtonsoft.Json;

namespace Vision_Master.Core
{
    /// <summary>
    /// 配置管理器 - 相容原始代碼版本
    /// </summary>
    public class ConfigManager
    {
        private readonly string _configPath;
        private ConfigData _config;

        #region 屬性 - 相容原始代碼

        /// <summary>
        /// PLC IP 位址
        /// </summary>
        public string PlcIp => _config?.PLCSettings?.IP ?? "192.168.1.100";

        /// <summary>
        /// PLC 通訊埠
        /// </summary>
        public int PlcPort => _config?.PLCSettings?.Port ?? 5000;

        /// <summary>
        /// VisionMaster 方案路徑
        /// </summary>
        public string SolutionPath => _config?.SolutionPath ?? "";

        /// <summary>
        /// NG 圖像保存路徑
        /// </summary>
        public string SaveNGImagePath => _config?.SaveNGImagePath ?? "";

        /// <summary>
        /// 日期格式
        /// </summary>
        public string DateFormat => _config?.DateFormat ?? "yyyy-MM-dd";

        /// <summary>
        /// 保質期天數
        /// </summary>
        public int ExpiryDays => _config?.ExpiryDays ?? 180;

        /// <summary>
        /// PLC 是否啟用
        /// </summary>
        public bool PlcEnabled => _config?.PLCSettings?.Enabled ?? true;

        #endregion

        #region 建構函式與靜態方法

        /// <summary>
        /// 建構函式 - 相容原始代碼
        /// </summary>
        /// <param name="configPath">配置檔案路徑（支援 .ini 或 .json）</param>
        public ConfigManager(string configPath)
        {
            _configPath = configPath;
        }

        /// <summary>
        /// 靜態方法載入配置 - 相容新代碼
        /// 從預設位置載入 Config.json 或 Config.ini
        /// </summary>
        public static ConfigData LoadConfig()
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                // 優先嘗試 JSON 格式
                string jsonPath = Path.Combine(appPath, "Config.json");
                if (File.Exists(jsonPath))
                {
                    var manager = new ConfigManager(jsonPath);
                    if (manager.Load())
                    {
                        return manager._config;
                    }
                }

                // 其次嘗試 INI 格式
                string iniPath = Path.Combine(appPath, "Config.ini");
                if (File.Exists(iniPath))
                {
                    var manager = new ConfigManager(iniPath);
                    if (manager.Load())
                    {
                        return manager._config;
                    }
                }

                // 都找不到，返回預設配置
                Logger.Write("[Config] 找不到配置檔案，使用預設配置", Logger.LogLevel.Warning);
                return CreateDefaultConfig();
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 靜態載入配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return CreateDefaultConfig();
            }
        }

        /// <summary>
        /// 靜態方法獲取當前配置 - 相容新代碼
        /// </summary>
        public static ConfigData GetConfig()
        {
            return LoadConfig();
        }

        /// <summary>
        /// 創建預設配置
        /// </summary>
        private static ConfigData CreateDefaultConfig()
        {
            return new ConfigData
            {
                SolutionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "solution.solw"),
                SaveNGImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NGImages"),
                DateFormat = "yyyy-MM-dd",
                ExpiryDays = 180,
                PLCSettings = new PLCSettings
                {
                    IP = "192.168.1.100",
                    Port = 5000,
                    Enabled = true
                },
                LoginSettings = new LoginSettings
                {
                    Username = "admin",
                    Password = "admin"
                }
            };
        }

        #endregion

        #region 公開方法

        /// <summary>
        /// 載入配置 - 相容原始代碼
        /// </summary>
        /// <returns>載入是否成功</returns>
        public bool Load()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    Logger.Write($"[Config] 配置檔案不存在: {_configPath}", Logger.LogLevel.Error);
                    return false;
                }

                // 根據副檔名決定讀取方式
                string extension = Path.GetExtension(_configPath).ToLower();

                if (extension == ".json")
                {
                    return LoadFromJson();
                }
                else if (extension == ".ini")
                {
                    return LoadFromIni();
                }
                else
                {
                    Logger.Write($"[Config] 不支援的配置檔案格式: {extension}", Logger.LogLevel.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 載入配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 驗證登入 - 相容原始代碼
        /// </summary>
        /// <param name="username">使用者名稱</param>
        /// <param name="password">密碼</param>
        /// <returns>驗證是否成功</returns>
        public bool ValidateLogin(string username, string password)
        {
            try
            {
                // 從配置中讀取帳號密碼
                if (_config?.LoginSettings == null)
                {
                    Logger.Write("[Config] 登入設定不存在，使用預設帳號", Logger.LogLevel.Warning);
                    // 預設帳號：admin / admin
                    return username == "admin" && password == "admin";
                }

                bool isValid = username == _config.LoginSettings.Username &&
                               password == _config.LoginSettings.Password;

                if (isValid)
                {
                    Logger.Write($"[Config] 使用者 '{username}' 登入成功");
                }
                else
                {
                    Logger.Write($"[Config] 使用者 '{username}' 登入失敗", Logger.LogLevel.Warning);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 驗證登入時發生錯誤: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 儲存配置
        /// </summary>
        public bool Save()
        {
            try
            {
                string extension = Path.GetExtension(_configPath).ToLower();

                if (extension == ".json")
                {
                    string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                    File.WriteAllText(_configPath, json);
                    Logger.Write("[Config] 配置已儲存");
                    return true;
                }
                else
                {
                    Logger.Write("[Config] INI 格式暫不支援儲存", Logger.LogLevel.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 儲存配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 從 JSON 檔案載入配置
        /// </summary>
        private bool LoadFromJson()
        {
            try
            {
                string json = File.ReadAllText(_configPath);
                _config = JsonConvert.DeserializeObject<ConfigData>(json);

                // 確保必要的子物件存在
                if (_config.PLCSettings == null)
                    _config.PLCSettings = new PLCSettings();
                if (_config.LoginSettings == null)
                    _config.LoginSettings = new LoginSettings();

                Logger.Write("[Config] JSON 配置載入成功");
                LogConfig();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 讀取 JSON 配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 從 INI 檔案載入配置
        /// </summary>
        private bool LoadFromIni()
        {
            try
            {
                _config = new ConfigData();

                // 讀取 INI 檔案
                var lines = File.ReadAllLines(_configPath);
                string currentSection = "";

                foreach (var line in lines)
                {
                    string trimmedLine = line.Trim();

                    // 跳過空行和註解
                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                        continue;

                    // 解析區段
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).ToLower();
                        continue;
                    }

                    // 解析鍵值對
                    int equalIndex = trimmedLine.IndexOf('=');
                    if (equalIndex <= 0)
                        continue;

                    string key = trimmedLine.Substring(0, equalIndex).Trim().ToLower();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();

                    // 根據區段和鍵設定值
                    ParseIniValue(currentSection, key, value);
                }

                // 確保必要的子物件存在
                if (_config.PLCSettings == null)
                    _config.PLCSettings = new PLCSettings();
                if (_config.LoginSettings == null)
                    _config.LoginSettings = new LoginSettings();

                Logger.Write("[Config] INI 配置載入成功");
                LogConfig();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 讀取 INI 配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 解析 INI 鍵值對
        /// </summary>
        private void ParseIniValue(string section, string key, string value)
        {
            try
            {
                switch (section)
                {
                    case "plc":
                        if (_config.PLCSettings == null)
                            _config.PLCSettings = new PLCSettings();

                        switch (key)
                        {
                            case "ip":
                                _config.PLCSettings.IP = value;
                                break;
                            case "port":
                                if (int.TryParse(value, out int port))
                                    _config.PLCSettings.Port = port;
                                break;
                            case "enabled":
                                _config.PLCSettings.Enabled = value.ToLower() == "true" || value == "1";
                                break;
                        }
                        break;

                    case "vision":
                        switch (key)
                        {
                            case "solutionpath":
                                _config.SolutionPath = value;
                                break;
                            case "savengpath":
                            case "savenimagepath":
                                _config.SaveNGImagePath = value;
                                break;
                            case "dateformat":
                                _config.DateFormat = value;
                                break;
                            case "expirydays":
                                if (int.TryParse(value, out int days))
                                    _config.ExpiryDays = days;
                                break;
                        }
                        break;

                    case "login":
                        if (_config.LoginSettings == null)
                            _config.LoginSettings = new LoginSettings();

                        switch (key)
                        {
                            case "username":
                                _config.LoginSettings.Username = value;
                                break;
                            case "password":
                                _config.LoginSettings.Password = value;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"[Config] 解析 INI 值時發生錯誤: {section}.{key} = {value}, {ex.Message}", Logger.LogLevel.Warning);
            }
        }

        /// <summary>
        /// 記錄配置內容
        /// </summary>
        private void LogConfig()
        {
            Logger.Write("========== 配置資訊 ==========");
            Logger.Write($"方案路徑: {SolutionPath}");
            Logger.Write($"NG 圖像路徑: {SaveNGImagePath}");
            Logger.Write($"日期格式: {DateFormat}");
            Logger.Write($"保質期天數: {ExpiryDays}");
            Logger.Write($"PLC IP: {PlcIp}");
            Logger.Write($"PLC Port: {PlcPort}");
            Logger.Write($"PLC 啟用: {PlcEnabled}");
            Logger.Write($"登入帳號: {_config?.LoginSettings?.Username ?? "admin"}");
            Logger.Write("==============================");
        }

        #endregion

        #region 內部類別

        /// <summary>
        /// 配置數據類別
        /// </summary>
        public class ConfigData
        {
            public string SolutionPath { get; set; } = "";
            public string SaveNGImagePath { get; set; } = "";
            public string DateFormat { get; set; } = "yyyy-MM-dd";
            public int ExpiryDays { get; set; } = 180;
            public int TargetOKCount { get; set; } = 0;
            public int TargetNGCount { get; set; } = 0;
            public PLCSettings PLCSettings { get; set; } = new PLCSettings();
            public LoginSettings LoginSettings { get; set; } = new LoginSettings();
        }

        /// <summary>
        /// PLC 設定類別
        /// </summary>
        public class PLCSettings
        {
            public string IP { get; set; } = "192.168.1.100";
            public int Port { get; set; } = 5000;
            public bool Enabled { get; set; } = true;
        }

        /// <summary>
        /// 登入設定類別
        /// </summary>
        public class LoginSettings
        {
            public string Username { get; set; } = "admin";
            public string Password { get; set; } = "admin";
        }

        #endregion
    }
}