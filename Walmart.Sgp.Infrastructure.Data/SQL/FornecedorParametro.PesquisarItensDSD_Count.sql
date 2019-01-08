 /*
 DECLARE	@cdV9D BIGINT = NULL;
 SET @cdV9D = 1;
 --*/
 
 
SELECT COUNT(*)
FROM ItemDetalhe AS ID WITH (NOLOCK)
INNER JOIN FornecedorParametro FP WITH (NOLOCK)
ON FP.IDFornecedorParametro = ID .idFornecedorParametro
WHERE FP.cdV9D = @cdV9D AND ID.vlTipoReabastecimento IN (7,37,97) AND ID.blAtivo = 1 AND ID.tpStatus = 'A'