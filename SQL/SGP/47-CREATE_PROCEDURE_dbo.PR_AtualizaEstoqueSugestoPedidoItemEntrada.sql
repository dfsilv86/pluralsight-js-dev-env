

CREATE PROCEDURE [dbo].[PR_AtualizaEstoqueSugestoPedidoItemEntrada]
AS
BEGIN

	UPDATE WLMSLP_STAGE.dbo.SugestaoPedidoItemEntrada
	SET Estoque = dbo.fnObterPosicaoEstoque(ID_ITEM_SAIDA, IdLoja, GETDATE()),
		blPossuiVendasUltimaSemana = dbo.fnPossuiVendasPeriodoN(IdLoja, ID_ITEM_SAIDA, null, null)

	UPDATE WLMSLP_STAGE.dbo.SugestaoPedidoItemEntrada
	SET Estoque = 0
	WHERE Estoque < 0

END
GO


