/*DECLARE @idReturnSheet INT = 33
DECLARE @cdItem INT = 500051463*/

SELECT
   IdReturnSheetItemPrincipal   
FROM
   ReturnSheetItemPrincipal RSIP WITH(NOLOCK)  
INNER JOIN
   ItemDetalhe IDD WITH(NOLOCK)  
      ON IDD.IDItemDetalhe = RSIP.IdItemDetalhe  
WHERE
   IDD.cdItem = @cdItem  
   AND RSIP.IdReturnSheet = @idReturnSheet