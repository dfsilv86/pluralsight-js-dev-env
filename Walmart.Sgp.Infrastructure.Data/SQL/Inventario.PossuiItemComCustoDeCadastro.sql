/*
DECLARE @id INT
SET @id = 1
--*/

WITH CteCusto AS (
SELECT e.IDLoja, e.IDItemDetalhe, MAX(dtRecebimento) as dtRecebimento
FROM Inventario i (nolock)
JOIN InventarioItem ii (nolock) ON i.IDInventario = ii.IDInventario
JOIN Estoque e (nolock) ON e.IDLoja = i.IDLoja AND e.IDItemDetalhe = ii.IDItemDetalhe
WHERE i.IDInventario = @id
AND e.dtRecebimento <= i.dhInventario
GROUP BY e.IDLoja, e.IDItemDetalhe)

SELECT TOP 1 
        CAST(1 AS BIT)
FROM CteCusto c
JOIN Estoque e (nolock) on e.IDLoja = c.IDLoja AND e.IDItemDetalhe = c.IDItemDetalhe AND e.dtRecebimento = c.dtRecebimento
WHERE e.vlCustoCadastroAtual IS NOT NULL AND e.vlCustoCadastroAtual <> 0