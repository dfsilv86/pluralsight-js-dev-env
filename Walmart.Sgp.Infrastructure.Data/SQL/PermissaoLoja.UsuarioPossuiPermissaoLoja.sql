/*
DECLARE @idUsuario INT = 2795
DECLARE @idLoja INT = 87
--*/

SELECT
	COUNT(*)
FROM PermissaoLoja PL WITH(NOLOCK)
	INNER JOIN Permissao P WITH(NOLOCK) 
		ON P.IDPermissao = PL.IDPermissao
WHERE 
	  P.IDUsuario = @idUsuario
  AND PL.IDLoja = @idLoja