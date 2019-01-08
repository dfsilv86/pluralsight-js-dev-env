/*DECLARE @idRoteiro AS INT = 1
DECLARE @dtPedido AS VARCHAR(max) = '2016-04-11 00:00:00'*/

SELECT TOP 1 RP.idUsuarioAutorizacao, RP.dhAutorizacao, RP.blAutorizado, NULL AS SplitOn1, U.Username, U.FullName 
FROM RoteiroPedido AS RP WITH (NOLOCK)
INNER JOIN CWIUser U WITH (NOLOCK)
	ON U.Id = RP.idUsuarioAutorizacao
INNER JOIN SugestaoPedido SP WITH (NOLOCK)
	ON SP.IDSugestaoPedido = RP.idSugestaoPedido
WHERE RP.idRoteiro = @idRoteiro AND SP.dtPedido = @dtPedido AND RP.blAutorizado = 1