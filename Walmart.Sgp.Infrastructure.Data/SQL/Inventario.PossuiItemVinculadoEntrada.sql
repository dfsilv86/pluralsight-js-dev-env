/*
DECLARE @id INT
SET @id = 1
--*/

SELECT  CAST(1 AS BIT)
WHERE   EXISTS(
        SELECT      1
        FROM        Inventario INV WITH (NOLOCK)
        INNER JOIN  InventarioItem II WITH (NOLOCK)
        ON          II.IDInventario = INV.IDInventario
        INNER JOIN  ItemDetalhe ITM WITH (NOLOCK)
        ON          ITM.IDItemDetalhe = II.IDItemDetalhe
        WHERE       INV.IDInventario = @id
        AND         ITM.tpVinculado = 'E')