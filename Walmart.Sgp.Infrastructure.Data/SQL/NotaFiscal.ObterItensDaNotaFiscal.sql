/*
DECLARE @idNotaFiscal AS INT
     
SET @idNotaFiscal = 14509039
*/

SELECT 
	NFI.qtItem,
	NFI.vlCusto,
	NULL AS SplitOn1,
	NFIS.DsNotaFiscalItemStatus,
	NULL AS SplitOn2,
	ID.IDItemDetalhe,
	ID.cdItem,
	ID.dsItem	
  FROM 
	   NotaFiscalItem NFI WITH(NOLOCK) 
		INNER JOIN NotaFiscalItemStatus NFIS WITH(NOLOCK) 
			ON NFI.IdNotaFiscalItemStatus = NFIS.IdNotaFiscalItemStatus
		INNER JOIN ItemDetalhe ID WITH(NOLOCK) 
			ON NFI.IDItemDetalhe = ID.IDItemDetalhe  
		INNER JOIN Departamento D WITH(NOLOCK) 
			ON ID.IDDepartamento = D.IDDepartamento
 WHERE 
	 NFI.IDNotaFiscal = @idNotaFiscal
 AND D.blPerecivel = 'S'