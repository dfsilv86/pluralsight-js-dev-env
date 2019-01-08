SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=======================================================================================================================
Procedure..............: PR_GeraSugestaoPedidoCD
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 25/04/2016 (Projeto PESS)
Objetivo...............: Gerar a sugestao de pedidos para o CD dos itens Staple de peso fixo e peso variável
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: 
Data Alteração.........: 
Descrição da alteração.: 
=======================================================================================================================
*/
create PROCEDURE [dbo].[PR_GeraSugestaoPedidoCD] @DataSugestao DATE = NULL
AS
BEGIN
	DECLARE @TpWeek INT
		,@blExisteSugestao BIT

	SELECT @TpWeek = DV.dsText
	FROM CWIDOMAINVALUE DV
	JOIN CWIDOMAIN D
		ON D.IDDOMAIN = DV.IDDOMAIN
	WHERE D.nmDomain = 'SemanaWalMart'
		AND DV.dsValue = 'W'

	IF @DataSugestao IS NULL
		SET @DataSugestao = GETDATE()
	ELSE IF ABS(DATEPART(wk, GETDATE() + 20) - DATEPART(wk, GETDATE())) % 2 = 1
		SET @TpWeek = CASE @TpWeek
				WHEN 1
					THEN 2
				ELSE 1
				END

	TRUNCATE TABLE WLMSLP_STAGE..SugestaoPedidoCD

	INSERT INTO WLMSLP_STAGE..SugestaoPedidoCD (
		idFornecedorParametro
		,idItemDetalhePedido
		,idItemDetalheSugestao
		,idCD
		,dtPedido
		,dtEnvioPedido
		,dtCancelamentoPedido
		,dtCancelamentoPedidoOriginal
		,dtInicioForecast
		,dtFimForecast
		,tpWeek
		,tpInterval
		,cdReviewDate
		,vlLeadTime
		,qtVendorPackage
		,vlEstoqueSeguranca
		,tempoMinimoCD
		,tpCaixaFornecedor
		,vlPesoLiquido
		,vlTipoReabastecimento
		,vlCusto
		,qtdPackCompra
		,qtdPackCompraOriginal
		,qtdOnHand
		,qtdOnOrder
		,qtdForecast
		,qtdPipeline
		,IdOrigemDadosCalculo
		,qtdDiasAbastecimento
		)
	SELECT IDE.IDFornecedorParametro
		,IDE.IdItemDetalhe IdItemDetalhePedido
		,ISNULL(IDS.IdItemDetalhe, IDE.IdItemDetalhe) IdItemDetalheSugestao
		,RIC.idCD
		,@DataSugestao dtPedido
		,@DataSugestao dtEnvioPedido
		,CONVERT(DATETIME, @DataSugestao) + COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + 1 dtCancelamentoPedido
		,CONVERT(DATETIME, @DataSugestao) + COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + 1 dtCancelamentoPedidoOriginal
		,CONVERT(DATETIME, @DataSugestao) + COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + 1 dtInicioForecast
		,CONVERT(DATETIME, @DataSugestao) + COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + 1 + CASE 
			WHEN COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + DATEPART(WEEKDAY, @DataSugestao) + ISNULL(RIC.vlEstoqueSeguranca, 0) < ISNULL(IDE.tempoMinimoCd, 0)
				THEN COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + DATEPART(WEEKDAY, @DataSugestao) + ISNULL(RIC.vlEstoqueSeguranca, 0)
			ELSE ISNULL(IDE.tempoMinimoCd, 0)
			END dtFimForecast
		,COALESCE(FCP.TPWEEK, FP.TPWEEK) TPWEEK
		,COALESCE(FCP.TPINTERVAL, FP.TPINTERVAL) TPINTERVAL
		,CAST(COALESCE(FCP.cdReviewDate, FP.cdReviewDate) AS VARCHAR) CDREVIEWDATE
		,COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) VLLEADTIME
		,IDE.qtVendorPackage
		,ISNULL(RIC.vlEstoqueSeguranca, 0)
		,ISNULL(IDE.tempoMinimoCd, 0)
		,IDE.tpCaixaFornecedor
		,ISNULL(IDE.vlPesoLiquido, 0)
		,RIC.vlTipoReabastecimento
		,IC.BuyPrice vlCusto
		,0 qtdPackCompra
		,0 qtdPackCompraOriginal
		,ISNULL(IEC.qtdOnHand, 0)
		,ISNULL(IEC.qtdOnOrder, 0)
		,0 qtdForecast
		,CASE 
			WHEN ISNULL(IEC.qtdOnHand, 0) + ISNULL(IEC.qtdOnOrder, 0) < 0
				THEN 0
			ELSE ISNULL(IEC.qtdOnHand, 0) + ISNULL(IEC.qtdOnOrder, 0)
			END qtdPipeline
		,CASE 
			WHEN FCP.idFornecedorCDParametro IS NULL
				THEN 3
			ELSE 2
			END IdOrigemDadosCalculo
		,CASE 
			WHEN COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + DATEPART(WEEKDAY, @DataSugestao) + ISNULL(RIC.vlEstoqueSeguranca, 0) < ISNULL(IDE.tempoMinimoCd, 0)
				THEN COALESCE(FCP.VLLEADTIME, FP.VLLEADTIME, 0) + DATEPART(WEEKDAY, @DataSugestao) + ISNULL(RIC.vlEstoqueSeguranca, 0)
			ELSE ISNULL(IDE.tempoMinimoCd, 0)
			END
	FROM RelacaoItemCD RIC
	LEFT JOIN ItemEstoqueCD IEC
		ON IEC.idCD = RIC.idCD
			AND IEC.idItemDetalhe = RIC.idItemEntrada
	LEFT JOIN ItemDetalhe IDE
		ON IDE.IDItemDetalhe = RIC.idItemEntrada
			AND IDE.tpStatus = 'A'
			AND IDE.cdSistema = 1
			AND IDE.blItemTransferencia = 0
			AND IDE.blAtivo = 1
	JOIN ItemDetalhe IDS
		ON IDS.IdItemDetalhe = RIC.idItemSaida
			AND IDS.tpStatus = 'A'
			AND IDS.blAtivo = 1
			AND IDE.blItemTransferencia = 0
	JOIN FornecedorParametro FP
		ON FP.IDFornecedorParametro = ISNULL(IDE.idFornecedorParametro, IDS.idFornecedorParametro)
			AND FP.cdStatusVendor = 'A'
			AND FP.tpStoreApprovalRequired IN ('Y', 'R')
	JOIN Fornecedor F
		ON F.IDFornecedor = FP.IDFornecedor
			AND F.blAtivo = 1
	LEFT JOIN FornecedorCDParametro FCP
		ON FCP.IDFornecedorParametro = FP.IDFornecedorParametro
			AND FCP.IDCD = RIC.idCD
	JOIN ItemCustoCD IC
		ON IC.idCD = RIC.idCD
			AND IC.idItemDetalhe = RIC.idItemEntrada
			AND IC.dtRecebimento = (
				SELECT MAX(dtRecebimento)
				FROM ItemCustoCD
				WHERE IC.idCD = RIC.idCD
					AND IC.idItemDetalhe = RIC.idItemEntrada
				)
	WHERE RIC.vlTipoReabastecimento IN (20, 22, 40, 42, 43, 81)
		AND CAST(COALESCE(FCP.cdReviewDate, FP.cdReviewDate) AS NVARCHAR) LIKE '%' + CAST(DATEPART(WEEKDAY, @DataSugestao) AS NVARCHAR) + '%'
		AND (
			COALESCE(FCP.tpInterval, FP.tpInterval) IN (1, 0)
			OR (
				COALESCE(FCP.tpInterval, FP.tpInterval) = @tpWeek
				AND COALESCE(FCP.tpWeek, FP.tpWeek) = 2
				)
			)

	EXEC PR_CalcularForecastCD

	UPDATE S
	SET qtdForecast = FS.ValorForecast
		,qtdPackCompraOriginal = CONVERT(INT, CASE 
				WHEN (
						CASE 
							WHEN s.tpCaixaFornecedor = 'F'
								AND qtVendorPackage <> 0
								THEN ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline) / qtVendorPackage
							WHEN s.tpCaixaFornecedor = 'V'
								AND vlPesoLiquido <> 0
								THEN ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline) / vlPesoLiquido
							ELSE ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline)
							END
						) < 0
					THEN 0
				ELSE (
						CASE 
							WHEN s.tpCaixaFornecedor = 'F'
								AND qtVendorPackage <> 0
								THEN ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline) / qtVendorPackage
							WHEN s.tpCaixaFornecedor = 'V'
								AND vlPesoLiquido <> 0
								THEN ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline) / vlPesoLiquido
							ELSE ((FS.ValorForecast * qtdDiasAbastecimento) - qtdPipeline)
							END
						)
				END)
	FROM WLMSLP_STAGE..SugestaoPedidoCD S
	INNER JOIN WLMSLP_STAGE..ForecastSugestaoCd FS
		ON FS.IdItemDetalhePedido = S.idItemDetalhePedido
			AND FS.IdItemDetalheSugestao = S.idItemDetalheSugestao
			AND Fs.idcd = S.idCD

	SELECT TOP 1 @blExisteSugestao = 1
	FROM SugestaoPedidoCD
	WHERE dtPedido = @DataSugestao

	IF ISNULL(@blExisteSugestao, 0) = 0
		INSERT INTO SugestaoPedidoCD (
			idFornecedorParametro
			,idItemDetalhePedido
			,idItemDetalheSugestao
			,idCD
			,dtPedido
			,dtEnvioPedido
			,dtCancelamentoPedido
			,dtCancelamentoPedidoOriginal
			,dtInicioForecast
			,dtFimForecast
			,tpWeek
			,tpInterval
			,cdReviewDate
			,vlLeadTime
			,qtVendorPackage
			,vlEstoqueSeguranca
			,tempoMinimoCD
			,tpCaixaFornecedor
			,vlPesoLiquido
			,vlTipoReabastecimento
			,vlCusto
			,qtdPackCompra
			,qtdPackCompraOriginal
			,qtdOnHand
			,qtdOnOrder
			,qtdForecast
			,qtdPipeline
			,IdOrigemDadosCalculo
			)
		SELECT idFornecedorParametro
			,idItemDetalhePedido
			,idItemDetalheSugestao
			,idCD
			,dtPedido
			,dtEnvioPedido
			,dtCancelamentoPedido
			,dtCancelamentoPedidoOriginal
			,dtInicioForecast
			,dtFimForecast
			,tpWeek
			,tpInterval
			,cdReviewDate
			,vlLeadTime
			,qtVendorPackage
			,vlEstoqueSeguranca
			,tempoMinimoCD
			,tpCaixaFornecedor
			,vlPesoLiquido
			,vlTipoReabastecimento
			,vlCusto
			,qtdPackCompra
			,qtdPackCompraOriginal
			,qtdOnHand
			,qtdOnOrder
			,qtdForecast
			,qtdPipeline
			,IdOrigemDadosCalculo
		FROM WLMSLP_STAGE..SugestaoPedidoCD
		WHERE dtPedido = @DataSugestao
END
