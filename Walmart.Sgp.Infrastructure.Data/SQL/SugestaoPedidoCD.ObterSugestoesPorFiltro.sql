/*
DECLARE @dtSolicitacao VARCHAR(MAX), @idDepartamento INT, @idCD INT, @idItem INT, @idFornecedorParametro INT, @statusPedido INT, @idSugestaoPedidoCD INT;
SET @dtSolicitacao = '2016-04-23';
SET @idDepartamento = 17;
SET @idCD = 12;
SET @idItem = NULL;
SET @idFornecedorParametro = NULL;
SET @statusPedido = 2;
SET @idSugestaoPedidoCD = 1;
--*/

SELECT SPC.idSugestaoPedidoCD, SPC.idFornecedorParametro, SPC.idItemDetalhePedido, SPC.idItemDetalheSugestao, SPC.idCD, SPC.dtPedido, SPC.dtEnvioPedido, SPC.dtCancelamentoPedido, SPC.dtCancelamentoPedidoOriginal, SPC.dtInicioForecast, SPC.dtFimForecast, SPC.tpWeek, SPC.tpInterval, SPC.cdReviewDate, SPC.vlLeadTime, SPC.qtVendorPackage, SPC.vlEstoqueSeguranca, SPC.tempoMinimoCD, SPC.tpCaixaFornecedor, SPC.vlPesoLiquido, SPC.vlTipoReabastecimento, SPC.vlCusto, SPC.qtdPackCompra, SPC.qtdPackCompraOriginal, SPC.qtdOnHand, SPC.qtdOnOrder, SPC.qtdForecast, SPC.qtdPipeline, SPC.IdOrigemDadosCalculo, SPC.blFinalizado, SPC.tpStatusEnvio, SPC.dhEnvioSugestao,
NULL AS SplitOn1,
ItemDetalhePedido.cdItem, ItemDetalhePedido.dsItem, -- Entrada
NULL AS SplitOn2,
FP.cdV9D, FP.IDFornecedor, F.nmFornecedor, FP.cdTipo, D.cdDepartamento, dsDepartamento,
NULL AS SplitOn3,
CD.cdCD,
NULL AS SplitOn4,
FL.cdFineLine,
NULL AS SplitOn5,
ODC.dsOrigem,
ItemDetalheSugestao.cdItem AS cdItemSugestao, ItemDetalheSugestao.dsItem AS dsItemSugestao -- Saida
FROM SugestaoPedidoCD AS SPC WITH(NOLOCK)
INNER JOIN ItemDetalhe ItemDetalhePedido WITH(NOLOCK)
	ON ItemDetalhePedido.IDItemDetalhe = SPC.idItemDetalhePedido
INNER JOIN ItemDetalhe ItemDetalheSugestao WITH(NOLOCK)
	ON ItemDetalheSugestao.IDItemDetalhe = SPC.idItemDetalheSugestao
INNER JOIN FornecedorParametro FP WITH(NOLOCK)
	ON FP.IDFornecedorParametro = SPC.idFornecedorParametro
INNER JOIN Fornecedor F WITH(NOLOCK)
	ON F.IDFornecedor = FP.IDFornecedor
INNER JOIN CD cd WITH(NOLOCK)
	ON cd.IDCD = SPC.idCD
INNER JOIN Fineline FL WITH(NOLOCK)
	ON FL.IDFineLine = ItemDetalhePedido.IDFineline
INNER JOIN OrigemDadosCalculo ODC WITH(NOLOCK)
	ON ODC.IDOrigemDadosCalculo = SPC.IdOrigemDadosCalculo
INNER JOIN Departamento D WITH(NOLOCK)
	ON D.IDDepartamento = FP.idDepartamento
WHERE (SPC.idSugestaoPedidoCD = @idSugestaoPedidoCD) OR (@idSugestaoPedidoCD IS NULL AND (SPC.dtPedido = @dtSolicitacao
	AND SPC.idCD = @idCD
	AND ItemDetalhePedido.IDDepartamento = @idDepartamento
	AND (SPC.blFinalizado = @statusPedido OR @statusPedido = 2)	
	AND ((FP.IDFornecedor = (SELECT TOP 1 IDFornecedor FROM FornecedorParametro WHERE IDFornecedorParametro = @idFornecedorParametro)) OR @idFornecedorParametro IS NULL)
	AND (SPC.idItemDetalhePedido = @idItem OR SPC.idItemDetalheSugestao = @idItem OR @idItem IS NULL)))
