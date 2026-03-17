using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Vision_Master.Core
{
    /// <summary>
    /// 產品配置類別
    /// </summary>
    public class Product
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("daysOffset")]
        public int DaysOffset { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 計算標準有效日期（今天 + DaysOffset）
        /// </summary>
        public string GetStandardDate()
        {
            DateTime standardDate = DateTime.Today.AddDays(DaysOffset);
            return standardDate.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 取得格式化的標準日期（供顯示用）
        /// </summary>
        public string GetFormattedStandardDate()
        {
            DateTime standardDate = DateTime.Today.AddDays(DaysOffset);
            return standardDate.ToString("yyyy.MM.dd");
        }
    }

    /// <summary>
    /// 產品配置檔案結構
    /// </summary>
    public class ProductConfiguration
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        [JsonProperty("currentProduct")]
        public string CurrentProduct { get; set; }

        public ProductConfiguration()
        {
            Products = new List<Product>();
            CurrentProduct = "";
        }
    }

    /// <summary>
    /// 產品管理器
    /// </summary>
    public static class ProductManager
    {
        private static ProductConfiguration _config;
        private static string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");
        private static Product _currentProduct;

        /// <summary>
        /// 目前選擇的產品
        /// </summary>
        public static Product CurrentProduct => _currentProduct;

        /// <summary>
        /// 所有產品列表
        /// </summary>
        public static List<Product> AllProducts => _config?.Products;

        /// <summary>
        /// 載入產品配置
        /// </summary>
        public static bool LoadConfig()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    Logger.Write($"產品配置檔案不存在，建立預設配置: {_configPath}", Logger.LogLevel.Warning);
                    CreateDefaultConfig();
                }

                string json = File.ReadAllText(_configPath);
                _config = JsonConvert.DeserializeObject<ProductConfiguration>(json);

                if (_config == null || _config.Products == null || _config.Products.Count == 0)
                {
                    Logger.Write("產品配置檔案格式錯誤或為空", Logger.LogLevel.Error);
                    return false;
                }

                // 設定當前產品
                _currentProduct = _config.Products.FirstOrDefault(p => p.Name == _config.CurrentProduct);
                if (_currentProduct == null)
                {
                    _currentProduct = _config.Products[0];
                    _config.CurrentProduct = _currentProduct.Name;
                    SaveConfig();
                }

                Logger.Write($"產品配置載入成功，當前產品: {_currentProduct.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"載入產品配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 儲存產品配置
        /// </summary>
        public static bool SaveConfig()
        {
            try
            {
                if (_config == null)
                    return false;

                _config.CurrentProduct = _currentProduct?.Name ?? "";
                string json = JsonConvert.SerializeObject(_config, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_configPath, json);
                Logger.Write("產品配置已儲存");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"儲存產品配置失敗: {ex.Message}", Logger.LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// 切換到下一個產品
        /// </summary>
        public static Product SwitchToNextProduct()
        {
            if (_config == null || _config.Products == null || _config.Products.Count == 0)
                return null;

            int currentIndex = _config.Products.IndexOf(_currentProduct);
            int nextIndex = (currentIndex + 1) % _config.Products.Count;
            _currentProduct = _config.Products[nextIndex];

            SaveConfig();
            Logger.Write($"切換產品: {_currentProduct.Name}");
            return _currentProduct;
        }

        /// <summary>
        /// 切換到指定產品
        /// </summary>
        public static Product SwitchToProduct(string productName)
        {
            if (_config == null || _config.Products == null)
                return null;

            var product = _config.Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                _currentProduct = product;
                SaveConfig();
                Logger.Write($"切換產品: {_currentProduct.Name}");
            }
            return _currentProduct;
        }

        /// <summary>
        /// 建立預設配置檔案
        /// </summary>
        private static void CreateDefaultConfig()
        {
            _config = new ProductConfiguration
            {
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "巷口乾麵",
                        DaysOffset = 365,
                        Code = "M1216XX"
                    },
                    new Product
                    {
                        Name = "巷口麵線",
                        DaysOffset = 365,
                        Code = "M1217XX"
                    }
                },
                CurrentProduct = "巷口乾麵"
            };

            _currentProduct = _config.Products[0];
            SaveConfig();
        }
    }
}