/*
DECLARE @dtPedido AS VARCHAR(max), @idItemDetalheSaida AS BIGINT, @IDFornecedorParametro AS BIGINT, @cdSistema AS BIGINT;

SET @dtPedido = '2016-07-06 00:00:00';
SET @idItemDetalheSaida = 1;
SET @IDFornecedorParametro = 1;
SET @cdSistema = 1;
--*/

SELECT COUNT(*)
FROM SugestaoPedido SP WITH(NOLOCK)
JOIN ItemDetalhe ITS WITH(NOLOCK)
      ON ITS.IDItemDetalhe = SP.IDItemDetalhePedido
JOIN Loja L WITH(NOLOCK)
      ON L.IDLoja = SP.IdLoja
JOIN Trait T WITH (NOLOCK)
                ON T.IdItemDetalhe = SP.idItemDetalhePedido
                                AND T.blAtivo = 1
                                AND T.cdSistema = @cdSistema
                                AND T.IdLoja = SP.IdLoja
JOIN AutorizaPedido AP WITH (NOLOCK)
    ON AP.dtPedido = SP.dtPedido
        AND AP.IdLoja = SP.IdLoja
        AND AP.IdDepartamento = ITS.IDDepartamento
CROSS APPLY dbo.fnBuscaGrade(L.IDBandeira, ITS.IDDepartamento, SP.IDLoja)
WHERE SP.dtPedido = @dtPedido
AND SP.tpStatusEnvio IS NULL
AND SP.qtdPackCompra > 0
AND SP.IDItemDetalheSugestao = @idItemDetalheSaida
and SP.IDFornecedorParametro = @IDFornecedorParametro