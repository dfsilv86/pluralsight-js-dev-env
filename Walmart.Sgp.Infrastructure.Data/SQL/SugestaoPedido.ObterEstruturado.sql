
/*
DECLARE @idSugestaoPedido INT;
SET @idSugestaoPedido = 43375899;
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
	 , ISNULL(SP.blReturnSheet, 0) as blReturnSheet
	 , ISNULL(sp.blCDConvertido, 0) as blCDConvertido
	 , ISNULL(sp.TpCaixaFornecedor, 'F') as TpCaixaFornecedor
	 , sp.vlPesoLiquido
	 , SP.vlSaldoOO
	 , SP.vlSaldoIW
	 , SP.vlSaldoIT
	 , SP.idCD
	 , SP.vlTipoReabastecimento
     , NULL AS SplitOn1
	 , ID.IDItemDetalhe
     , ID.IDDepartamento
	 , ID.cdSistema
	 , ID.cdItem
	 , ID.dsItem
     , NULL AS SplitOn2
     , LJ.IDLoja
     , LJ.IDBandeira
	 , LJ.cdLoja
	 , LJ.nmLoja
	 , LJ.idRegiaoAdministrativa
     , NULL AS SplitOn3
	 , ID2.IDItemDetalhe
     , ID2.IDDepartamento
	 , ID2.cdSistema
	 , ID2.cdItem
	 , ID2.dsItem
     , NULL AS SplitOn4
     , FL.cdFineLine
     , FL.dsFineLine
	 , NULL AS SplitOn5
     , ODC.dsOrigem
  FROM SugestaoPedido SP WITH (NOLOCK)
       INNER JOIN Loja LJ WITH (NOLOCK)
               ON LJ.IDLoja = SP.IdLoja
       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
               ON ID.IDItemDetalhe = SP.IDItemDetalhePedido
       INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
               ON ID2.IDItemDetalhe = SP.IDItemDetalheSugestao
	   INNER JOIN FineLine FL WITH (NOLOCK)
	           ON FL.IDFineline = ID2.IDFineline
	   LEFT JOIN OrigemDadosCalculo ODC WITH (NOLOCK)
	           ON ODC.IDOrigemDadosCalculo = SP.IDOrigemDadosCalculo	
 WHERE SP.IDSugestaoPedido = @idSugestaoPedido