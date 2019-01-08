/*
DECLARE @idFornecedorParametro INT, @idLoja INT, @dtPedido DATE;

SET @idFornecedorParametro = 8447;
SET @idLoja = 243;
SET @dtPedido = 
*/

SELECT SP.IDSugestaoPedido
     , SP.IDItemDetalhePedido
     , SP.IDItemDetalheSugestao
     , SP.IdLoja
     , SP.dtPedido
     , SP.tpWeek
     , SP.tpInterval
     , SP.cdReviewDate
     , SP.vlLeadTime
     , SP.qtVendorPackage
     , SP.dtProximoReviewDate
     , SP.dtInicioForecast
     , SP.dtFimForecast
     , SP.vlEstoqueSeguranca
     , SP.vlShelfLife
     , SP.vlLeadTimeReal
     , SP.blAtendePedidoMinimo
     , SP.IDFornecedorParametro
     , SP.qtdPackCompra
     , SP.qtdPackCompraOriginal
     , SP.cdOrigemCalculo
     , SP.vlPackSugerido1
     , SP.vlModulo
     , SP.vlEstoque
     , SP.vlTotalPedidosAberto
     , SP.vlPipeline
     , SP.vlForecast
     , SP.vlForecastMedio
     , SP.vlEstoqueSegurancaQtd
     , SP.vlQtdDiasEstoque
     , SP.vlSugestaoPedido
     , SP.vlEstoqueOriginal
     , SP.vlFatorConversao
     , SP.blPossuiVendasUltimaSemana
     , SP.tpStatusEnvio
     , SP.dhEnvioSugestao
	 , SP.vlSaldoOO
	 , SP.vlSaldoIW
	 , SP.vlSaldoIT
     , NULL AS SplitOn1
	 , ID.IDItemDetalhe
     , ID.IDDepartamento
	 , ID.cdSistema
     , NULL AS SplitOn2
     , LJ.IDLoja
     , LJ.IDBandeira
  FROM SugestaoPedido SP WITH (NOLOCK)
       INNER JOIN Loja LJ WITH (NOLOCK)
               ON LJ.IDLoja = SP.IdLoja
       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
               ON ID.IDItemDetalhe = SP.IDItemDetalhePedido
 WHERE SP.IDFornecedorParametro = @idFornecedorParametro
   AND SP.dtPedido = @dtPedido
   AND SP.blAtendePedidoMinimo = 1
   AND SP.IDLoja = @idLoja