/*
DECLARE @idRoteiro INT;
SET @idRoteiro = 1
--*/

SELECT COUNT(*)
FROM RoteiroPedido RP WITH(NOLOCK)
JOIN SugestaoPedido SP WITH(NOLOCK) ON SP.IDSugestaoPedido = RP.idSugestaoPedido
WHERE 
	RP.idRoteiro = @idRoteiro
	AND RP.blAutorizado = 0
	AND dtPedido = (SELECT CONVERT(date, GETDATE()));