/* 
DECLARE @idRelacionamentoItemPrincipalCorrente INT, @idItemDetalhe INT;
SET @idRelacionamentoItemPrincipalCorrente = 1
SET @idItemDetalhe = 16
--*/

-- Legado: RelacionamentoItemPrincipaData.ValidarItemDetalheSaida
SELECT 
	COUNT(1) 
FROM ( 
	SELECT 
		IDItemDetalhe 
	FROM 
		RelacionamentoItemPrincipal WITH (NOLOCK)
	WHERE 
		IDTipoRelacionamento IN (1, 2) AND -- Vinculado e Receituario.
		IDRelacionamentoItemPrincipal <> @idRelacionamentoItemPrincipalCorrente AND 
		IDItemDetalhe = @idItemDetalhe
	UNION
	SELECT 
		 P.IDItemDetalhe 
	FROM 
		RelacionamentoItemPrincipal P WITH (NOLOCK)
			INNER JOIN RelacionamentoItemSecundario S WITH (NOLOCK)
				ON P.IDRelacionamentoItemPrincipal = S.IDRelacionamentoItemPrincipal
			WHERE 
				P.IDTipoRelacionamento = 3 AND -- Manipulado.
				P.IDRelacionamentoItemPrincipal <> @idRelacionamentoItemPrincipalCorrente AND 
				S.IDItemDetalhe = @idItemDetalhe AND
                S.IDRelacionamentoItemPrincipal <> P.IDRelacionamentoItemPrincipal
) AS INTERNAL