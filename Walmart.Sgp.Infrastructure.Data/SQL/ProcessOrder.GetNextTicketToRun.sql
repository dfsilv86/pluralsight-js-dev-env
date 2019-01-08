/*
DECLARE @WorkerName VARCHAR(MAX), @ServiceTypeName VARCHAR(MAX), @ServiceMethodName VARCHAR(MAX), @CreatedUserId INT, @MachineName VARCHAR(50);
SET @WorkerName = 'abc';
SET @MachineName = 'CG-002260';
--*/
DECLARE @QueuedProcessOrderId INT;

WITH QueuedProcessOrders AS (
	SELECT TOP 100 PERCENT PO.[ProcessOrderId]
	     , PO.[Ticket]
		 , PO.[WorkerName]
		 , PO.[ExecuteAfter]
		 , PO.[State]
		 , PO.[CreatedUserId]
		 , PO.[CreatedMachineName]
		 , POS.[ServiceTypeName]
		 , POS.[ServiceMethodName]
		 , POS.[MaxGlobal]
		 , POS.[MaxPerUser]
      FROM ProcessOrder PO WITH (READPAST, ROWLOCK, UPDLOCK)
	       INNER JOIN ProcessOrderService POS WITH (READPAST, ROWLOCK, UPDLOCK)
		           ON POS.ProcessOrderId = PO.ProcessOrderId
	 WHERE PO.WorkerName IS NULL
	   AND (PO.ExecuteAfter IS NULL OR PO.ExecuteAfter < GETDATE())
	   AND (@MachineName IS NULL OR PO.CreatedMachineName IS NULL OR PO.CreatedMachineName = @MachineName)
	   AND PO.[State] = 2
	   AND (@CreatedUserId IS NULL OR PO.CreatedUserId = @CreatedUserId)
	   AND (@ServiceTypeName IS NULL OR POS.ServiceTypeName = @ServiceTypeName)
	   AND (@ServiceMethodName IS NULL OR POS.ServiceMethodName = @ServiceMethodName)
	 ORDER BY PO.CreatedDate ASC
), AllProcessOrders AS (
	SELECT PO.[State]
		 , PO.[CreatedUserId]
		 , POS.[ServiceTypeName]
		 , POS.[ServiceMethodName]
      FROM ProcessOrder PO WITH (NOLOCK)
	       INNER JOIN ProcessOrderService POS WITH (NOLOCK)
		           ON POS.ProcessOrderId = PO.ProcessOrderId
)
SELECT TOP 1 @QueuedProcessOrderId = PO2.ProcessOrderId
  FROM QueuedProcessOrders PO2 WITH (READPAST, ROWLOCK, UPDLOCK)
 WHERE (PO2.MaxGlobal  = 0 OR PO2.MaxGlobal  > (SELECT COUNT(1) FROM AllProcessOrders PO3 WITH (NOLOCK) WHERE PO3.[State] = 3 AND PO3.ServiceTypeName = PO2.ServiceTypeName AND PO3.ServiceMethodName = PO2.ServiceMethodName))
   AND (PO2.MaxPerUser = 0 OR PO2.MaxPerUser > (SELECT COUNT(1) FROM AllProcessOrders PO4 WITH (NOLOCK) WHERE PO4.CreatedUserId = PO2.CreatedUserId AND PO4.[State] = 3 AND PO4.ServiceTypeName = PO2.ServiceTypeName AND PO4.ServiceMethodName = PO2.ServiceMethodName))

UPDATE PO1
   SET WorkerName = @WorkerName
  FROM ProcessOrder PO1 WITH (READPAST, ROWLOCK, UPDLOCK)  -- importante que a transação não fique aberta por muito tempo com READPAST
 WHERE PO1.ProcessOrderId = @QueuedProcessOrderId
   AND PO1.WorkerName IS NULL
   AND PO1.[State] = 2;

SELECT TOP 1 Ticket FROM ProcessOrder WITH (READPAST, ROWLOCK, UPDLOCK) WHERE WorkerName = @WorkerName AND ProcessOrderId = @QueuedProcessOrderId AND [State] = 2;