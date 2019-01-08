/*DECLARE @idLoja INT;
DECLARE @idDepartamento INT;
DECLARE @dhUltimoFechamentoFiscalLoja DATETIME;*/

SELECT		TOP 1
			I.*
FROM		Inventario I
WHERE		I.IDLoja = @idLoja
AND			I.IDDepartamento = @idDepartamento
AND			I.stInventario IN (1, 2, 3)
AND			I.dhInventario >= @dhUltimoFechamentoFiscalLoja
ORDER BY	I.dhInventario DESC