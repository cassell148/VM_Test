# Vision Master 快速開始指南

## 🚀 30 秒快速上手

### 步驟 1：開啟專案
```
1. 用 Visual Studio 開啟 Vision_Master.csproj
2. 還原 NuGet 套件（如需要）
```

### 步驟 2：加入 VisionMaster SDK 引用
```
專案 → 加入參考 → 瀏覽 → 選擇以下 DLL：
✓ VM.Core.dll
✓ VM.PlatformSDKCS.dll  
✓ VMControls.Winform.Release.dll
✓ IMVSEdgeWidthFindModuCs.dll
```

### 步驟 3：準備配置檔
```ini
修改 Config.ini：

[PLC]
IP=192.168.1.100        # 改成你的 PLC IP
Port=502

[Vision]
PixelToMM=0.0229        # 改成你的相機校正值
```

### 步驟 4：準備視覺方案
```
1. 建立 solution.solw 視覺方案
2. 放到執行檔目錄
3. 或啟動後從選單手動載入
```

### 步驟 5：執行程式
```
F5 啟動 → 登入（admin/123456）→ 執行檢測
```

---

## 🎯 第一次使用

### 登入帳密
- **帳號**: `admin`
- **密碼**: `123456`（Base64: MTIzNDU2）

### 修改密碼
1. 開啟 Config.ini
2. 找到 `[Auth]` 區段
3. 修改 `PasswordHash` 為新密碼的 Base64
4. 線上 Base64 編碼器：https://www.base64encode.org/

### 像素轉換校正
```
PixelToMM = 實際尺寸(mm) / 圖像像素數

範例：
已知物體 10mm，圖像測得 436 像素
PixelToMM = 10 / 436 = 0.0229
```

---

## 📊 視覺方案要求

### Procedure 設定
```
至少包含一個 Procedure（流程）
```

### 必要工具
```
工具名稱：間距檢測1
工具類型：IMVSEdgeWidthFindModuTool
```

### 輸出格式
```
變數名稱：Height
格式：「狀態;像素值」
範例：「OK;150.5」或「NG;145.2」
```

---

## 🐛 常見問題

### Q1: 程式無法啟動
```
檢查：Config.ini 是否存在
解決：複製 Config.ini 範例檔到執行檔目錄
```

### Q2: 找不到 VisionMaster DLL
```
檢查：是否正確加入專案引用
解決：重新加入所有必要的 DLL 參考
```

### Q3: 載入視覺方案失敗
```
檢查：solution.solw 是否存在
解決：手動從選單載入方案檔
```

### Q4: 檢測結果不正確
```
檢查：PixelToMM 是否正確校正
解決：重新計算並修改 Config.ini
```

### Q5: 登入失敗
```
檢查：Config.ini 中的 PasswordHash
解決：確認密碼 Base64 編碼正確
```

---

## 📝 開發建議

### 加入新的檢測工具
```csharp
// 在 Procedure_Begin 中
var myTool = VmSolution.Instance[$"{_procedure.Name}.工具名稱"] as 工具類型;
if (myTool != null)
{
    // 設定參數
}
```

### 取得檢測結果
```csharp
// 在 Procedure_End 中
var result = _procedure.ModuResult.GetOutputString("變數名稱");
string value = result.astStringVal[0].strValue;
```

### 加入新的配置項目
```csharp
// 在 ConfigManager.cs 中
public string MyConfig { get; private set; }

// 在 Load() 方法中
MyConfig = _iniFile.ReadString("Section", "Key", "預設值");

// 在 Save() 方法中
_iniFile.WriteString("Section", "Key", MyConfig);
```

---

## 🔗 相關連結

- VisionMaster 官方文件
- .NET Framework 4.8 SDK
- Base64 線上編碼：https://www.base64encode.org/

---

## 📞 支援

遇到問題？
1. 查看 README.md 詳細文件
2. 檢查 Logs 資料夾中的日誌
3. 使用日誌檢視器查看詳細錯誤

---

**祝你使用愉快！** 🎉
