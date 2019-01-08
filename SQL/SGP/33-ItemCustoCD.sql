SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemCustoCD](
	[idItemCustoCD] [int] IDENTITY(1,1) NOT NULL,
	[idCD] [int] NOT NULL,
	[idItemDetalhe] [bigint] NOT NULL,
	[BuyPrice] [decimal](18, 6) NOT NULL,
	[dtRecebimento] [datetime] NULL,
	[dhcriacao ] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[idItemCustoCD] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[ItemCustoCD]  WITH CHECK ADD  CONSTRAINT [ItemCustoCD_idItemDetalhe] FOREIGN KEY([idItemDetalhe])
REFERENCES [dbo].[ItemDetalhe] ([IDItemDetalhe])
GO

ALTER TABLE [dbo].[ItemCustoCD] CHECK CONSTRAINT [ItemCustoCD_idItemDetalhe]
GO

CREATE NONCLUSTERED INDEX IX_ItemCustoCD_idItemDetalhe
ON [dbo].[ItemCustoCD] ([idItemDetalhe])
GO

ALTER TABLE [dbo].[ItemCustoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemCustoCD_idCD] FOREIGN KEY([idCD])
REFERENCES [dbo].[CD] ([idCD])
GO

ALTER TABLE [dbo].[ItemCustoCD] CHECK CONSTRAINT [FK_ItemCustoCD_idCD]
GO

CREATE NONCLUSTERED INDEX [IX_ItemCustoCD_idCD] ON [dbo].[SugestaoPedidoCD] 
(
	[idCD] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

Create index IX_ItemCustoCD_idCD on [dbo].[ItemCustoCD] (idCD)
go
