/****** Object:  Table [dbo].[ItemEstoqueCD]    Script Date: 04/19/2016 16:13:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemEstoqueCD](
	[cdItem] [bigint] NOT NULL,
	[cdCd ] [int] NOT NULL,
	[QtdOnHand] [int] NULL,
	[QtdOnOrder] [int] NULL
) ON [PRIMARY]

GO


