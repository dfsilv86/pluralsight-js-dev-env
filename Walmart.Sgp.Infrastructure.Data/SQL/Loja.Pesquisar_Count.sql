/*
DECLARE @IDUsuario INT, @TipoPermissao INT, @cdSistema INT, @idBandeira INT, @cdLoja INT, @nmLoja NVARCHAR(50);
SET @IDUsuario = 20;
SET @TipoPermissao = 2;
SET @cdSistema = NULL;
SET @idBandeira = NULL;
SET @cdLoja = NULL;
SET @nmLoja = NULL;
--*/

-- TODO: mover para proc por performance?

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Lojas AS (
	-- Busca conforme permissao por bandeira
	SELECT L.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
				  AND (@idBandeira IS NULL OR PB.IDBandeira = @idBandeira)
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDBandeira = PB.IDBandeira
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 2)
	UNION
	-- Busca conforme permissao por loja
	SELECT PL.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND (@idBandeira IS NULL OR L.IDBandeira = @idBandeira)
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
)
SELECT COUNT(1)
  FROM Loja L WITH (NOLOCK)
 WHERE (@cdLoja IS NULL OR L.cdLoja = @cdLoja)
   AND (@nmLoja IS NULL OR L.nmLoja LIKE '%' + @nmLoja + '%')
   AND EXISTS (SELECT TOP 1 1 FROM Lojas LS WITH (NOLOCK) WHERE LS.IDLoja = L.IdLoja)
