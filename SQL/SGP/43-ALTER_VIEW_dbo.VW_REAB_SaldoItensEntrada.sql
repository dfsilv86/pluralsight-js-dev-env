
ALTER VIEW [dbo].[VW_REAB_SaldoItensEntrada]
AS
WITH Lojas
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
ItensX
AS
(
	SELECT ID_ITEM_ENTRADA, IDFornecedorParametro, ID_ITEM_SAIDA, S.IdLoja, L.tpRegiao, S.VLLEADTIME, S.TPINTERVAL, S.CDREVIEWDATE
	FROM WLMSLP_STAGE..SugestaoPedidoItemEntrada S
	INNER JOIN Lojas L ON L.IDLoja = S.IdLoja
),
Itens
AS
	(
	-- 7, 33, 37
	SELECT  IDS.cdItem cdItemSaida,
			L.IdLoja,
			L.cdLoja,
			IDE.IdItemDetalhe IdItemEntrada, 
			IDE.cdItem cdItemEntrada, 
			IDE.vlTipoReabastecimento,
			IDE.vlModulo,
			IDE.qtVendorPackage,
			IDE.vlFatorConversao,				
			IX.tpRegiao,
			IX.tpInterval,
			IX.vlLeadTime,
			CASE WHEN IDE.vlTipoReabastecimento IN (7, 33, 37) THEN dbo.fnRetornaUltimoReviewDate(IX.cdReviewDate, NULL)
				ELSE dbo.fnRetornaUltimoReviewDate('1234567', NULL) END PVReview
	  FROM ItensX IX
	  INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
		ON IDS.IDItemDetalhe = IX.ID_ITEM_SAIDA
	  INNER JOIN Loja L WITH(NOLOCK)
		ON L.IDLoja = IX.IdLoja
	  INNER JOIN ItemDetalhe IDE WITH(NOLOCK)
	    ON IDE.IDItemDetalhe = IX.ID_ITEM_ENTRADA
	  
	  
)
SELECT cdItemSaida,
	   IDLoja,
	   cdLoja ,
	   IdItemEntrada,
	   cdItemEntrada, 
	   vlTipoReabastecimento,
	   ISNULL(vlModulo,1) vlModulo,
	   qtVendorPackage,
	   vlFatorConversao,				
	   tpRegiao,
	   dbo.fnTotalNotaNoPeriodo(IdItemEntrada, IdLoja, DataUltimoPedido, DataUltimoPedido + vlLeadTime) ExisteNota
  FROM (SELECT cdItemSaida,
			   IDLoja,
			   cdLoja ,
			   IdItemEntrada,
			   cdItemEntrada, 
			   vlTipoReabastecimento,
			   vlModulo,
			   qtVendorPackage,
			   vlFatorConversao,				
			   tpRegiao,
			   vlLeadTime,	   			   
			   CASE WHEN PVReview < DATEPART(WEEKDAY,GETDATE()) 
					THEN GETDATE() + (PVReview - DATEPART(WEEKDAY,GETDATE()))
				WHEN PVReview > DATEPART(WEEKDAY,GETDATE())
					THEN GETDATE() + (PVReview - DATEPART(WEEKDAY,GETDATE()) - 7 + CASE WHEN TPINTERVAL = 1 THEN 0 ELSE - 7 END)
				ELSE
					GETDATE() + (- 7 + CASE WHEN TPINTERVAL = 1 THEN 0 ELSE -7 END)
				END DataUltimoPedido
		  FROM Itens WITH(NOLOCK)
		  ) AS X




GO


