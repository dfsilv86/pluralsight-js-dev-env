/*
DECLARE @idLoja INT
SET @idLoja = 2
--*/

SELECT		TOP 1
			FF.*
FROM		FechamentoFiscal FF
WHERE		FF.IDLoja = @idLoja
AND			FF.dhFechamentoFiscal IS NOT NULL
ORDER BY	FF.dhFechamentoFiscal DESC