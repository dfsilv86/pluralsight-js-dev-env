SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ItemCustoCD](
	[cdItem] [int] NOT NULL,
	[UF] [char](2) NOT NULL,
	[BuyPrice] [decimal](18, 6) NOT NULL,
	[dtRecebimento] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


