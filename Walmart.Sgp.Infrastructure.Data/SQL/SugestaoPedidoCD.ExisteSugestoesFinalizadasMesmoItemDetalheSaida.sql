/*
DECLARE @idSugestaoPedidoCD BIGINT;
SET @idSugestaoPedidoCD = 881;
*/

SELECT
	COUNT(1)
FROM SugestaoPedidoCD SPC WITH(NOLOCK)
WHERE SPC.idSugestaoPedidoCD = @idSugestaoPedidoCD
AND EXISTS(
	SELECT 1
	FROM SugestaoPedidoCD SPC_TMP WITH(NOLOCK)
	WHERE SPC.dtPedido = SPC_TMP.dtPedido
		AND SPC.idItemDetalheSugestao = SPC_TMP.idItemDetalheSugestao		
		AND SPC.blFinalizado = 1
		AND SPC.idCD = SPC_TMP.idCD)