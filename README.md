# CoindeskApi

建資料庫語法

```sql
CREATE DATABASE Coindesk
GO

CREATE TABLE [Coindesk].[dbo].[Currencies]
(
    [Id]           INT          NOT NULL IDENTITY,
    [Code]         VARCHAR(10)  NOT NULL,
    [Lang]         VARCHAR(10)  NOT NULL,
    [CurrencyName] NVARCHAR(10) NOT NULL,
    [CreateTime]   DATETIME     NOT NULL,
    [ModifiedTime] DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO Coindesk.dbo.Currencies
    (Code, Lang, CurrencyName, CreateTime)
Values ('USD', 'zh-TW', N'美金', GETDATE()),
       ('GBP', 'zh-TW', N'英鎊', GETDATE()),
       ('EUR', 'zh-TW', N'歐元', GETDATE())
```

在 Docker 執行專案
```bash
docker-compose up -d
```

---

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
- [x] swagger-ui
- [ ] 多語系設計
- [ ] design pattern 實作
- [ ] 能夠運行在 Docker
- [ ] 加解密技術應用 (AES/RSA…etc.)

