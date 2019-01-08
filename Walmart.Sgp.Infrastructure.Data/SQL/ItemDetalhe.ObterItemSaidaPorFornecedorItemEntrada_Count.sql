/*
DECLARE @cdItem INT, @cdSistema INT;
SET @cdItem = 12345;
SET @cdSistema = 1;
--*/

SELECT COUNT(*)
FROM (
	SELECT DISTINCT IDS.IDItemDetalhe
	  FROM ItemDetalhe IDS WITH (NOLOCK)
		   INNER JOIN Fornecedor F WITH (NOLOCK)
				   ON F.IDFornecedor = IDS.IDFornecedor
		   INNER JOIN FineLine FL WITH (NOLOCK)
				   ON FL.IDFineLine = IDS.IDFineLine
		   INNER JOIN Subcategoria SC WITH (NOLOCK)
				   ON SC.IDSubcategoria = IDS.IDSubcategoria
		   INNER JOIN Categoria CA WITH (NOLOCK)
				   ON CA.IDCategoria = IDS.IDCategoria
		   INNER JOIN Departamento DE WITH (NOLOCK)
				   ON DE.IDDepartamento = IDS.IDDepartamento
		   INNER JOIN Divisao DI WITH (NOLOCK)
				   ON DI.IDDivisao = DE.IDDivisao
		   INNER JOIN RelacionamentoItemPrincipal RP WITH(NOLOCK)
				   ON RP.IDItemDetalhe = IDS.IDItemDetalhe
				   AND RP.IDTipoRelacionamento = 1
		   INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
				   ON RS.IDRelacionamentoItemPrincipal = RP.IDRelacionamentoItemPrincipal
		   INNER JOIN ItemDetalhe IDE WITH(NOLOCK)
				   ON RS.IDItemDetalhe = IDE.IDItemDetalhe
	 WHERE IDS.cdSistema = @cdSistema
	   AND IDS.tpVinculado = 'S'	   
	   AND IDE.IDFornecedorParametro IS NOT NULL
	   AND (@cdItem IS NULL OR IDS.cdItem = @cdItem)
	   AND (@cdPLU IS NULL OR IDS.cdPLU = @cdPLU)
	   AND (@dsItem IS NULL OR IDS.dsItem LIKE '%' + @dsItem + '%')
	   AND (@tpStatus IS NULL OR IDS.tpStatus = @tpStatus)
	   AND (@cdFineLine IS NULL OR FL.cdFineline = @cdFineline)
	   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
	   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
	   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
	   AND (@idFornecedorParametro IS NULL OR IDE.idFornecedorParametro = @idFornecedorParametro)
	   AND (@IDRegiaoCompra IS NULL OR IDS.IDRegiaoCompra = @IDRegiaoCompra)
       AND (@blPerecivel IS NULL OR DE.blPerecivel = @blPerecivel)
) AS Tb
