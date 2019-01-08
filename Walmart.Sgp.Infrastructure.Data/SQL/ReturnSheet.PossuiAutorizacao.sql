/*
DECLARE @IdReturnSheet BIGINT;
SET @IdReturnSheet = NULL;
--*/

SELECT COUNT(1) FROM SugestaoReturnSheet AS SRS WITH(NOLOCK)
INNER JOIN ReturnSheetItemLoja RSIL WITH(NOLOCK)
ON SRS.IdReturnSheetItemLoja = RSIL.IdReturnSheetItemLoja
INNER JOIN ReturnSheetItemPrincipal RSIP WITH(NOLOCK)
ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
INNER JOIN ReturnSheet RS WITH(NOLOCK)
ON RS.IdReturnSheet = RSIP.IdReturnSheet
WHERE SRS.blAutorizado = 1 AND RS.IdReturnSheet = @IdReturnSheet