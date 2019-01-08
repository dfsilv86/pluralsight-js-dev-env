/*
DECLARE @IDUsuario INT, @tipoPermissao INT, @cdSistema INT, @dsBandeira NVARCHAR(20), @idFormato INT;
SET @IDUsuario = 2337;
SET @tipoPermissao = 2;
SET @cdSistema = 1;
SET @dsBandeira = NULL;
*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), BandeirasComPermissao AS (
	---- Busca conforme permissao por bandeira
	SELECT B.IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Bandeira B WITH (NOLOCK)
		           ON B.IDBandeira = PB.IDBandeira
				  AND B.CdSistema = @cdSistema
	 WHERE @TipoPermissao IS NULL OR @TipoPermissao = 2
	UNION
	-- Busca conforme permissao por loja
	SELECT L.IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND L.CdSistema = @cdSistema
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
)
SELECT RowConstrainedResult.*
  FROM ( 

        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
				SELECT	
					B.IDBandeira,
					dsBandeira,
					sgBandeira,
					blAtivo,
					IDFormato
				FROM 
					BandeirasComPermissao BP
						INNER JOIN Bandeira B
							ON BP.IDBandeira = B.IDBandeira
				WHERE 
					@dsBandeira IS NULL OR dsBandeira LIKE '%' + @dsBandeira + '%'
				AND IDFormato = ISNULL(@idFormato, IDFormato)
			   ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum
