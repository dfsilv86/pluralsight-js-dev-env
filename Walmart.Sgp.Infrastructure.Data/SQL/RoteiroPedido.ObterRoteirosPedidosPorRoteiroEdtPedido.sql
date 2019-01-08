/*DECLARE @idRoteiro AS BIGINT = 1
DECLARE @dtPedido AS VARCHAR(max) = '2016-04-11 00:00:00'*/

SELECT RP.idRoteiro, NULL AS SplitOn1, ID.cdItem, ID.dsItem, ID.IDItemDetalhe
FROM RoteiroPedido AS RP WITH (NOLOCK)
INNER JOIN SugestaoPedido SP WITH (NOLOCK)
	ON SP.IDSugestaoPedido = RP.idSugestaoPedido
INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	ON ID.IDItemDetalhe = SP.IDItemDetalhePedido
WHERE RP.idRoteiro = @idRoteiro AND SP.dtPedido = @dtPedido
GROUP BY ID.IDItemDetalhe, ID.cdItem, ID.dsItem, RP.IdRoteiro