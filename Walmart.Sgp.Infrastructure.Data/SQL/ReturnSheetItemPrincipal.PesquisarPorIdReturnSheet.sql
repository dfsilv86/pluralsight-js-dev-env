/*
DECLARE @idReturnSheet BIGINT;
SET @idReturnSheet = NULL;
--*/

SELECT RSIP.IdReturnSheetItemPrincipal, RSIP.blAtivo, (SELECT COUNT(1) FROM ReturnSheetItemLoja AS RSIL WHERE IdReturnSheetItemPrincipal = RSIP.IdReturnSheetItemPrincipal AND RSIL.blAtivo = 1) AS Lojas,
NULL AS SplitOn1,
		RS.IdReturnSheet, RS.Descricao, RS.DhAtualizacao, RS.DhCriacao, RS.DhFinalEvento, RS.DhFinalReturn, RS.DhInicioEvento, RS.DhInicioReturn,
		RS.IdUsuarioCriacao, NULL AS SplitOn2,
		ID.IDItemDetalhe, ID.dsItem, ID.IDCategoria, ID.IDFornecedorParametro, ID.IDFornecedor, ID.cdItem, ID.cdSistema, ID.cdOldNumber, ID.tpStatus, ID.tpVinculado, NULL AS SplitOn3,
		RC.IdRegiaoCompra, RC.dsRegiaoCompra, NULL AS SplitOn4,
		DP.IDDepartamento, DP.IDDivisao, DP.cdDepartamento, DP.dsDepartamento, DP.blPerecivel
FROM ReturnSheetItemPrincipal AS RSIP WITH(NOLOCK)
INNER JOIN ReturnSheet RS WITH(NOLOCK)
ON RS.IdReturnSheet = RSIP.IdReturnSheet
INNER JOIN ItemDetalhe ID WITH(NOLOCK)
ON ID.IDItemDetalhe = RSIP.IdItemDetalhe
INNER JOIN RegiaoCompra RC WITH(NOLOCK)
ON RC.IdRegiaoCompra = RS.IdRegiaoCompra
INNER JOIN Departamento DP WITH(NOLOCK)
on DP.IdDepartamento = RS.IdDepartamento
WHERE RSIP.IdReturnSheet = @idReturnSheet
AND RSIP.blAtivo = 1