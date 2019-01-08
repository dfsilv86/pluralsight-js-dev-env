/*
DECLARE	@Ticket VARCHAR(300)
SET @Ticket = 'ExportacaoCorreta 100%.xlsx.af539cd8-f7da-4f4b-906d-c6dc06872919|2016-08-15 17:40:17.000'
--*/

SELECT COUNT(1) AS HasFileVaultTicket
  FROM (
		SELECT TOP 1 1 AS HasValue
		  FROM ProcessOrderArgument WITH (NOLOCK)
		 WHERE Value = @Ticket
       ) X