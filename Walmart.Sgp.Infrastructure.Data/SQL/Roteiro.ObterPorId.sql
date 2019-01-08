/*
DECLARE @idRoteiro INT;
SET @idRoteiro = 1;
--*/

SELECT 
	R.idRoteiro, R.cdV9D, R.blKgCx, R.vlCargaMinima, R.Descricao, R.idUsuarioCriacao, R.dhCriacao, R.blAtivo,
	NULL as SplitOn1,
	F.nmFornecedor
FROM Roteiro R WITH(NOLOCK)
JOIN FornecedorParametro FP WITH(NOLOCK) ON FP.IDFornecedorParametro = 
	(SELECT TOP 1 IDFornecedorParametro 
	 FROM FornecedorParametro
	 WHERE cdV9D = R.cdV9D
		AND blAtivo = 1
		AND cdTipo IN('L','D')
	 ORDER BY cdTipo DESC)
JOIN Fornecedor F WITH(NOLOCK) ON F.IDFornecedor = FP.IDFornecedor
WHERE 
	R.idRoteiro = @idRoteiro