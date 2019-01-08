/*
DECLARE @id INT
SET @id = 1
--*/

SELECT  CAST(1 AS BIT)
WHERE   EXISTS(
SELECT      1
FROM        InventarioItem II WITH (NOLOCK)
INNER JOIN  ItemDetalhe ID WITH (NOLOCK)
ON          II.IDItemDetalhe = ID.IDItemDetalhe
WHERE       II.IDInventario = @id
AND         ID.tpStatus IN ('D', 'I'))