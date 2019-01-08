/*DECLARE @cdSistema INT = 1
DECLARE @cdItem INT = 500051463
DECLARE @idReturnSheet INT = 33
DECLARE @uf VARCHAR(max) = 'SP'*/

SELECT 
 (SELECT RSIL.IdReturnSheetItemLoja FROM ReturnSheetItemLoja AS RSIL WITH (NOLOCK) 
 INNER JOIN ReturnSheetItemPrincipal RSIP WITH (NOLOCK) 
 ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
 WHERE RSIL.IdLoja = l.IdLoja AND RSIL.IdItemDetalhe = ID1.IdItemDetalheEntrada AND RSIP.IdReturnSheet = @idReturnSheet) AS IdReturnSheetItemLoja,
 ID1.IdItemDetalheSaida
 ,ID1.IdItemDetalheEntrada
 ,l.IdLoja
 ,l.cdLoja
 ,l.nmLoja
 ,l.dsEstado
 ,cd.cdCD
 ,dbo.fnValidarTipoReabastecimentoXCDConvertido(ID1.IdItemDetalheEntrada, lp.IDCD, l.IDLoja) AS valido
 ,CASE WHEN RS.blAtivo = 1 THEN RS.PrecoVenda ELSE NULL END AS PrecoVenda
 ,CASE WHEN RS.IdLoja IS NULL OR RS.blAtivo = 0 THEN 0 ELSE 1 END AS selecionado
FROM Loja l WITH (NOLOCK)
JOIN LojaCDParametro lp WITH (NOLOCK) ON l.IDLoja = lp.IDLoja
 AND lp.blAtivo = 1
JOIN RelacaoItemLojaCD ril WITH (NOLOCK) ON lp.IDLojaCDParametro = ril.IDLojaCDParametro
 AND ril.blAtivo = 1
JOIN CD cd WITH (NOLOCK) ON cd.IDCD = lp.IDCD
JOIN (
	 SELECT id.IDItemDetalhe IDItemDetalheSaida
	  ,CASE 
	   WHEN COALESCE(id.tpVinculado, id.tpReceituario, id.tpManipulado) IS NULL
		THEN id.IDItemDetalhe
	   ELSE rs.IDItemDetalhe
	   END IDItemDetalheEntrada
	  ,ide.IDFornecedorParametro, id.tpVinculado, id.tpReceituario, id.tpManipulado
	 FROM ItemDetalhe id WITH (NOLOCK)
	 LEFT JOIN RelacionamentoItemPrincipal rp WITH (NOLOCK) ON rp.IDItemDetalhe = id.IDItemDetalhe
	 LEFT JOIN RelacionamentoItemSecundario rs WITH (NOLOCK) ON rs.IDRelacionamentoItemPrincipal = rp.IDRelacionamentoItemPrincipal
	 LEFT JOIN ItemDetalhe ide WITH (NOLOCK) ON ide.IDItemDetalhe = rs.IDItemDetalhe
	 WHERE id.cdItem = @cdItem
	 ) ID1 ON ID1.IDItemDetalheEntrada = 
		 CASE WHEN COALESCE(ID1.tpVinculado, ID1.tpReceituario, ID1.tpManipulado) IS NULL THEN -- Validação que a Ana pediu <<<
			ril.IDItem
		 ELSE
			ril.IdItemEntrada
		 END
LEFT JOIN (
 SELECT precovenda
  ,rsil.IdReturnSheetItemLoja
  ,rsil.IdLoja
  ,rsil.blAtivo
  ,rsil.IdItemDetalhe IdItemDetalheEntrada
  ,rsip.IdItemDetalhe IdItemDetalheSaida
 FROM ReturnSheetItemPrincipal rsip WITH (NOLOCK)
 JOIN ReturnSheetItemLoja rsil WITH (NOLOCK) ON rsil.IdReturnSheetItemPrincipal = rsip.IdReturnSheetItemPrincipal
 WHERE rsip.IdReturnSheet = @idReturnSheet
 ) RS ON rs.IdLoja = l.IDLoja
 AND rs.IdItemDetalheEntrada = ID1.IdItemDetalheEntrada
 AND rs.IdItemDetalheSaida = ID1.IdItemDetalheSaida
WHERE lp.cdSistema = @cdSistema AND (dsEstado = @uf OR @uf IS NULL)
