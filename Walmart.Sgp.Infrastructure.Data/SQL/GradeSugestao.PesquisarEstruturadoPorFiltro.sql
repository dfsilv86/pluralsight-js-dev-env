/*
DECLARE @cdSistema INT = 1;
DECLARE @idBandeira INT = NULL;
DECLARE @cdDepartamento INT = NULL;
DECLARE @cdLoja INT = NULL;
*/

SELECT		
	GS.IDGradeSugestao
	,GS.vlHoraInicial
	,GS.vlHoraFinal
	,NULL AS SplitOn1
	,GS.IDBandeira
	,BD.dsBandeira
	,NULL AS SplitOn2
	,GS.IDDepartamento
	,DP.dsDepartamento
	,NULL AS SplitOn3
	,LJ.nmLoja
	,GS.IDLoja
	,NULL AS SplitOn4
	,GS.cdSistema		
	,CDV.dsText AS [Text]
	,1 AS SplitOn5	
	,UC.FullName usuarioCriacaoFullname
	,GS.dhCriacao
	,ISNULL(UA.FullName, UC.FullName) usuarioAtualizacaoFullname
	,ISNULL(GS.dhAtualizacao, GS.dhCriacao) as dhAtualizacao
FROM		
	GradeSugestao GS WITH (NOLOCK)
		INNER JOIN	Bandeira BD WITH (NOLOCK)
			ON BD.IDBandeira = GS.IDBandeira
		LEFT JOIN Departamento DP WITH (NOLOCK)
			ON DP.IDDepartamento = GS.IDDepartamento
		LEFT JOIN Loja LJ WITH (NOLOCK)
			ON LJ.IDLoja = GS.IDLoja
		INNER JOIN CWIDomainValue CDV WITH (NOLOCK)
			ON CDV.IDDomain = 3	AND	CDV.dsValue = GS.cdSistema
		LEFT JOIN CWIUser UC WITH (NOLOCK)
			ON UC.Id = GS.cdUsuarioCriacao
		LEFT JOIN CWIUser UA WITH (NOLOCK)
			ON UA.Id = GS.cdUsuarioAtualizacao
WHERE		
	GS.cdSistema = @cdSistema
AND	(@idBandeira IS NULL OR GS.IDBandeira = @idBandeira)
AND	(@cdDepartamento IS NULL OR DP.cdDepartamento = @cdDepartamento)
AND	(@cdLoja IS NULL OR	LJ.cdLoja = @cdLoja)