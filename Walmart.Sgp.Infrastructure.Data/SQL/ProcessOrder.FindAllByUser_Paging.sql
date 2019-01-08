/*
DECLARE	@IsAdministrator BIT, @CurrentUserId INT, @CreatedUserId INT, @processName VARCHAR(150), @state INT;
SET @CurrentUserId = 2;
SET @IsAdministrator = 1;
--SET @CreatedUserId = 2526316;

DECLARE @StateNameCreated VARCHAR, @StateNameQueued VARCHAR, @StateNameError VARCHAR, @StateNameIsExecuting VARCHAR, @StateNameFailed VARCHAR, @StateNameFinished VARCHAR, @StateNameResultsAvailable VARCHAR, @StateNameResultsExpunged VARCHAR;
SET @StateNameCreated = 'C';
SET @StateNameQueued = 'A';
SET @StateNameError = 'ER';
SET @StateNameIsExecuting = 'EM';
SET @StateNameFailed = 'Finalizado com erro';
SET @StateNameFinished = 'Finalizado com sucesso';
SET @StateNameResultsAvailable = 'Resultado disponível';
SET @StateNameResultsExpunged = 'Resultado expurgado';
--*/

SELECT RowConstrainedResult.*
	 , NULL AS SplitOn3
	 , POA.ProcessOrderArgumentId
	 , POA.Name
	 , POA.Value
	 , POA.IsExposed
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
				SELECT PO.ProcessOrderId
				     , PO.Ticket
					 , PO.ProcessName
					 , PO.[State]
					 , PO.CurrentProgress
					 , PO.TotalProgress
					 , PO.[Message]
					 , PO.CreatedDate
					 , PO.ModifiedDate
					 , PO.CreatedUserId
					 , PO.ModifiedUserId
					 , PO.ExecuteAfter
					 , PO.StartDate
					 , PO.EndDate
					 , PO.WorkerName
					 , PO.CreatedMachineName
					 , CASE PO.[State]
					        WHEN 0 THEN @StateNameCreated
					        WHEN 1 THEN @StateNameQueued
					        WHEN 2 THEN @StateNameError
					        WHEN 3 THEN @StateNameIsExecuting
					        WHEN 4 THEN @StateNameFailed
					        WHEN 5 THEN @StateNameFinished
					        WHEN 6 THEN @StateNameResultsAvailable
					        WHEN 7 THEN @StateNameResultsExpunged
					   END AS StateName
					 , NULL AS SplitOn1
					 , POS.ProcessOrderServiceId
					 --, POS.ServiceTypeName
					 --, POS.ServiceMethodName
					 , POS.ResultTypeFullName
					 , POS.ResultFilePath
					 --, POS.MaxGlobal
					 --, POS.MaxPerUser
					 --, POS.RoleId
					 --, POS.StoreId
					 , NULL AS SplitOn2
					 , U.FullName
					 , U.Username
					 , U.Email
					 --, P.blAdministrador AS IsAdministrator
				  FROM ProcessOrder PO WITH (NOLOCK)
				       INNER JOIN ProcessOrderService POS WITH (NOLOCK)
					           ON POS.ProcessOrderId = PO.ProcessOrderId
					   INNER JOIN CWIUser U
       						   ON U.Id = PO.CreatedUserId
					    --LEFT JOIN Permissao P
       					--	   ON P.IDUsuario = U.Id
				 WHERE (@State IS NULL OR PO.State = @State)
				   AND (@CreatedUserId IS NULL OR PO.CreatedUserId = @CreatedUserId)
				   AND (@IsAdministrator = 1 OR PO.CreatedUserId = @CurrentUserId)
				   AND (@ProcessName IS NULL OR PO.ProcessName = @ProcessName)
 			   ) __INTERNAL
       ) AS RowConstrainedResult        
       LEFT JOIN ProcessOrderArgument POA WITH (NOLOCK)
       		ON POA.ProcessOrderId = RowConstrainedResult.ProcessOrderId
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum;