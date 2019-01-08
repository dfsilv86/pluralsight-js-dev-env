/*DECLARE @idItemDetalhe AS INT = 374371
DECLARE @idRoteiro AS INT = 1
DECLARE @dtPedido AS VARCHAR(max) = '2016-04-11 00:00:00'*/

SELECT L.IDLoja, L.cdLoja, L.nmLoja, NULL AS SplitOn1, SP.IDSugestaoPedido, SP.IDItemDetalhePedido, SP.qtdPackCompra, SP.cdOrigemCalculo, SP.TpCaixaFornecedor, SP.qtVendorPackage, SP.vlPesoLiquido,
CASE WHEN SP.qtdSugestaoRoteiroRA IS NULL THEN
	0
ELSE
	SP.qtdSugestaoRoteiroRA
END AS qtdSugestaoRoteiroRA, NULL AS SplitOn2,
ID.dsItem, ID.cdItem
FROM SugestaoPedido AS SP WITH (NOLOCK)
INNER JOIN RoteiroPedido RP WITH (NOLOCK)
	ON RP.idSugestaoPedido = SP.IDSugestaoPedido
INNER JOIN Loja L WITH (NOLOCK)
	ON L.IDLoja = SP.IdLoja
INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	ON ID.IDItemDetalhe = SP.IDItemDetalhePedido
WHERE SP.dtPedido = @dtPedido AND SP.IDItemDetalhePedido = @idItemDetalhe AND RP.idRoteiro = @idRoteiro