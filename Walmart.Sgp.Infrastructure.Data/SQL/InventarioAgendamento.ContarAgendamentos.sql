/*
DECLARE @idLoja INT, @idDepartamento INT, @inicioDtAgendamento DATETIME, @fimDtAgendamento DATETIME;
SET @idLoja = 262;
SET @idDepartamento = 5;
SET @inicioDtAgendamento = '2016-04-23';
SET @fimDtAgendamento = '2016-04-25';
*/

SELECT 
	COUNT(1) 
FROM
	InventarioAgendamento IA 
		INNER JOIN Inventario I WITH (NOLOCK)
			ON IA.IDInventario = I.IDInventario
WHERE
		I.IDLoja = @idLoja
    AND I.IDDepartamento = @idDepartamento
	AND IA.dtAgendamento >= @inicioDtAgendamento AND IA.dtAgendamento < @fimDtAgendamento
	AND IA.stAgendamento <> @statusAgendamentoCancelado
	AND I.stInventario IN @stInventarios
