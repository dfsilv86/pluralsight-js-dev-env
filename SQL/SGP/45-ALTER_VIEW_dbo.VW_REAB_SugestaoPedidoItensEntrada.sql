
ALTER VIEW [dbo].[VW_REAB_SugestaoPedidoItensEntrada]
AS
WITH Itens
AS
(
	SELECT * FROM fnReabSugestaoPedidoItensCross (NULL)
	
	UNION ALL 
	
	SELECT * FROM fnReabSugestaoPedidoItensDSD (NULL)
	
	UNION ALL 

	SELECT * FROM fnReabSugestaoPedidoItensStaple (NULL)

	UNION ALL 

	SELECT * FROM fnReabSugestaoPedidoItensDiretoSaida (NULL)
),
Lojas
AS
(
	SELECT IdLoja, 'S' tpRegiao 
	  FROM Loja WITH(NOLOCK)
	 WHERE blCalculaSugestao = 1
	   AND dsEstado  NOT IN ('PE','AL','CE','RN','PI','MA','PB','BA','SE')
	   AND DataConversao IS NOT NULL

	UNION ALL

	SELECT L.IdLoja, 'N' tpRegiao 
	  FROM Loja L WITH(NOLOCK)
	 WHERE blCalculaSugestao = 1
		AND L.dsEstado IN ('PE','AL','CE','RN','PI','MA','PB','BA','SE')
		AND L.DataConversao IS NOT NULL
),
ItensFiltrados
AS
(
	SELECT I.*, L.tpRegiao 
	  FROM Itens I WITH(NOLOCK) INNER JOIN Lojas L WITH(NOLOCK)
						   ON I.IdLoja = L.IdLoja
	 
),
ReviewDates
AS
(
	SELECT RD.IDReviewCycleCode, RD.UltimoReview, RD.ProximoReview
	  FROM ReviewDate RD WITH (NOLOCK)
     WHERE RD.IDReviewCycleCode LIKE '%' + LTRIM(RTRIM(DATEPART(WEEKDAY,GETDATE()))) + '%'
)
SELECT XX.IdFornecedor,
	   XX.ID_ITEM_ENTRADA,
	   XX.CD_ITEM_ENTRADA,
	   XX.ID_ITEM_SAIDA,
	   XX.CD_ITEM_SAIDA,
	   XX.IdLoja,
	   XX.cdLoja,
	   XX.VLLEADTIME,
	   XX.DataUltimoPedido,
	   XX.DataProximoPedido,
	   XX.PVReview,
	   XX.NXReview,
	   XX.CDREVIEWDATE,
	   XX.DataInicialForecast,			   
	   XX.TPWEEK,
	   CASE WHEN XX.Estoque < 0 THEN 0 ELSE XX.Estoque END Estoque,
	   XX.cdSequenciaVendor,
	   XX.cdDepartamentoVendor,
	   ISNULL(XX.vlModulo, 1) vlModulo,
	   XX.qtVendorPackage,
	   XX.vlTipoReabastecimento, 
	   XX.TPINTERVAL,	   
	   XX.IDFornecedorParametro,
	   XX.vlShelfLife,
	   CASE XX.vlFatorConversao WHEN 0 THEN 1 ELSE XX.vlFatorConversao END vlFatorConversao , 
	   XX.blPossuiVendasUltimaSemana,
	   XX.DataProximoPedido + XX.VLLEADTIME DataFinalForecast, 
	   dbo.fnTotalNotaNoPeriodo(XX.ID_ITEM_ENTRADA, XX.IdLoja, XX.DataUltimoPedido, XX.DataUltimoPedido + XX.VLLEADTIME) ExisteNota, 
	   tpRegiao,
	   tpCalculoSeguranca,
	   IDOrigemDadosCalculo,
	   IDCD,
	   tpCaixaFornecedor,
	   vlPesoLiquido,
	   blCDConvertido
  FROM (
		SELECT IdFornecedor,
			   ID_ITEM_ENTRADA,
			   CD_ITEM_ENTRADA,
			   ID_ITEM_SAIDA,
			   CD_ITEM_SAIDA,
			   IdLoja,
			   cdLoja,
			   VLLEADTIME,
			   CASE WHEN PVReview < wDay THEN 
						GETDATE() + (PVReview - wDay)
					WHEN PVReview > wDay THEN 
						GETDATE() + (PVReview - wDay - 7 + CASE WHEN TPINTERVAL = 1 THEN 0 ELSE - 7 END)
					ELSE 
						GETDATE() + (- 7 + CASE WHEN TPINTERVAL = 1 THEN 0 ELSE -7 END)
			   END DataUltimoPedido,
			    CASE WHEN wDay < NXReview THEN 
						GETDATE() + (NXReview - wDay) + AddDiasProximoPedido
					 WHEN wDay > NXReview THEN 
						GETDATE() + 7 - (wDay - NXReview) + AddDiasProximoPedido
					 ELSE 
						GETDATE() + 7 + AddDiasProximoPedido
				END DataProximoPedido,
			   PVReview,
			   NXReview,
			   CDREVIEWDATE,
			   getdate() + VLLEADTIME + 1 as DataInicialForecast,			   
			   TPWEEK,
			   --dbo.fnObterPosicaoEstoque(ID_ITEM_SAIDA, IdLoja, GETDATE()) Estoque,
			   0 Estoque,
			   cdSequenciaVendor,
			   cdDepartamentoVendor,
			   vlModulo,
			   qtVendorPackage,
			   vlTipoReabastecimento, 
			   TPINTERVAL,	   
			   IDFornecedorParametro,
			   vlShelfLife,
			   vlFatorConversao, 
			   blPossuiVendasUltimaSemana, 
			   tpRegiao,
			   tpCalculoSeguranca,
			   IDOrigemDadosCalculo,
			   IDCD,
			   tpCaixaFornecedor,
			   vlPesoLiquido,
			   blCDConvertido	
		  FROM 
		  (	SELECT 
				   IdFornecedor,
				   ID_ITEM_ENTRADA,
				   CD_ITEM_ENTRADA,
				   ID_ITEM_SAIDA,
				   CD_ITEM_SAIDA,
				   IdLoja,
				   cdLoja,
				   VLLEADTIME,
				   CDREVIEWDATE,	
				   TPINTERVAL,	   
				   getdate() + VLLEADTIME + 1 as DataInicial,
				   RD.UltimoReview PVReview,
				   RD.ProximoReview NXReview,
				   DATEPART(WEEKDAY,GETDATE()) wDay,
					TPWEEK,
					cdSequenciaVendor,
					cdDepartamentoVendor,
					vlModulo,
					qtVendorPackage,
					vlTipoReabastecimento,
					IDFornecedorParametro,
					vlShelfLife,
					CASE WHEN (TPINTERVAL = 0 OR TPINTERVAL = 1)
						THEN 0
						ELSE 7
					END AddDiasProximoPedido,
					vlFatorConversao,
					blPossuiVendasUltimaSemana, 
					tpRegiao,
					tpCalculoSeguranca,
					IDOrigemDadosCalculo,
					IDCD,
				   tpCaixaFornecedor,
				   vlPesoLiquido,
				   blCDConvertido	
			  FROM ItensFiltrados IFI WITH(NOLOCK) INNER JOIN ReviewDates RD WITH(NOLOCK)
											  ON IFI.cdReviewDate = RD.IDReviewCycleCode 
			) X
		) XX










GO


