/*
DECLARE @cdSistema INT, @idBandeira INT, @cdLoja INT, @nmLoja VARCHAR, @tpReabastecimento VARCHAR

SET @cdSistema = 20;
SET @idBandeira = 2;
--*/

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
), LojasFiltradas AS (
	SELECT 
		L.IDLoja,
		L.cdLoja,
		L.nmLoja
	  FROM Loja L WITH (NOLOCK)
	 WHERE (@cdLoja IS NULL OR L.cdLoja = @cdLoja)
	   AND (@nmLoja IS NULL OR L.nmLoja LIKE '%' + @nmLoja + '%')
	   AND (@cdSistema IS NULL OR L.cdSistema = @cdSistema)
	   AND(@idBandeira IS NULL OR L.idBandeira = @idBandeira)
	   AND EXISTS (SELECT TOP 1 1 FROM Lojas LS WITH (NOLOCK) WHERE LS.IDLoja = L.IdLoja)
)
SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (

SELECT 
	LCPD.*,
	NULL AS SplitOn1,
	LO.cdLoja,
    LO.nmLoja,
	NULL AS SplitOn2,
    CD.cdCD,
    CD.nmNome,
	NULL AS SplitOn3,      
	DP.dsDepartamento,
	NULL AS SplitOn4,
	RD.cdReviewDate, 
    RD.tpReabastecimento
FROM 
	LojaCDParametro LCPD INNER JOIN LojasFiltradas LO WITH (NOLOCK)
                            ON LCPD.IDLoja = LO.IDLoja 
                    INNER JOIN CD CD WITH (NOLOCK)
                            ON LCPD.IDCD = CD.IDCD 
                    INNER JOIN ReviewDateCD RD WITH (NOLOCK)
                            ON RD.IDLojaCDParametro = LCPD.IDLojaCDParametro
                    INNER JOIN Departamento DP WITH (NOLOCK)
                            ON DP.IDDepartamento = RD.IDDepartamento
WHERE    
	RD.tpReabastecimento = @tpReabastecimento
			   ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum