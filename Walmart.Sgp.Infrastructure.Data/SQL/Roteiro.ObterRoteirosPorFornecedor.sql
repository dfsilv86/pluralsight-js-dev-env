/*DECLARE @cdV9D BIGINT = NULL
DECLARE @cdDepartamento INT = NULL
DECLARE @cdLoja INT = NULL
DECLARE @roteiro VARCHAR(max) = NULL*/

SELECT 
	F.nmFornecedor,
	NULL as SplitOn1,
	R.idRoteiro, R.cdV9D, R.blKgCx, R.vlCargaMinima, R.Descricao
FROM Roteiro R WITH(NOLOCK)
JOIN FornecedorParametro FP WITH(NOLOCK) ON FP.IDFornecedorParametro = 
	(SELECT TOP 1 IDFornecedorParametro 
	 FROM FornecedorParametro WITH(NOLOCK)
	 WHERE cdV9D = R.cdV9D
		AND blAtivo = 1
		AND cdTipo IN('L','D')
	 ORDER BY cdTipo DESC)
JOIN Departamento D WITH(NOLOCK) ON D.IDDepartamento = FP.IdDepartamento
JOIN Fornecedor F WITH(NOLOCK) ON F.IDFornecedor = FP.IDFornecedor  
WHERE 
	R.blAtivo = 1
AND (@roteiro IS NULL OR R.Descricao LIKE '%' + @roteiro + '%')
AND	(@cdV9D IS NULL OR R.cdV9D = @cdV9D)
AND (@cdDepartamento IS NULL OR D.cdDepartamento = @cdDepartamento)
AND (@cdLoja IS NULL OR EXISTS(SELECT 1 
							   FROM RoteiroLoja RL WITH(NOLOCK)
							   JOIN Loja L ON L.IDLoja = RL.idLoja
							   WHERE RL.idRoteiro = R.idRoteiro
								 AND L.cdLoja = @cdLoja AND RL.blativo = 1))