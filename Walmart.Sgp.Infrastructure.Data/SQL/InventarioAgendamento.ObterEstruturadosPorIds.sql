/*
DECLARE @ids INT
SET @ids = 1;
--*/

SELECT 
	IA.*,
	NULL AS SplitOn1,
	I.IDInventario,
	I.stInventario,
	I.dhInventario, 
	I.IDLoja,
	I.IDDepartamento,
	I.IDBandeira,
	NULL AS SplitOn2,
	B.IDBandeira,
	B.cdSistema,
    B.dsBandeira,
	NULL AS SplitOn3,
    L.cdLoja,
    L.nmLoja,    
	NULL AS SplitOn4,
    D.cdDepartamento,
    D.dsDepartamento
FROM
	InventarioAgendamento IA 
		INNER JOIN Inventario I WITH (NOLOCK)
			ON IA.IDInventario = I.IDInventario
		INNER JOIN Loja L WITH (NOLOCK)
			ON I.IDLoja = L.IDLoja	
		INNER JOIN Bandeira B WITH (NOLOCK)
			ON L.IDBandeira = B.IDBandeira	
		INNER JOIN Departamento D WITH (NOLOCK)
			ON I.IDDepartamento = D.IDDepartamento
WHERE
	IDInventarioAgendamento IN @ids	