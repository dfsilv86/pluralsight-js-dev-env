/*
DECLARE @cdV9D BIGINT = 100230960
DECLARE @cdSistema INT = 1
*/

SELECT F.IDFornecedor
	 , F.nmFornecedor
	 , F.cdFornecedor
	 , NULL AS SplitOn1
	 , FP.IDFornecedorParametro
	 , FP.cdTipo
	 , FP.tpStoreApprovalRequired
  FROM Fornecedor F WITH(NOLOCK)
	   INNER JOIN FornecedorParametro FP WITH(NOLOCK)
		       ON FP.IDFornecedor = F.IDFornecedor
 WHERE FP.cdV9D = @cdV9D
   AND FP.cdSistema = @cdSistema
   AND FP.cdStatusVendor = 'A'
   AND F.cdSistema = @cdSistema
   AND F.stFornecedor = 'A'