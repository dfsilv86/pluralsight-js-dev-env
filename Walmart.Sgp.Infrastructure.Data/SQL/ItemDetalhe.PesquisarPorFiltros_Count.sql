/*
DECLARE @cdItem BIGINT, @dsItem NVARCHAR(50), @cdSistema SMALLINT, @cdDepartamento INT, @cdCategoria INT, @cdSubcategoria INT, @cdFineline INT, @tpStatus CHAR(1), @cdPLU BIGINT;
DECLARE @IDUsuario INT, @tpVinculado CHAR(1);;
SET @IDUsuario = 2;
SET @cdSistema = 1;
SET @cdDepartamento = 2;
SET @cdCategoria = NULL;
SET @cdSubcategoria = NULL;
SET @cdFineline = NULL;
SET @tpStatus = 'A';
--*/

WITH Permissoes AS (
	-- Busca as permissoes do usuario
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Lojas AS (
	-- Busca conforme permissao por bandeira
	SELECT L.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDBandeira = PB.IDBandeira
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	UNION
	-- Busca conforme permissao por loja
	SELECT PL.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
), Traits AS (
	SELECT T.IDItemDetalhe
	  FROM Trait T WITH (NOLOCK)
	 WHERE T.IdLoja IN (SELECT IdLoja FROM Lojas WITH(NOLOCK))
), ItemDetalhes AS (
	SELECT ID.cdSistema, ID.cdItem, ID.dsItem
	  FROM ItemDetalhe ID WITH (NOLOCK)	   
		   INNER JOIN FineLine FL WITH (NOLOCK)
				   ON FL.IDFineLine = ID.IDFineLine
		   INNER JOIN Subcategoria SC WITH (NOLOCK)
				   ON SC.IDSubcategoria = ID.IDSubcategoria
		   INNER JOIN Categoria CA WITH (NOLOCK)
				   ON CA.IDCategoria = ID.IDCategoria
		   INNER JOIN Departamento DE WITH (NOLOCK)
				   ON DE.IDDepartamento = ID.IDDepartamento
		   INNER JOIN Divisao DI WITH (NOLOCK)
				   ON DI.IDDivisao = DE.IDDivisao        
	 WHERE EXISTS (SELECT TOP 1 1 FROM Traits T WHERE T.IdItemDetalhe = ID.IDItemDetalhe)
	   AND (@cdSistema IS NULL OR ID.cdSistema = @cdSistema)
	   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
	   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
	   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
	   AND (@cdFineline IS NULL OR FL.cdFineLine = @cdFineline)
	   AND (@tpVinculado IS NULL OR ID.tpVinculado = @tpVinculado)
	   AND (@tpStatus IS NULL OR ID.tpStatus = @tpStatus)
	   AND (@cdPLU IS NULL OR ID.cdPLU = @cdPLU)
)
SELECT COUNT(1) FROM ItemDetalhes TT WHERE (@cdItem IS NULL OR TT.cdItem = @cdItem)
   AND (@dsItem IS NULL OR TT.dsItem LIKE '%' + @dsItem + '%')
   --AND (@cdSistema IS NULL OR TT.cdSistema = @cdSistema);
