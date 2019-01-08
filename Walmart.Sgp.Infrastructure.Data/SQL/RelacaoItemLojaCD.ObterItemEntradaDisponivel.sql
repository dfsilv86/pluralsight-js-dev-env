/*
DROP TABLE #Itens
DECLARE @cdItemSaida INT, @cdLoja INT, @idCD INT, @idFornecedorParametro INT;
SET @cdItemSaida = 9400427;
SET @cdLoja = 1006;
SET @idCD = 9;
SET @idFornecedorParametro = NULL;
--*/
DECLARE @blConvertido BIT;

SET @blConvertido = ISNULL((
			SELECT blConvertido
			FROM CD
			WHERE idCD = @idCD
			), 0);

SELECT IDItemDetalhe
	,vlTipoReabastecimento
	,CdCrossRef
	,Sequencial
	,tipo
INTO #Itens
FROM (
	SELECT ITS.IDItemDetalhe
		,dbo.fnObterTipoReabastecimento(ITS.IDItemDetalhe, @idCD, L.IDLoja) vlTipoReabastecimento
		,ITP.CdCrossRef
		,ITP.Sequencial
		,1 AS tipo
	FROM RelacionamentoItemPrincipal RIP
	JOIN RelacionamentoItemPrime ITP ON ITP.idItemDetalhe = RIP.IDItemDetalhe
		AND ITP.Sequencial = 1
	INNER JOIN CD CD ON CD.idCD = @idCD
		AND CD.blConvertido = 1
	JOIN ItemDetalhe ITS ON ITS.IDItemDetalhe = RIP.IDItemDetalhe
	JOIN Trait TRS WITH (NOLOCK) ON TRS.IDItemDetalhe = RIP.IDItemDetalhe
	JOIN Loja L WITH (NOLOCK) ON L.IDLoja = TRS.IDLoja
	JOIN Fornecedor F WITH (NOLOCK) ON F.IDFornecedor = ITS.IDFornecedor
	WHERE ITS.tpVinculado = 'S'
		AND ITS.cdItem = @cdItemSaida
		AND L.cdLoja = @cdLoja
		AND ITS.vlTipoReabastecimento IS NOT NULL
		AND ITS.blItemTransferencia = 0
		AND ITS.idFornecedorParametro IS NOT NULL
		AND (
			@idFornecedorParametro IS NULL
			OR ITS.idFornecedorParametro = @idFornecedorParametro
			)
		AND ITS.blAtivo = 1
		AND ITS.tpStatus = 'A'
		AND EXISTS (
			SELECT 1
			FROM RelacionamentoItemPrime RIP
			WHERE RIP.cdCrossRef = ITP.cdCrossRef
				AND RIP.Sequencial > 1
				AND dbo.fnObterTipoReabastecimento(RIP.IdItemDetalhe, @idCD, L.idLoja) IN (
					20
					,22
					,40
					,42
					,43
					,81
					)
			)
		AND dbo.fnObterTipoReabastecimento(ITS.IDItemDetalhe, @idCD, L.IDLoja) IN (
			20
			,22
			,40
			,42
			,43
			,81
			)
	
	UNION
	
	SELECT ITE.IDItemDetalhe
		,dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, @idCD, L.IDLoja) vlTipoReabastecimento
		,ITP.CdCrossRef
		,ITP.Sequencial
		,2 AS tipo
	FROM RelacionamentoItemPrincipal RIP
	JOIN RelacionamentoItemSecundario RIS ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
	LEFT JOIN RelacionamentoItemPrime ITP ON ITP.idItemDetalhe = RIS.IDItemDetalhe
	JOIN ItemDetalhe ITS ON ITS.IDItemDetalhe = RIP.IDItemDetalhe
	JOIN Trait TRS WITH (NOLOCK) ON TRS.IDItemDetalhe = RIP.IDItemDetalhe
	JOIN Trait TRE WITH (NOLOCK) ON TRE.IDItemDetalhe = RIS.IDItemDetalhe
		AND TRE.IdLoja = TRS.IdLoja
	JOIN Loja L WITH (NOLOCK) ON L.IDLoja = TRS.IDLoja
	JOIN ItemDetalhe ITE ON ITE.IDItemDetalhe = RIS.IDItemDetalhe
	JOIN Fornecedor F WITH (NOLOCK) ON F.IDFornecedor = ITE.IDFornecedor
	LEFT JOIN CompraCasada CC WITH (NOLOCK) ON CC.idItemDetalheEntrada = RIS.IDItemDetalhe
		AND CC.blAtivo = 1
	WHERE ITE.tpVinculado = 'E'
		AND ITS.cdItem = @cdItemSaida
		AND L.cdLoja = @cdLoja
		AND ITE.vlTipoReabastecimento IS NOT NULL
		AND ITE.blItemTransferencia = 0
		AND ITE.idFornecedorParametro IS NOT NULL
		AND (
			@idFornecedorParametro IS NULL
			OR ITE.idFornecedorParametro = @idFornecedorParametro
			)
		AND ITE.blAtivo = 1
		AND ITE.tpStatus = 'A'
		AND (
			CC.blItemPai IS NULL
			OR CC.blItemPai != 0
			)
		AND dbo.fnItemAtendeRegrasVendorPrimario(CASE 
				WHEN ITP.Sequencial IS NOT NULL
					THEN 1
				ELSE 0
				END, --@blPossuiXRef
			CASE 
				WHEN dbo.fnObterTipoReabastecimento(ITP.IdItemDetalhe, @idCD, L.IdLoja) IN (
						20
						,22
						,40
						,42
						,43
						,81
						)
					THEN 1
				ELSE 0
				END, --@blStaple
			CASE 
				WHEN ITP.Sequencial = 1
					THEN 1
				ELSE 0
				END, --@blPrime 
			CASE 
				WHEN ITP.Sequencial IS NOT NULL
					AND ISNULL((
							SELECT TOP 1 1
							FROM RelacionamentoItemPrime RIP2
							WHERE RIP2.cdCrossRef = ITP.cdCrossRef
								AND RIP2.idItemDetalhe <> ITP.idItemDetalhe
								AND RIP2.Sequencial > 1
								AND dbo.fnObterTipoReabastecimento(RIP2.idItemDetalhe, @idCD, L.IdLoja) IN (
									20
									,22
									,40
									,42
									,43
									,81
									)
							), 0) = 1
					THEN 1
				ELSE 0
				END, --@blPossuiSecundariosPrime
			@blConvertido) = 1 --@blCDConvertido
	) Itens

