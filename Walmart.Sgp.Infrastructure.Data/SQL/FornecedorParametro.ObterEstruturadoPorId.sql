/*
DEClARE @idFornecedorParametro INT
SET @idFornecedorParametro = 22
--*/

SELECT		FP.IDFornecedorParametro
			,FP.IDFornecedor
			,FP.cdV9D			
			,FP.tpWeek
			,FP.vlValorMinimo
			,FP.tpInterval
            ,FP.blAtivo
			,FP.vlLeadTime
			,FP.tpPedidoMinimo
			,FP.vlFillRate
			,FP.cdTipo
			,FP.cdStatusVendor
			,FP.tpStoreApprovalRequired
			,FP.cdReviewDate
			,NULL AS SplitOn1			
			,F.stFornecedor
			,F.nmFornecedor
			,F.cdSistema			
FROM		FornecedorParametro FP WITH (NOLOCK)		
INNER JOIN	Fornecedor F WITH (NOLOCK)
ON			FP.IDFornecedor = F.IDFornecedor
WHERE		FP.IDFornecedorParametro = @idFornecedorParametro