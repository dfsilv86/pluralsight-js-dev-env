/*DECLARE @idRoteiro AS INT = 1
DECLARE @idItemDetalhe AS INT = 374371
DECLARE @dtPedido AS VARCHAR(max) = '2016-04-11 00:00:00'*/

SELECT SP.IDItemDetalhePedido,
CASE WHEN SP.qtdSugestaoRoteiroRA IS NULL THEN
	0
ELSE
	SP.qtdSugestaoRoteiroRA
END AS qtdSugestaoRoteiroRA,
SP.IDSugestaoPedido, SP.qtdPackCompra, SP.cdOrigemCalculo, SP.vlPesoLiquido, SP.qtVendorPackage, SP.TpCaixaFornecedor, NULL AS SplitOn1,
ID.IDItemDetalhe, ID.tpCaixaFornecedor, ID.cdItem, ID.dsItem, NULL AS SplitOn2,
R.idRoteiro, R.blKgCx
FROM SugestaoPedido AS SP WITH (NOLOCK)
INNER JOIN RoteiroPedido RP WITH (NOLOCK)
ON RP.idSugestaoPedido = SP.IDSugestaoPedido
INNER JOIN Roteiro R WITH (NOLOCK)
ON R.idRoteiro = RP.idRoteiro
INNER JOIN ItemDetalhe ID WITH (NOLOCK)
ON ID.IDItemDetalhe = SP.IDItemDetalhePedido AND ID.blAtivo = 1
WHERE RP.idRoteiro = @idRoteiro AND R.blAtivo = 1 AND SP.dtPedido = @dtPedido AND ID.IDItemDetalhe = @idItemDetalhe