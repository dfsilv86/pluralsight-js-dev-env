/*
DECLARE @inicioDataTermino date, @fimDataTermino date
SET @inicioDataTermino = GETDATE();
SET @fimDataTermino = GETDATE() + 1;
--*/

SELECT 	
	TOP 1 cdOrigemCalculo
FROM 
	SugestaoPedido
WHERE 
	dtPedido = @dtPedido
