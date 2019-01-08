SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SugestaoPedidoCD](
	[idSugestaoPedidoCD] [bigint] IDENTITY(1,1) NOT NULL,
	[idFornecedorParametro] [bigint] NOT NULL,	
	[idItemDetalhePedido] [bigint] NOT NULL,
	[idItemDetalheSugestao] [bigint] NOT NULL,
	[idCD] [int] NOT NULL,
	[dtPedido] [date] NOT NULL,
	[dtEnvioPedido] [date] NOT NULL,
	[dtCancelamentoPedido] [date] NOT NULL,
	[dtCancelamentoPedidoOriginal] [date] NOT NULL,
	[dtInicioForecast] [date] NOT NULL,
	[dtFimForecast] [date] NOT NULL,
	[tpWeek] [smallint] NOT NULL,
	[tpInterval] [smallint] NOT NULL,
	[cdReviewDate] [int] NOT NULL,
	[vlLeadTime] [smallint] NOT NULL,
	[qtVendorPackage] [int] NOT NULL,
	[vlEstoqueSeguranca] [int] NULL,
	[tempoMinimoCD] [int] NULL,
	[tpCaixaFornecedor] [char](1) NULL,	
	[vlPesoLiquido] [decimal](11, 4) NULL,
	[vlTipoReabastecimento] [smallint] NULL,
	[vlCusto] [decimal](11, 4) NOT NULL,
	[qtdPackCompra] [int] NOT NULL,
	[qtdPackCompraOriginal] [int] NOT NULL,
	[qtdOnHand] [int] NOT NULL,
	[qtdOnOrder] [int] NOT NULL,
	[qtdForecast] [int] NOT NULL,
	[qtdPipeline] [int] NOT NULL,
	[IdOrigemDadosCalculo] [int] NOT NULL,
	[blFinalizado] [bit] NOT NULL default 0,
	[tpStatusEnvio] [char](1) NULL,
	[dhEnvioSugestao] [datetime] NULL,	
 CONSTRAINT [PK_SugestaoPedidoCD] PRIMARY KEY CLUSTERED 
(
	[idSugestaoPedidoCD] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SugestaoPedidoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_SugestaoPedidoCD_idFornecedorParametro] FOREIGN KEY([idFornecedorParametro])
REFERENCES [dbo].[FornecedorParametro] ([idFornecedorParametro])
GO

ALTER TABLE [dbo].[SugestaoPedidoCD] CHECK CONSTRAINT [FK_SugestaoPedidoCD_idFornecedorParametro]
GO

ALTER TABLE [dbo].[SugestaoPedidoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_SugestaoPedidoCD_idItemDetalhePedido] FOREIGN KEY([idItemDetalhePedido])
REFERENCES [dbo].[ItemDetalhe] ([IDItemDetalhe])
GO

ALTER TABLE [dbo].[SugestaoPedidoCD] CHECK CONSTRAINT [FK_SugestaoPedidoCD_idItemDetalhePedido]
GO

ALTER TABLE [dbo].[SugestaoPedidoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_SugestaoPedidoCD_idItemDetalheSugestao] FOREIGN KEY([idItemDetalheSugestao])
REFERENCES [dbo].[ItemDetalhe] ([IDItemDetalhe])
GO

ALTER TABLE [dbo].[SugestaoPedidoCD] CHECK CONSTRAINT [FK_SugestaoPedidoCD_idItemDetalheSugestao]
GO

ALTER TABLE [dbo].[SugestaoPedidoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_SugestaoPedidoCD_idCD] FOREIGN KEY([idCD])
REFERENCES [dbo].[CD] ([idCD])
GO

ALTER TABLE [dbo].[SugestaoPedidoCD] CHECK CONSTRAINT [FK_SugestaoPedidoCD_idCD]
GO

ALTER TABLE [dbo].[SugestaoPedidoCD]  WITH NOCHECK ADD  CONSTRAINT [FK_SugestaoPedidoCD_IdOrigemDadosCalculo] FOREIGN KEY([IdOrigemDadosCalculo])
REFERENCES [dbo].[OrigemDadosCalculo] ([IdOrigemDadosCalculo])
GO

ALTER TABLE [dbo].[SugestaoPedidoCD] CHECK CONSTRAINT [FK_SugestaoPedidoCD_IdOrigemDadosCalculo]
GO

CREATE NONCLUSTERED INDEX [IX_SugestaoPedidoCD_idItemDetalhePedido] ON [dbo].[SugestaoPedidoCD] 
(
	[idItemDetalhePedido] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_SugestaoPedidoCD_idItemDetalheSugestao] ON [dbo].[SugestaoPedidoCD] 
(
	[idItemDetalheSugestao] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_SugestaoPedidoCD_idCD] ON [dbo].[SugestaoPedidoCD] 
(
	[idCD] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_SugestaoPedidoCD_idFornecedorParametro] ON [dbo].[SugestaoPedidoCD] 
(
	[idFornecedorParametro] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_SugestaoPedidoCD_IdOrigemDadosCalculo] ON [dbo].[SugestaoPedidoCD] 
(
	[IdOrigemDadosCalculo] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
