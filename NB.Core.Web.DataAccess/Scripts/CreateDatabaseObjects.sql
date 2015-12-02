USE Earning;
GO

IF OBJECT_ID(N'dbo.EarningForecast', 'U') IS NULL 
CREATE TABLE dbo.EarningForecast(
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Ticker] [nvarchar](8) NULL,
    [Frequency] [nvarchar](20) NULL,
    [FiscalEnd] [nvarchar](8) NULL,
    [Consensus] [nvarchar](max) NULL,
    [High] [decimal](18, 2) NULL,
    [Low] [decimal](18, 2) NULL,
    [Estimates] [int] NULL,
    [ReviseUps] [int] NULL,
    [ReviseDowns] [int] NULL,
    [CreationDate] [datetime] default GETDATE(),
PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO

CREATE NONCLUSTERED INDEX [ix_searchByTicker_FiscalEnd] ON [dbo].[EarningForecast] 
(
    [Ticker] ASC,
    [FiscalEnd] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

