/* 
DECLARE @idItemDetalhe INT, @idLoja INT
SET @idItemDetalhe = 411238;
SET @idLoja = 253;
*/

SELECT 
	DISTINCT dtMovimentado 
FROM dbo.Movimentacao (NOLOCK)
	WHERE idItem IN
	(
		SELECT 
			IdItemDetalhe
		FROM 
			dbo.ItemDetalhe IDT (NOLOCK)
		WHERE 
			IDItemDetalhe = @idItemDetalhe
	
		UNION ALL
		SELECT 
			RIP.IDItemDetalhe
		FROM 
			dbo.RelacionamentoItemPrincipal RIP (NOLOCK)
				INNER JOIN dbo.RelacionamentoItemSecundario RIS (NOLOCK)
					ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal	
		WHERE 
			RIS.IDItemDetalhe = @idItemDetalhe

		UNION ALL
		SELECT 
			RIS.IDItemDetalhe
		FROM 
			dbo.RelacionamentoItemPrincipal RIP (NOLOCK)
				INNER JOIN dbo.RelacionamentoItemSecundario RIS (NOLOCK)
					ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
		WHERE 
			RIP.IDItemDetalhe = @idItemDetalhe
	)
	AND IDLoja = @idLoja
	AND IDTipoMovimentacao IN (11, 12, 13)
	AND dtMovimentado >=
		(
			SELECT 
				TOP 1 DATEADD(M, 1, CAST(CAST(nrAno as VARCHAR) + '-' + CAST(nrMes as VARCHAR) + '-01' as DATE))
			FROM 
				dbo.FechamentoFiscal (NOLOCK)
			WHERE 
				IDLoja = @idLoja AND dhContabilizacao IS NOT NULL
			ORDER BY 
				dhFechamentoFiscal desc
		)
	ORDER BY 
		dtMovimentado DESC