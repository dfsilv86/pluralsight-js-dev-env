/*
DECLARE	@idPerfil INT, @idAlcada INT;
SET @idPerfil = 8;
SET @idAlcada = NULL;
--*/

SELECT		 A.IDAlcada
			,R.Id AS IDPerfil
			,A.blAlterarSugestao
			,A.blAlterarInformacaoEstoque
			,A.blAlterarPercentual
			,A.vlPercentualAlterado
			,A.blZerarItem			
			,NULL AS SplitOn1
			,R.Name			
			,NULL AS SplitOn2
			,AD.idAlcada
			,AD.idAlcadaDetalhe
			,AD.idRegiaoAdministrativa
			,AD.idBandeira
			,AD.idDepartamento
			,AD.vlPercentualAlterado
			,NULL AS SplitOn3
			,RA.dsRegiaoAdministrativa
			,NULL AS SplitOn4
			,B.IDFormato
			,B.blAtivo
			,B.blImportarTodos
			,B.cdSistema
			,B.cdUsuarioAtualizacao
			,B.cdUsuarioCriacao
			,B.dhAtualizacao
			,B.dhCriacao
			,B.dsBandeira
			,B.sgBandeira
			,B.tpCusto
			,NULL AS SplitOn5
			,D.IDDivisao
			,D.blAtivo AS blAtivoD
			,D.blPerecivel
			,D.cdDepartamento
			,D.cdSistema AS cdSistemaD
			,D.cdUsuarioAtualizacao AS cdUsuarioAtualizacaoD
			,D.cdUsuarioCriacao AS cdUsuarioCriacaoD
			,D.dhAtualizacao AS dhAtualizacaoD
			,D.dhCriacao AS dhCriacaoD
			,D.dsDepartamento
			,D.pcDivergenciaNF
FROM		CWIRole R WITH (NOLOCK)
LEFT JOIN	Alcada A WITH (NOLOCK)
ON			A.IDPerfil = R.Id
LEFT JOIN	AlcadaDetalhe AD WITH (NOLOCK)
ON			AD.IdAlcada = A.IdAlcada
LEFT JOIN   RegiaoAdministrativa RA WITH (NOLOCK)
ON			RA.idRegiaoAdministrativa = AD.idRegiaoAdministrativa
LEFT JOIN   Bandeira B WITH (NOLOCK)
ON			B.IDBandeira = AD.idBandeira
LEFT JOIN   Departamento D WITH (NOLOCK)
ON			D.IDDepartamento = AD.idDepartamento
WHERE		(@idPerfil IS NOT NULL AND R.Id = @idPerfil) OR (@idAlcada IS NOT NULL AND A.IdAlcada = @idAlcada)
