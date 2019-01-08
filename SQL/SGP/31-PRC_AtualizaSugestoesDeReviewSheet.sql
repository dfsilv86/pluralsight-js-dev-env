/****** Object:  StoredProcedure [dbo].[PRC_AtualizaSugestoesDeReviewSheet]    Script Date: 04/28/2016 11:11:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[PRC_AtualizaSugestoesDeReviewSheet]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	DECLARE @dayWeek INT =  DATEPART(dw, GETDATE()),
			@currentDate DATE = CONVERT(DATE, GETDATE(), 103)



		IF OBJECT_ID('tempdb.dbo.#tmpProxReviewDate', 'U') IS NOT NULL
			DROP TABLE #tmpProxReviewDate;

	CREATE TABLE #tmpProxReviewDate
	(
		dwAtual INT,
		dwNReview INT,
		tpInterval INT,
		DataProximoReview AS 
		CONVERT(DATE, GETDATE() + (dwNReview - dwAtual) + CASE WHEN (tpInterval = 1)
												      		   THEN (CASE WHEN dwAtual >= dwNReview
												      	  				THEN 7
												      	  				ELSE 0
												      	  			 END)
												      		   ELSE 14
														  END, 103)
	)

	INSERT #tmpProxReviewDate (dwAtual, dwNReview, tpInterval) 
     				   VALUES (@dayWeek, 1, 1),
     						  (@dayWeek, 2, 1),
     						  (@dayWeek, 3, 1),
     						  (@dayWeek, 4, 1),
     						  (@dayWeek, 5, 1),
     						  (@dayWeek, 6, 1),
     						  (@dayWeek, 7, 1),
     						  (@dayWeek, 1, 2),
     						  (@dayWeek, 2, 2),
     						  (@dayWeek, 3, 2),
     						  (@dayWeek, 4, 2),
     						  (@dayWeek, 5, 2),
     						  (@dayWeek, 6, 2),
     						  (@dayWeek, 7, 2);


	WITH Base
	AS 
	(
	SELECT SP.IdSugestaoPedido,
		   ISNULL(FP.cdReviewDate, 0) cdReviewDate,  
		   ISNULL(FP.tpWeek, 0) tpWeek,
		   ISNULL(FP.tpInterval, 0) tpInterval,
		   ISNULL(FP.vlLeadTime, 0) vlLeadTime,
		   dbo.fnRetornaProximoReviewDate(FP.cdReviewDate, NULL) NXReview,
		   ISNULL(ID.vlShelfLife, 0 ) vlShelfLife,
		   ISNULL(ID.qtVendorPackage, 0) qtVendorPackage,
		   ISNULL(ID.vlModulo, 0) vlModulo,
		   dbo.fnObterPosicaoEstoque(ID.IdItemDetalhe , SP.IDLoja, GETDATE()) Estoque
	  FROM SugestaoPedido SP INNER JOIN FornecedorParametro FP 
									 ON FP.IdFornecedorParametro  = SP.IdFornecedorParametro 
							 INNER JOIN ItemDetalhe ID 
									 ON ID.IdItemDetalhe = SP.IdItemDetalheSugestao
	 WHERE dtPedido = @currentDate
	   AND cdOrigemCalculo <> 'S'
	)
	UPDATE SP
	   SET SP.cdReviewDate = B.cdReviewDate,
		   SP.tpWeek = B.tpWeek,
		   SP.tpInterval = B.tpInterval,
		   SP.vlLeadTime = B.vlLeadTime,
		   SP.dtProximoReviewDate = T.DataProximoReview,
		   SP.vlShelfLife = B.vlShelfLife,
		   SP.qtVendorPackage = B.qtVendorPackage,
		   SP.vlModulo = B.vlModulo,
		   SP.VLEstoqueOriginal = B.Estoque,
		   SP.dtInicioForecast = DATEADD(DAY, B.vlLeadTime + 1, GETDATE()),
		   SP.dtFimForecast = DATEADD(DAY, B.vlLeadTime,T.DataProximoReview),
		   SP.blPossuiVendasUltimaSemana = dbo.fnPossuiVendasPeriodo(SP.IdLoja, SP.IdItemDetalheSugestao, null, null)
	  FROM SugestaoPedido SP INNER JOIN Base B 
									 ON SP.idSugestaoPedido = B.IdSugestaoPedido
							 INNER JOIN #tmpProxReviewDate  T
									 ON T.dwNReview = B.NXReview
									AND T.tpInterval = B.tpInterval;
																										
	-- Atualiza Forecast Intes Infore GRS	
	WITH Itens
	AS
	(
		SELECT SP.IDSugestaoPedido,
			   SP.IDItemDetalheSugestao, 
			   SP.IdLoja,
			   CASE WHEN DATEPART(WEEKDAY, SP.dtInicioForecast) >  DATEPART(WEEKDAY, SP.dtFimForecast)
					THEN DATEPART(WEEKDAY, SP.dtFimForecast)
					ELSE DATEPART(WEEKDAY, SP.dtInicioForecast)
			   END Inicial,
			   CASE WHEN DATEPART(WEEKDAY, SP.dtInicioForecast) >  DATEPART(WEEKDAY, SP.dtFimForecast)
					THEN DATEPART(WEEKDAY, SP.dtInicioForecast)
					ELSE DATEPART(WEEKDAY, SP.dtFimForecast)
			   END Final
		  FROM SugestaoPedido SP
		 WHERE SP.dtPedido = @currentDate
		   AND cdOrigemCalculo IN ('I', 'G')
	),
	Movimentacoes
	AS
	(
		SELECT I.IDSugestaoPedido, CONVERT(DATE, MOV.dtMovimentado, 103) dtMovimentado, SUM(qtdMovimentacao)  * -1 qtdMovimentacao
		  FROM Movimentacao MOV WITH (NOLOCK),
			   Itens  I
		 WHERE MOV.IdLoja = I.IdLoja
		   AND MOV.IDItem = I.IDItemDetalheSugestao
		   AND MOV.IDTipoMovimentacao IN (5,6,7,8,99,100,101,102)
		   AND MOV.dtMovimentado BETWEEN DATEADD(DAY, - 36, GETDATE()) AND DATEADD(DAY, - 1, GETDATE())
		   AND DATEPART(WEEKDAY, MOV.dtMovimentado) BETWEEN I.Inicial AND i.Final
		 GROUP BY I.IDSugestaoPedido, MOV.dtMovimentado
		HAVING SUM(qtdMovimentacao) <> 0
	),
	Consolidado
	AS
	(	
		SELECT IDSugestaoPedido, SUM(qtdMovimentacao) / CASE WHEN  COUNT(*) = 0 THEN 1 ELSE  COUNT(*)  END  vlForecast
		  FROM Movimentacoes
		 GROUP BY IDSugestaoPedido
	)
	UPDATE SP
	   SET SP.vlForecast = C.vlForecast
	  FROM SugestaoPedido SP INNER JOIN Consolidado C
									 ON SP.IDSugestaoPedido = C.IDSugestaoPedido

	
END
