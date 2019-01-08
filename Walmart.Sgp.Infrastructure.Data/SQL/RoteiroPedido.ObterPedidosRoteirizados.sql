/*DECLARE @dtPedido AS VARCHAR(max) = '2016-04-05 00:00:00'
DECLARE @idDepartamento AS BIGINT = 0
DECLARE @cdV9D AS BIGINT = NULL
DECLARE @stPedido AS BIT = NULL
DECLARE @roteiro AS VARCHAR(max) = NULL*/

SELECT FP.cdV9D, F.nmFornecedor, R.Descricao, R.blKgCx, R.vlCargaMinima, 
CAST((SELECT COUNT(1) FROM RoteiroPedido AS RPP WITH (NOLOCK)
INNER JOIN SugestaoPedido SPP  WITH (NOLOCK)
ON SPP.IDSugestaoPedido = RPP.idSugestaoPedido
WHERE RPP.IdRoteiro = R.idRoteiro AND RPP.blAutorizado = 1 AND SPP.dtPedido = @dtPedido) AS bit) AS blAutorizado, R.idRoteiro
FROM Roteiro AS R WITH (NOLOCK)
INNER JOIN RoteiroPedido RP WITH (NOLOCK)
	ON RP.idRoteiro = R.idRoteiro AND R.blAtivo = 1
INNER JOIN SugestaoPedido SP WITH (NOLOCK)
	ON SP.IDSugestaoPedido = RP.idSugestaoPedido
INNER JOIN FornecedorParametro FP WITH (NOLOCK)
	ON FP.cdV9D = R.cdV9D AND FP.blAtivo = 1
INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	ON ID.IDItemDetalhe = SP.IDItemDetalhePedido AND ID.blAtivo = 1
INNER JOIN Fornecedor F WITH (NOLOCK)
	ON F.IDFornecedor = FP.IDFornecedor AND F.blAtivo = 1
WHERE SP.dtPedido = @dtPedido
  AND ID.IDDepartamento = @idDepartamento
  AND (FP.cdV9D = @cdV9D OR @cdV9D IS NULL)
  AND (RP.blAutorizado = @stPedido OR @stPedido IS NULL)
  AND (R.Descricao LIKE '%' + @roteiro + '%' OR @roteiro IS NULL)
GROUP BY R.idRoteiro, FP.cdV9D, F.nmFornecedor, R.Descricao, R.blKgCx, R.vlCargaMinima