# CoindeskApi
面試作業


```sql
CREATE TABLE Currency
(
    Id           INT identity,
    Code         VARCHAR(3)  NOT NULL,
    Lang         VARCHAR(10) NOT NULL,
    CurrencyName NVARCHAR(10)
)
go
```

---

任務描述  
請利用 ASP .NET Core 8.0 建置一個 Web API 專案，實作以下內容，將結果上傳至 GitHub，並提供repo鏈結。  

資料庫  
SQL Server Express LocalDB（Entity Framework Core）  

功能簡述  
- 呼叫 coindesk API，解析其下行內容與資料轉換，並實作新的 API。coindesk API：https://api.coindesk.com/v1/bpi/currentprice.json
- 建立一張幣別與其對應中文名稱的資料表（需附建立SQL語法），並提供查詢/新增/修改/刪除功能 API。
- 查詢幣別請依照幣別代碼排序。

實作內容  
- [x] 幣別 DB 維護功能。
- [x] 呼叫 coindesk 的 API。
- [x] 呼叫 coindesk 的 API，並進行資料轉換，組成新 API。此新 API 提供：
  - 更新時間（時間格式範例：1990/01/01 00:00:00）。
  - 幣別相關資訊（幣別，幣別中文名稱，以及匯率）。
- [ ] 所有功能均須包含單元測試。
- [ ] 將專案上傳至 GitHub 並設為公開分享，回傳repo鏈結。
- [ ] 嘗試錄製demo，上傳至Youtube影片(不要用Shorts)，設為非公開分享回傳

實作加分題 (請於 README 說明包含以下哪些項目)  
- [x] 印出所有 API 被呼叫以及呼叫外部 API 的 request and response  body log
- [x] Error handling 處理 API response
- [ ] swagger-ui
- [ ] 多語系設計
- [ ] design pattern 實作
- [ ] 能夠運行在 Docker
- [ ] 加解密技術應用 (AES/RSA…etc.)

