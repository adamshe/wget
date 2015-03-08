CREATE SCHEMA [EarningForecast] AUTHORIZATION [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EarningForecast](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Ticker] [nvarchar](8) NULL,
	[FiscalEnd] [nvarchar](8) NULL,
	[Consensus] [nvarchar](max) NULL,
	[High] [decimal](18, 2) NULL,
	[Low] [decimal](18, 2) NULL,
	[Estimates] [decimal](18, 2) NULL,
	[ReviseUps] [int] NULL,
	[ReviseDowns] [int] NULL,
	[CreationDate] [datetime] NULL,
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
