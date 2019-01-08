/*
DECLARE @agendamentoStatusCancelado INT, @inventarioStatusCancelado INT, @idBandeira INT, @cdLoja INT, @cdDepartamento INT, @dtAgendamento DATETIME

SET @agendamentoStatusCancelado = 2;
SET @inventarioStatusCancelado = 5;
SET @idBandeira = 1;
SET @cdLoja = 87;
SET @cdDepartamento = 93;
SET @dtAgendamento = '2015-03-16';
*/

SELECT 
	IA.IDInventarioAgendamento,    
	IA.dtAgendamento,
	IA.stAgendamento,
	NULL AS SplitOn1,
    B.IDBandeira,
	B.cdSistema,
    B.dsBandeira,
	NULL AS SplitOn2,
    L.cdLoja,
    L.nmLoja,    
	NULL AS SplitOn3,
    D.cdDepartamento,
    D.dsDepartamento,
	NULL AS SplitOn4,    
	I.IDInventario,
    I.stInventario    
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
	IA.stAgendamento <> @agendamentoStatusCancelado
    AND I.stInventario <> @inventarioStatusCancelado
    AND B.IDBandeira = @idBandeira
    AND L.cdLoja = ISNULL(@cdLoja, L.cdLoja)    
    AND D.cdDepartamento = ISNULL(@cdDepartamento, D.cdDepartamento)    
    AND IA.dtAgendamento = ISNULL(@dtAgendamento, IA.dtAgendamento)    
ORDER BY 
	IA.dtAgendamento