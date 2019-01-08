/*DECLARE @idReturnSheet INT = 33
DECLARE @idItemDetalheSaida BIGINT = 649099*/

SELECT
   RSIL.IdReturnSheetItemLoja,
   RSIL.IdReturnSheetItemPrincipal,
   RSIL.IdItemDetalhe,
   RSIL.IdLoja,
   RSIL.PrecoVenda,
   RSIL.blAtivo  
FROM
   ReturnSheetItemPrincipal RSIP WITH(NOLOCK)
INNER JOIN
   ReturnSheetItemLoja RSIL WITH(NOLOCK)
      ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal  
WHERE
   RSIP.IdReturnSheet = @idReturnSheet   
   AND RSIP.IdItemDetalhe = @idItemDetalheSaida