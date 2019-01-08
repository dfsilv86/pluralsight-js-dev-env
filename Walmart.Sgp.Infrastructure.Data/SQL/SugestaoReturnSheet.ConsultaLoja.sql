/*USE WLMSLP_Pess_DEV

DECLARE @Evento AS VARCHAR(max) = NULL
DECLARE @IdLoja AS BIGINT = 1081
DECLARE @IdDepartamento AS BIGINT = 13
DECLARE @DataSolicitacao AS VARCHAR(max) = '2016-03-23 12:53:00'
DECLARE @Vendor9D AS VARCHAR(max) = 0
DECLARE @IDItemDetalhe AS BIGINT = 0*/

SELECT RS.IdReturnSheet, RS.DhInicioReturn, RS.DhFinalReturn, RS.DhInicioEvento, RS.DhFinalEvento, RS.Descricao, NULL AS SplitOn1,
FP.IDFornecedorParametro, FP.cdV9D, FP.cdTipo, F.nmFornecedor, NULL AS SplitOn2,
IDSaida.IdItemDetalhe, IDSaida.cdItem, IDSaida.dsItem, IDEntrada.TpCaixaFornecedor, NULL AS SplitOn3,
SRS.IdSugestaoReturnSheet, SRS.PackSugeridoCompra, SRS.qtVendorPackageItemCompra, SRS.vlPesoLiquidoItemCompra, SRS.QtdLoja, SRS.EstoqueItemVenda, SRS.BlExportado, SRS.BlAutorizado, SRS.BlAtivo, SRS.QtdRA, SRS.PrecoVenda
FROM SugestaoReturnSheet AS SRS WITH (NOLOCK)
INNER JOIN ReturnSheetItemLoja RSIL WITH (NOLOCK)
ON RSIL.IdReturnSheetItemLoja = SRS.IdReturnSheetItemLoja
INNER JOIN ReturnSheetItemPrincipal RSIP WITH (NOLOCK)
ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
INNER JOIN ReturnSheet RS WITH (NOLOCK)
ON RS.IdReturnSheet = RSIP.IdReturnSheet
INNER JOIN ItemDetalhe IDSaida WITH (NOLOCK)
ON IDSaida.IDItemDetalhe = RSIP.IdItemDetalhe
INNER JOIN ItemDetalhe IDEntrada WITH (NOLOCK)
ON IDEntrada.IDItemDetalhe = RSIL.IdItemDetalhe
INNER JOIN FornecedorParametro FP WITH (NOLOCK)
ON FP.IDFornecedorParametro = IDEntrada.IDFornecedorParametro
INNER JOIN Fornecedor F WITH (NOLOCK)
ON F.IDFornecedor = FP.IDFornecedor
WHERE RSIL.IdLoja = @IdLoja AND IDSaida.IDDepartamento = @IdDepartamento AND (@Evento IS NULL OR RS.Descricao LIKE '%'+@Evento+'%')
AND (RS.DhInicioReturn <= @DataSolicitacao AND RS.DhFinalReturn >= @DataSolicitacao) AND (@IDItemDetalhe = 0 OR IDSaida.IDItemDetalhe = @IDItemDetalhe)
AND (@Vendor9D = 0 OR FP.cdV9D = @Vendor9D) AND (SRS.blAtivo = 1)