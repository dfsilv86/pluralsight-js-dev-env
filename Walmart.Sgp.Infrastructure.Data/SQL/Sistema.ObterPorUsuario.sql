/*
DECLARE @IDUsuario INT, @cdSistema INT, @cultureCode VARCHAR(5);
SET @IDUsuario = 1;
SET @CultureCode= 'pt-BR';
--*/

-- TODO: mover para proc por performance?
-- TODO: cadastrar valores em en-US e deixar o globalization traduzir?

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Sistemas AS (
	-- Busca pela permissao da bandeira
	SELECT B.cdSistema
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Bandeira B WITH (NOLOCK)
		           ON B.IDBandeira = PB.IDBandeira
	UNION
	-- Busca pela permissao da loja
	SELECT L.cdSistema
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoLoja PL WITH (NOLOCK)
				   ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
				   ON L.IDLoja = PL.IDLoja
)
SELECT DV.dsValue AS cdSistema, DV.dsText AS [Text]
  FROM CWIDomainValue DV WITH (NOLOCK)
       INNER JOIN Sistemas S WITH (NOLOCK)
	           ON S.cdSistema = DV.dsValue
 WHERE DV.IDDomain = 3
   AND (@cultureCode IS NULL OR DV.dsCultureCode = @cultureCode)
 ORDER BY DV.SortOrder
