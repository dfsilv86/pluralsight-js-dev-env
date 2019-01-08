/*
DECLARE @agendamentoStatusCancelado INT, @inventarioStatusCancelado INT, @idLoja INT, @idDepartamento INT, @dtAgendamento DATETIME

SET @agendamentoStatusCancelado = 2;
SET @inventarioStatusCancelado = 5;
SET @idLoja = 87;
SET @idDepartamento = 93;
SET @dtAgendamento = '2015-03-16';
*/

SELECT 
	COUNT(1) 
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
	    MONTH(IA.dtAgendamento) = MONTH(@dtAgendamento)
    AND YEAR(IA.dtAgendamento) = YEAR(@dtAgendamento)
	AND IA.stAgendamento <> @agendamentoStatusCancelado
    AND I.stInventario <> @inventarioStatusCancelado   
    AND L.idLoja = @idLoja
    AND D.idDepartamento = @idDepartamento
