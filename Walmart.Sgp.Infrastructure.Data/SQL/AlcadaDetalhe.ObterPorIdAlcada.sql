/*
DECLARE	@idAlcada INT
SET @idAlcada = 3
*/

SELECT		 AD.idAlcada
			,AD.idAlcadaDetalhe
			,AD.idRegiaoAdministrativa
			,AD.idBandeira
			,AD.idDepartamento
			,AD.vlPercentualAlterado
			,NULL AS SplitOn1
			,RA.dsRegiaoAdministrativa
			,NULL AS SplitOn2
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
			,NULL AS SplitOn3
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
FROM		AlcadaDetalhe AD WITH (NOLOCK)
LEFT JOIN   RegiaoAdministrativa RA WITH (NOLOCK)
ON			RA.idRegiaoAdministrativa = AD.idRegiaoAdministrativa
LEFT JOIN   Bandeira B WITH (NOLOCK)
ON			B.IDBandeira = AD.idBandeira
LEFT JOIN   Departamento D WITH (NOLOCK)
ON			D.IDDepartamento = AD.idDepartamento
WHERE		AD.IdAlcada = @idAlcada