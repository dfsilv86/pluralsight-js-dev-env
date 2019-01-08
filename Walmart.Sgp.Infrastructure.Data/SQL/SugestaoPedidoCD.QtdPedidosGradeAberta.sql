/*
DECLARE @dtPedido AS VARCHAR(max), @idItemDetalheSaida AS BIGINT, @idFornecedorParametro AS BIGINT, @cdSistema AS BIGINT;

SET @dtPedido = '2016-07-06 00:00:00';
SET @idItemDetalheSaida = 1;
SET @idFornecedorParametro = 1;
SET @cdSistema = 1;
--*/

SELECT COUNT(1)
FROM SugestaoPedidoCD SP
JOIN Trait T WITH (NOLOCK)
                ON T.IdItemDetalhe = SP.idItemDetalhePedido
                                AND T.blAtivo = 1
                                AND T.cdSistema = @cdSistema
JOIN LojaCDParametro LCP WITH (NOLOCK)
                ON LCP.IDLoja = T.IdLoja
                                AND LCP.blAtivo = 1
                                AND LCP.cdSistema = T.cdSistema
                                AND LCP.IDCD = SP.idCD
WHERE SP.dtEnvioPedido = @dtPedido
                AND SP.qtdPackCompra > 0
                AND SP.blFinalizado = 1
                AND SP.tpStatusEnvio IS NULL
                AND SP.IDItemDetalheSugestao = @idItemDetalheSaida
                        AND SP.idFornecedorParametro = @idFornecedorParametro