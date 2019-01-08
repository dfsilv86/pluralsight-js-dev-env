/*
DECLARE	@cutoffDate DATETIME, @CreatedMachineName VARCHAR(100);
SET @cutoffDate = GETDATE()-2;
PRINT @cutoffDate;
--*/

SELECT *
  FROM ProcessOrder PO WITH (READPAST)
 WHERE (PO.State <> 0 AND PO.State <> 2)  -- não remove o que ainda não teve oportunidade de ser executado.
   AND PO.ModifiedDate < @cutoffDate
   AND (@CreatedMachineName IS NULL OR PO.CreatedMachineName = @CreatedMachineName);