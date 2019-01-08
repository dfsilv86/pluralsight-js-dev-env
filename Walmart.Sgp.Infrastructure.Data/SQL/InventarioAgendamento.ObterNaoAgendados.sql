/*
DECLARE @agendamentoStatusCancelado INT, @inventarioStatusCancelado INT, @idBandeira INT, @cdLoja INT, @cdDepartamento INT

SET @agendamentoStatusCancelado = 2;
SET @inventarioStatusCancelado = 5;
SET @idBandeira = 1;
--SET @cdLoja = 87;
--SET @cdDepartamento = 93;
*/

WITH lojasDeptosAgendados AS (	
SELECT 
    L.IDLoja,
    D.IDDepartamento  
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
	    MONTH(IA.dtAgendamento) = MONTH(GETDATE())
    AND YEAR(IA.dtAgendamento) = YEAR(GETDATE())
	AND IA.stAgendamento <> @agendamentoStatusCancelado
    AND I.stInventario <> @inventarioStatusCancelado   
    AND B.IDBandeira = @idBandeira
    AND L.cdLoja = ISNULL(@cdLoja, L.cdLoja)    
    AND D.cdDepartamento = ISNULL(@cdDepartamento, D.cdDepartamento)
)

SELECT 
	B.IDBandeira,
	B.cdSistema,
	B.dsBandeira,
	NULL SplitOn1,	
	L.cdLoja,
	L.nmLoja,
	NULL SplitOn2,	
	D.cdDepartamento,
	D.dsDepartamento
FROM
	Loja L 
		INNER JOIN Bandeira B WITH (NOLOCK)
			ON L.IDBandeira = B.IDBandeira,
	Departamento D
WHERE
	D.blPerecivel = 'S'
AND D.cdSistema = l.cdSistema
AND D.cdDepartamento = ISNULL(@cdDepartamento, D.cdDepartamento)
AND L.IDBandeira = @idBandeira
AND L.blCarregaSGP = 1
AND L.cdLoja = ISNULL(@cdLoja, L.cdLoja)        
AND	NOT EXISTS (
		SELECT 
			1 
		FROM 
			lojasDeptosAgendados LDA 
		WHERE 
			LDA.IDLoja = L.IDLoja 
		AND LDA.IDDepartamento = D.IDDepartamento
)
	