/*
DECLARE @IDUsuario INT, @cdSistema INT, @IDFormato INT, @IDRegiaoAdministrativa INT;
SET @IDUsuario = 1;
SET @cdSistema = 1;
SET @IDFormato = 1;
SET @IDRegiaoAdministrativa = null;
*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Bandeiras AS (

	-- Busca a bandeira conforme permissao da bandeira
	SELECT PB.IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK) 
				   ON L.IDBandeira = PB.IDBandeira 
						AND (@IDRegiaoAdministrativa IS NULL OR L.IdRegiaoAdministrativa = @IDRegiaoAdministrativa)
	UNION
	-- Busca a bandeira conforme permissao da loja
	SELECT L.IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoLoja PL WITH (NOLOCK)
				   ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
				   ON L.IDLoja = PL.IDLoja 
						AND (@IDRegiaoAdministrativa IS NULL OR L.IdRegiaoAdministrativa = @IDRegiaoAdministrativa)
)
SELECT B.IDBandeira, B.dsBandeira, B.sgBandeira, B.tpCusto, B.cdSistema, B.blAtivo, B.dhCriacao, B.dhAtualizacao, B.cdUsuarioCriacao, B.cdUsuarioAtualizacao, B.blImportarTodos, B.IDFormato
  FROM Bandeiras BS WITH (NOLOCK)
       INNER JOIN Bandeira B WITH (NOLOCK)
	           ON B.IDBandeira = BS.IDBandeira
 WHERE @cdSistema IS NULL OR B.cdSistema = @cdSistema
   AND (@IDFormato IS NULL OR B.IDFormato = @IDFormato)
   AND B.blAtivo = 'S'
   ORDER BY B.dsBandeira DESC