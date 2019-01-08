/*
DECLARE @id INT
SET @id = 1
--*/

SELECT		II.*
			,NULL AS SplitOn1
			,ITD.*
FROM		InventarioItem II WITH (NOLOCK)
INNER JOIN	ItemDetalhe ITD WITH (NOLOCK)
ON			II.IDItemDetalhe = ITD.IDItemDetalhe
WHERE		II.IDInventarioItem = @id