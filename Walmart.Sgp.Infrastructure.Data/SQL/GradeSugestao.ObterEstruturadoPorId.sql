/*
DECLARE @id INT = NULL;
set @id = 1
*/

SELECT		TOP 200 
			GS.IDGradeSugestao
			,GS.vlHoraInicial
			,GS.vlHoraFinal
			,NULL AS SplitOn1
			,GS.IDBandeira
			,BD.dsBandeira
			,NULL AS SplitOn2
			,GS.IDDepartamento
			,DP.dsDepartamento
			,DP.cdDepartamento
			,NULL AS SplitOn3
			,LJ.nmLoja
			,GS.IDLoja
			,LJ.cdLoja
			,NULL AS SplitOn4
			,GS.cdSistema		
			,CDV.dsText AS [Text]
FROM		GradeSugestao GS WITH(NOLOCK)
INNER JOIN	Bandeira BD WITH(NOLOCK)
ON			BD.IDBandeira = GS.IDBandeira
LEFT JOIN	Departamento DP WITH(NOLOCK)
ON			DP.IDDepartamento = GS.IDDepartamento
LEFT JOIN	Loja LJ WITH(NOLOCK)
ON			LJ.IDLoja = GS.IDLoja
INNER JOIN	CWIDomainValue CDV WITH(NOLOCK)
ON			CDV.IDDomain = 3
AND			CDV.dsValue = GS.cdSistema
WHERE		GS.IDGradeSugestao = @id