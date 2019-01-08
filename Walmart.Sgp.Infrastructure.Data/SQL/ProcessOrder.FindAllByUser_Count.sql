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

SELECT COUNT(1)
  FROM ProcessOrder PO WITH (NOLOCK)
 WHERE (@State IS NULL OR PO.State = @State)
   AND (@CreatedUserId IS NULL OR PO.CreatedUserId = @CreatedUserId)
   AND (@IsAdministrator = 1 OR PO.CreatedUserId = @CurrentUserId)
   AND (@ProcessName IS NULL OR PO.ProcessName = @ProcessName)
