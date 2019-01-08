/*DECLARE @idUsuario INT = NULL
DECLARE @idBandeira INT = NULL
DECLARE @idLoja INT = NULL*/

SELECT
	COUNT(*)
FROM Permissao P WITH(NOLOCK)
JOIN CWIUser U WITH(NOLOCK) ON U.Id = P.IDUsuario
WHERE (@idUsuario IS NULL OR U.Id = @idUsuario)
	AND (@idBandeira IS NULL OR EXISTS(
		SELECT 1 
		FROM PermissaoBandeira PB WITH(NOLOCK) 
		WHERE PB.IDPermissao = P.IDPermissao AND PB.IDBandeira = @idBandeira))
	AND (@idLoja IS NULL OR EXISTS(
		SELECT 1 
		FROM PermissaoLoja PL WITH(NOLOCK)
		WHERE PL.IDPermissao = P.IDPermissao AND PL.IDLoja = @idLoja))