--select * from #itens
UPDATE i
SET IDItemDetalhe = (
		SELECT R.IDItemDetalhe
		FROM RelacionamentoItemPrime R
		WHERE R.CdCrossRef = i.CdCrossRef
			AND R.Sequencial = 1
		)
FROM #Itens i
WHERE Sequencial > 1
	AND i.vlTipoReabastecimento IN (
		20
		,22
		,40
		,42
		,43
		,81
		)
	AND @blConvertido = 1

--select * from #itens
SELECT DISTINCT IDE.IDItemDetalhe
	,IDE.cdItem
	,IDE.dsItem
	,IDE.IDFornecedor
	,CASE 
		WHEN i.CdCrossRef IS NULL
			THEN i.vlTipoReabastecimento
		ELSE IDE.vlTipoReabastecimento
		END vlTipoReabastecimento
	,CASE 
		WHEN i.vlTipoReabastecimento NOT IN (
				20
				,22
				,40
				,42
				,43
				,81
				)
			THEN NULL
		ELSE i.cdCrossRef
		END AS cdCrossRef
	,NULL AS SplitOn1
	,F.IDFornecedor
	,F.cdFornecedor
	,F.nmFornecedor
	,NULL AS SplitOn2
	,FP.cdV9D
FROM #Itens i
INNER JOIN ItemDetalhe IDE WITH (NOLOCK) ON IDE.IDItemDetalhe = i.IDItemDetalhe
INNER JOIN Fornecedor F WITH (NOLOCK) ON F.IDFornecedor = IDE.IDFornecedor
LEFT JOIN FornecedorParametro FP WITH (NOLOCK) ON FP.IDFornecedorParametro = IDE.IDFornecedorParametro
WHERE (
		FP.IDFornecedorParametro IS NULL
		OR (
			FP.IDFornecedorParametro IS NOT NULL
			AND NOT (
				FP.cdStatusVendor <> 'A'
				OR FP.blAtivo = 0
				)
			)
		)
	AND (
		F.IDFornecedor IS NULL
		OR (
			F.IDFornecedor IS NOT NULL
			AND NOT (
				F.blAtivo = 0
				OR F.stFornecedor = 'I'
				OR F.stFornecedor = 'D'
				)
			)
		)
