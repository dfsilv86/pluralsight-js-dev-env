/*
DECLARE	@idPerfil INT
SET @idPerfil = 10
*/

SELECT		TOP 1
			 A.IDAlcada
			,R.Id AS IDPerfil
			,A.blAlterarSugestao
			,A.blAlterarInformacaoEstoque
			,A.blAlterarPercentual
			,A.vlPercentualAlterado
			,A.blZerarItem			
			,NULL AS SplitOn1
			,R.Name			
FROM		CWIRole R WITH (NOLOCK)
LEFT JOIN	Alcada A WITH (NOLOCK)
ON			A.IDPerfil = R.Id
WHERE		R.Id = @idPerfil

