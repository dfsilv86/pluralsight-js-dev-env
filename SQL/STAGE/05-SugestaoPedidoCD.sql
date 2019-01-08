SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SugestaoPedidoCD](
	[IdSugestaoPedidoCD] [int] IDENTITY(1,1) NOT NULL,
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
	[vlEstoqueSeguranca] [int] NOT NULL,
	[tempoMinimoCD] [int] NOT NULL,
	[tpCaixaFornecedor] [char](1) NULL,
	[vlPesoLiquido] [decimal](11, 4) NOT NULL,
	[vlTipoReabastecimento] [smallint] NULL,
	[vlCusto] [decimal](11, 4) NOT NULL,
	[qtdPackCompra] [int] NOT NULL,
	[qtdPackCompraOriginal] [int] NOT NULL,
	[qtdOnHand] [int] NOT NULL,
	[qtdOnOrder] [int] NOT NULL,
	[qtdForecast] [int] NOT NULL,
	[qtdPipeline] [int] NOT NULL,
	[IdOrigemDadosCalculo] [int] NOT NULL,
	[cdItemSaida] [bigint] NULL,
	[cdCd] [int] NULL,
	[qtdDiasAbastecimento] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSugestaoPedidoCD] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


