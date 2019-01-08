/*
DECLARE	@Ticket VARCHAR(100)
SET @Ticket = '9ebd0a6d-feab-4efc-8e23-bf334137bedc'
--*/

SELECT PO.*
     , NULL AS SplitOn1
	 , POS.*
     , NULL AS SplitOn2
	 , U.FullName
	 , U.UserName
	 , U.Email
	 --, P.blAdministrador AS IsAdministrator
	 , NULL AS SplitOn3
	 , POA.*
  FROM ProcessOrder PO WITH (NOLOCK)
       INNER JOIN ProcessOrderService POS WITH (NOLOCK)
	           ON POS.ProcessOrderId = PO.ProcessOrderId
       INNER JOIN CWIUser U WITH (NOLOCK)
	           ON U.Id = PO.CreatedUserId
	    --LEFT JOIN Permissao P WITH (NOLOCK)
		--       ON P.IDUsuario = U.Id
        LEFT JOIN ProcessOrderArgument POA WITH (NOLOCK)
	           ON POA.ProcessOrderId = PO.ProcessOrderId
 WHERE PO.Ticket = @Ticket;
