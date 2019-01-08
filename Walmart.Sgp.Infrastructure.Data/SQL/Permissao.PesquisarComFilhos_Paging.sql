/*DECLARE @idUsuario INT = NULL
DECLARE @idBandeira INT = NULL
DECLARE @idLoja INT = NULL
*/

CREATE TABLE #Permissoes (
		RowNum INT,
		IDPermissao INT,
		UserId INT,
		Username varchar(50), 
		FullName varchar(128));

INSERT INTO #Permissoes
SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
					SELECT 
						 P.IDPermissao
						, U.Id as UserId
						, U.Username
						, U.FullName
					FROM Permissao P WITH(NOLOCK)
					JOIN CWIUser U WITH(NOLOCK) ON U.Id = P.IDUsuario
					WHERE (@idUsuario IS NULL OR U.Id = @idUsuario)
					  AND (@idBandeira IS NULL OR EXISTS(SELECT 1 
														 FROM PermissaoBandeira PB 
														 WHERE PB.IDPermissao = P.IDPermissao AND PB.IDBandeira = @idBandeira))
					  AND (@idLoja IS NULL OR EXISTS(SELECT 1
													 FROM PermissaoLoja PL
													 WHERE PL.IDPermissao = P.IDPermissao AND PL.IDLoja = @idLoja))
  ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum

SELECT 
	P.IDPermissao
	, NULL AS SplitOn1
	, P.UserId as Id
	, P.Username
	, P.FullName
	, NULL AS SplitOn2
	, B.IDBandeira
	, B.dsBandeira
	, NULL AS SplitOn3
	, L.IDLoja
	, L.cdLoja
	, L.nmLoja
	, NULL AS Spliton4
	, B2.dsBandeira as dsBandeiraLoja
FROM #Permissoes P WITH(NOLOCK)
LEFT JOIN PermissaoBandeira PB WITH(NOLOCK) ON PB.IDPermissao = P.IDPermissao
LEFT JOIN Bandeira B WITH(NOLOCK) ON B.IDBandeira = PB.IDBandeira
LEFT JOIN PermissaoLoja PL WITH(NOLOCK) ON PL.IDPermissao = P.IDPermissao
LEFT JOIN Loja L WITH(NOLOCK) ON L.IDLoja = PL.IDLoja 
LEFT JOIN Bandeira B2 WITH(NOLOCK) ON B2.IDBandeira = L.IDBandeira

DROP TABLE #Permissoes;