/*
DECLARE	@IsAdministrator BIT, @CurrentUserId INT, @CreatedUserId INT, @processName VARCHAR(150), @state INT;
SET @CurrentUserId = 2;
SET @IsAdministrator = 1;
--SET @CreatedUserId = 2526316;
SET @ProcessName = 'ImpVincularItemLojaCD';

DECLARE @StateNameCreated VARCHAR(100), @StateNameQueued VARCHAR(100), @StateNameError VARCHAR(100), @StateNameIsExecuting VARCHAR(100), @StateNameFailed VARCHAR(100), @StateNameFinished VARCHAR(100), @StateNameResultsAvailable VARCHAR(100), @StateNameResultsExpunged VARCHAR(100);
SET @StateNameCreated = 'Cr';
SET @StateNameError = 'ER';
SET @StateNameQueued = 'Ag';
SET @StateNameIsExecuting = 'EM';
SET @StateNameFailed = 'Finalizado com erro';
SET @StateNameFinished = 'Finalizado com sucesso';
SET @StateNameResultsAvailable = 'Resultado disponível';
SET @StateNameResultsExpunged = 'Resultado expurgado';
--*/

SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
				SELECT PO.ProcessOrderId
				     , PO.Ticket
					 , PO.ProcessName
					 , PO.[State]
					 , PO.CreatedDate
					 , PO.ModifiedDate
					 , PO.CreatedUserId
					 , PO.ModifiedUserId
					 , PO.ExecuteAfter
					 , PO.StartDate
					 , PO.EndDate
					 , CASE PO.[State]
					        WHEN 0 THEN @StateNameCreated
					        WHEN 1 THEN @StateNameError
					        WHEN 2 THEN @StateNameQueued
					        WHEN 3 THEN @StateNameIsExecuting
					        WHEN 4 THEN @StateNameFailed
					        WHEN 5 THEN @StateNameFinished
					        WHEN 6 THEN @StateNameResultsAvailable
					        WHEN 7 THEN @StateNameResultsExpunged
					   END AS StateName
					 , NULL AS SplitOn1
					 , POS.ProcessOrderServiceId
					 , POS.ResultTypeFullName
					 , POS.ResultFilePath
					 , NULL AS SplitOn2
					 , U.FullName
					 , U.Username
					 , U.Email
					 , NULL AS SplitOn3
					 , POA.ProcessOrderArgumentId
					 , POA.Name
					 , POA.Value
					 , POA.IsExposed
				  FROM ProcessOrder PO WITH (NOLOCK)
				       INNER JOIN ProcessOrderService POS WITH (NOLOCK)
					           ON POS.ProcessOrderId = PO.ProcessOrderId
					   INNER JOIN CWIUser U
       						   ON U.Id = PO.CreatedUserId
					    LEFT JOIN ProcessOrderArgument POA WITH (NOLOCK)
       						   ON POA.ProcessOrderId = PO.ProcessOrderId
							  AND POA.IsExposed = 1
							  AND POA.Name = 'model.Arquivos[0]'
				 WHERE (@State IS NULL OR PO.State = @State)
				   AND (@CreatedUserId IS NULL OR PO.CreatedUserId = @CreatedUserId)
				   AND (@IsAdministrator = 1 OR PO.CreatedUserId = @CurrentUserId)
				   AND (@ProcessName IS NULL OR PO.ProcessName = @ProcessName)
 			   ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum;