/*
DECLARE @IDFineLine INT, @cdPlu INT, @IDDepartamento INT, @tpStatus CHAR(1), @cdItem BIGINT, @dsItem NVARCHAR(100), @IDUsuario INT, @IDBandeira INT, @cdSistema INT, @TipoPermissao INT;
SET @cdSistema = 1;
SET @IDUsuario = 20;
--SET @cdItem = 9564885;
--*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), BandeirasComPermissao AS (
	---- Busca conforme permissao por bandeira
	SELECT B.IDBandeira, null as IDLoja
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Bandeira B WITH (NOLOCK)
		           ON B.IDBandeira = PB.IDBandeira
				  AND B.CdSistema = @cdSistema
	 WHERE @TipoPermissao IS NULL OR @TipoPermissao = 2
	UNION
	-- Busca conforme permissao por loja
	SELECT null as IDBandeira, l.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND L.CdSistema = @cdSistema
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
), ItemBandeira AS (
	SELECT ID.IDItemDetalhe
		 , LJ.IDBandeira
	  FROM ItemDetalhe ID WITH (NOLOCK)
		   INNER JOIN Trait TR WITH (NOLOCK)
				   ON TR.IdItemDetalhe = ID.IDItemDetalhe
		   INNER JOIN Loja LJ WITH (NOLOCK)
				   ON LJ.IDLoja = TR.IdLoja
	       INNER JOIN BandeirasComPermissao BCP WITH (NOLOCK)
	               ON (LJ.IDBandeira = BCP.IDBandeira OR LJ.IDLoja = BCP.IDLoja)
     WHERE (@IDFineLine IS NULL OR ID.IDFineline = @IDFineLine)
       AND (@IDDepartamento IS NULL OR ID.IDDepartamento = @IDDepartamento)
       AND (@IDBandeira IS NULL OR LJ.IDBandeira = @IDBandeira)
       AND (@cdSistema IS NULL OR TR.cdSistema = @cdSistema)
       AND (@cdPlu IS NULL OR ID.cdPLU = @cdPlu)
       AND (@cdItem IS NULL OR ID.cdItem = @cdItem)
       AND (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
       AND (@tpStatus IS NULL OR ID.tpStatus = @tpStatus)
	 GROUP BY ID.IDItemDetalhe
			, LJ.IDBandeira
)
SELECT RowConstrainedResult.*
  FROM ( 

        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
			SELECT ID.IDItemDetalhe
				 , ID.dsItem
				 , ID.cdItem
				 , ID.cdPLU
				 , ID.cdUPC
				 , ID.dsTamanhoItem
				 , ID.tpManipulado
				 , ID.tpVinculado
				 , ID.tpReceituario
				 , NULL AS SplitOn1
				 , DE.dsDepartamento
				 , NULL AS SplitOn2
				 , BA.IDBandeira
				 , BA.dsBandeira
			  FROM ItemBandeira IB
				   INNER JOIN ItemDetalhe ID WITH (NOLOCK)
						   ON ID.IDItemDetalhe = IB.IDItemDetalhe
				   INNER JOIN Departamento DE WITH (NOLOCK)
						   ON DE.IDDepartamento = ID.IDDepartamento
				   INNER JOIN Bandeira BA WITH (NOLOCK)
						   ON BA.IDBandeira = IB.IDBandeira
       		) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum;
