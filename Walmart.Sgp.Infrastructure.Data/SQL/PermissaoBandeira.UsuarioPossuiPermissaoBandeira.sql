/*DECLARE @idUsuario INT = 2795
DECLARE @idBandeira INT = 40*/

SELECT
	COUNT(*)
FROM PermissaoBandeira PB WITH(NOLOCK)
JOIN Permissao P WITH(NOLOCK) ON P.IDPermissao = PB.IDPermissao
WHERE P.IDUsuario = @idUsuario
  AND PB.IDBandeira = @idBandeira