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
    WHERE       INV.IDInventario = @id
    AND         EXISTS(
                SELECT  1
                FROM    Trait _T WITH (NOLOCK)
                WHERE   _T.IDItemDetalhe = II.IDItemDetalhe
                AND     _T.IDLoja = INV.IDLoja))