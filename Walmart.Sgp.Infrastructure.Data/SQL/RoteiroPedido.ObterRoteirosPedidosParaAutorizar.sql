/*DECLARE @dtPedido AS VARCHAR(max) = '2016-04-05 00:00:00'
DECLARE @idRoteiro as BIGINT = 1*/

SELECT RP.blAutorizado, RP.dhAutorizacao, RP.idRoteiro, RP.idRoteiroPedido, RP.idSugestaoPedido, RP.idUsuarioAutorizacao
FROM RoteiroPedido AS RP  WITH (NOLOCK)
INNER JOIN SugestaoPedido SP  WITH (NOLOCK)
ON SP.IDSugestaoPedido = RP.idSugestaoPedido
WHERE RP.idRoteiro = @idRoteiro AND SP.dtPedido = @dtPedido AND RP.blAutorizado = 0