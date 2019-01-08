/*
DECLARE @idItem INT;
SET @idItem = 719695;
*/

SELECT		TOP 1
			FP.IDFornecedorParametro
			,FP.IDFornecedor
			,FP.blAtivo
			,FP.cdV9D					
FROM		FornecedorParametro FP
			,ItemDetalhe ID	
WHERE		ID.IDItemDetalhe = @idItem			
AND			FP.IDFornecedor = ID.IDFornecedor
AND			FP.cdStatusVendor = 'A'
AND			FP.cdV9D IS NOT NULL
AND			SUBSTRING(CAST(FP.cdV9D AS VARCHAR), LEN(FP.cdV9D) - 2, 3) = (CAST(ID.cdDepartamentoVendor AS VARCHAR) + 
														CAST(ID.cdSequenciaVendor AS VARCHAR))
AND			(
				(ID.vlTipoReabastecimento IN (3, 33, 37, 7)
				AND FP.cdTipo IN ('D', 'L'))
				OR FP.cdTipo IN ('W', 'L')
			)