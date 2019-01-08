ALTER TABLE [dbo].[SugestaoPedido] 
ADD [tpCaixaFornecedor] CHAR(1) NULL
	,[vlPesoLiquido] DECIMAL(11, 4) NULL
	,[idCD] [int] NULL
	,[vlTipoReabastecimento] [smallint] NULL
	,[qtdSugestaoRoteiroRA] DECIMAL(11, 3) NULL

ALTER TABLE [dbo].[SugestaoPedido]
	WITH CHECK ADD CONSTRAINT [SugestaoPedido_idCD_CD] FOREIGN KEY ([idCD]) REFERENCES [dbo].[CD]([idCD])
GO

ALTER TABLE [dbo].[SugestaoPedido] CHECK CONSTRAINT [SugestaoPedido_idCD_CD]
GO

