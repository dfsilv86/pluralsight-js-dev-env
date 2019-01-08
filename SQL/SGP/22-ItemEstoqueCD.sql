/****** Object:  Table [dbo].[ItemEstoqueCD]    Script Date: 04/19/2016 16:10:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemEstoqueCD](
	[idItemEstoqueCD] [int] IDENTITY(1,1) NOT NULL,
	[idItemDetalhe] [bigint] NOT NULL,
	[idCd ] [int] NOT NULL,
	[QtdOnHand] [int] NULL,
	[QtdOnOrder] [int] NULL,
 CONSTRAINT [PK_ItemEstoqueCD] PRIMARY KEY CLUSTERED 
(
	[idItemEstoqueCD] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ItemEstoqueCD]  WITH CHECK ADD  CONSTRAINT [ItemEstoqueCD_idCd] FOREIGN KEY([idCd ])
REFERENCES [dbo].[CD] ([IDCD])
GO

ALTER TABLE [dbo].[ItemEstoqueCD] CHECK CONSTRAINT [ItemEstoqueCD_idCd]
GO

ALTER TABLE [dbo].[ItemEstoqueCD]  WITH CHECK ADD  CONSTRAINT [ItemEstoqueCD_idItemDetalhe] FOREIGN KEY([idItemDetalhe])
REFERENCES [dbo].[ItemDetalhe] ([IDItemDetalhe])
GO

ALTER TABLE [dbo].[ItemEstoqueCD] CHECK CONSTRAINT [ItemEstoqueCD_idItemDetalhe]
GO

CREATE NONCLUSTERED INDEX IX_ItemEstoqueCD_idItemDetalhe
ON [dbo].[ItemEstoqueCD] ([idItemDetalhe])
GO

CREATE NONCLUSTERED INDEX IX_ItemEstoqueCD_idCD
ON [dbo].[ItemEstoqueCD] ([idCD])
GO
