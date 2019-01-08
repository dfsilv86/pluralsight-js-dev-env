/* 
DECLARE @idRelacionamentoItemPrincipalCorrente INT, @idItemDetalhe INT, @tipoRelacionamento INT;
SET @idRelacionamentoItemPrincipalCorrente = 43623
SET @idItemDetalhe = 4773
SET @tipoRelacionamento = 2
--*/

-- Legado: RelacionamentoItemPrincipaData.PermiteAlterarTipoItemEntrada
SELECT 
	COUNT(1)
FROM 
	RelacionamentoItemPrincipal P WITH (NOLOCK)
		INNER JOIN RelacionamentoItemSecundario S WITH (NOLOCK)
			ON P.IDRelacionamentoItemPrincipal = S.IDRelacionamentoItemPrincipal
WHERE 
	(P.IDItemDetalhe = @idItemDetalhe OR S.IDItemDetalhe = @idItemDetalhe)
	AND P.IDTipoRelacionamento = ISNULL(@tipoRelacionamento, P.IDTipoRelacionamento)
	AND P.IDRelacionamentoItemPrincipal <> @idRelacionamentoItemPrincipalCorrente 