/*
DECLARE @idItemDetalhe AS INT, @idLoja AS INT
     
SET @idItemDetalhe = 1
SET @idLoja = 1
*/

SELECT 
	TOP 1 
	NI.IDNotaFiscalItem,
	NI.IdNotaFiscalItemStatus
FROM 
	NotaFiscal NF WITH (NOLOCK)
		INNER JOIN NotaFiscalItem NI WITH (NOLOCK)
			ON NF.IDNotaFiscal = NI.IDNotaFiscal		
 WHERE 
	NF.IDLoja = @idLoja
AND NI.IDItemDetalhe = @idItemDetalhe
ORDER BY
	NF.dtRecebimento DESC	