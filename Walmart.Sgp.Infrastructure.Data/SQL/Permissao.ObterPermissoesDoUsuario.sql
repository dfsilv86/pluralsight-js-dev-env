/*
DECLARE @idUsuario int;
SET @idUsuario = 1052;
*/

SELECT 
B.IDBandeira,
B.dsBandeira,
NULL AS SplitOn1,
L.IDLoja,
L.cdLoja,
L.nmLoja
FROM 
	Permissao P	WITH (NOLOCK)
		LEFT JOIN PermissaoBandeira PB WITH (NOLOCK)
			ON PB.IDPermissao = P.IDPermissao
				LEFT JOIN Bandeira B WITH (NOLOCK)
					ON B.IDBandeira = PB.IDBandeira
		LEFT JOIN PermissaoLoja PL WITH (NOLOCK)
			ON PL.IDPermissao = P.IDPermissao
				LEFT JOIN Loja L WITH (NOLOCK)
					ON L.IDLoja = PL.IDLoja			
WHERE IDUsuario = @idUsuario
