/*DECLARE @idRoteiro AS INT = 1
DECLARE @dtPedido AS VARCHAR(max) = '2016-04-11 00:00:00'*/

SELECT SP.IDSugestaoPedido, SP.qtdPackCompra, SP.qtdSugestaoRoteiroRA, SP.vlPesoLiquido, SP.qtVendorPackage, SP.tpCaixaFornecedor, NULL AS SplitOn1,
ID.IDItemDetalhe, ID.tpCaixaFornecedor, ID.cdItem, ID.dsItem, NULL AS SplitOn2,
R.idRoteiro, R.blKgCx
FROM SugestaoPedido AS SP WITH (NOLOCK)
INNER JOIN RoteiroPedido RP WITH (NOLOCK)
ON RP.idSugestaoPedido = SP.IDSugestaoPedido
INNER JOIN Roteiro R WITH (NOLOCK)
ON R.idRoteiro = RP.idRoteiro
INNER JOIN ItemDetalhe ID WITH (NOLOCK)
ON ID.IDItemDetalhe = SP.IDItemDetalhePedido AND ID.blAtivo = 1
WHERE R.idRoteiro = @idRoteiro AND R.blAtivo = 1 AND SP.dtPedido = @dtPedido