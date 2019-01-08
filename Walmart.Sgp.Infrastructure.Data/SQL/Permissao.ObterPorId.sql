/*
DECLARE @idPermissao INT
SET @idPermissao = 1938
--*/

SELECT 
	P.IDPermissao
	, P.blRecebeNotificaoOperacoes
	, P.blRecebeNotificaoFinanceiro
	, NULL AS SplitOn1
	, U.Id
	, U.Username
	, U.FullName
	, NULL AS SplitOn2
	, PB.IDPermissaoBandeira
	, PB.IDPermissao
	, NULL AS SplitOn3
	, B.IDBandeira
	, B.dsBandeira
	, NULL AS SplitOn4
	, PL.IDPermissaoLoja
	, PL.IDPermissao
	, NULL AS SplitOn5
	, L.IDLoja
	, L.cdLoja
	, L.nmLoja	
	, B2.dsBandeira as dsBandeiraLoja
FROM Permissao P WITH(NOLOCK)
JOIN CWIUser U WITH(NOLOCK) ON U.Id = P.IDUsuario
LEFT JOIN PermissaoBandeira PB WITH(NOLOCK) ON PB.IDPermissao = P.IDPermissao
LEFT JOIN Bandeira B WITH(NOLOCK) ON B.IDBandeira = PB.IDBandeira
LEFT JOIN PermissaoLoja PL WITH(NOLOCK) ON PL.IDPermissao = P.IDPermissao
LEFT JOIN Loja L WITH(NOLOCK) ON L.IDLoja = PL.IDLoja
LEFT JOIN Bandeira B2 WITH(NOLOCK) ON B2.IDBandeira = L.IDBandeira
WHERE P.IDPermissao = @idPermissao