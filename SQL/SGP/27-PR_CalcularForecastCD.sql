SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[PR_CalcularForecastCD]
AS
BEGIN
	SET NOCOUNT ON

	IF (OBJECT_ID('TEMPDB..#DiasForecast') IS NOT NULL)
	BEGIN
		DROP TABLE #DiasForecast
	END

	CREATE TABLE #DiasForecast (
		IdForecastDiarioCd INT PRIMARY KEY CLUSTERED
		,CdSemanaWalmartForecast INT
		,Semana INT
		,DiaMes INT
		)

	TRUNCATE TABLE WLMSLP_STAGE..ForecastDiarioCd

	TRUNCATE TABLE WLMSLP_STAGE..ForecastSugestaoCd

	/* Carrega tabela com dados de forecast semanal/diario */
	BEGIN
		INSERT WLMSLP_STAGE..ForecastDiarioCd (
			IdLoja
			,IdCd
			,IdItemDetalhe
			,CdSemanaWalmartForecast
			,DiaSemana
			,vlForecastSemana
			,Alloc
			,DtInicioForecast
			)
		SELECT IdLoja
			,idCD
			,IDItemDetalhe
			,CdSemanaWalmartForecast
			,DiaSemana
			,VlForecastSemana
			,DsPercentualSemana
			,DtInicioForecast
		FROM (
			SELECT L.IdLoja
				,SP.idCD
				,SP.idItemDetalheSugestao AS IdItemDetalhe
				,CdSemanaWalmartForecast
				,VlForecastSemana
				,DsPercentualSemana
				,F.DtInicioForecast
			FROM WLMSLP_STAGE..Forecast F
			INNER JOIN Loja l
				ON l.cdLoja = F.cdLoja
					AND l.cdSistema = F.cdFormato
			INNER JOIN (
				SELECT idCD
					,cdCd
					,idItemDetalheSugestao
					,cdItemSaida
				FROM WLMSLP_STAGE..SugestaoPedidoCD
				GROUP BY idCD
					,cdCd
					,idItemDetalheSugestao
					,cdItemSaida
				) SP
				ON SP.cdCd = F.cdCd
					AND Sp.cdItemSaida = F.cdItem
			
			UNION
			
			SELECT L.IdLoja
				,SP.idCD
				,SP.iditemdetalhep AS IdItemDetalhe
				,CdSemanaWalmartForecast
				,VlForecastSemana
				,DsPercentualSemana
				,F.DtInicioForecast
			FROM WLMSLP_STAGE..Forecast F
			INNER JOIN Loja l
				ON l.cdLoja = F.cdLoja
					AND l.cdSistema = F.cdFormato
			INNER JOIN WLMSLP_STAGE..SugestaoPedidoCdItemRelacionado SP
				ON SP.cdCd = F.cdCd
					AND Sp.cdItemp = F.cdItem
			) AS R
		CROSS APPLY (
			SELECT DiaSemana
			FROM (
				VALUES (1)
					,(2)
					,(3)
					,(4)
					,(5)
					,(6)
					,(7)
				) AS X(DiaSemana)
			) AS X
	END

	DECLARE @MinCdSemanaWalmartForecast INT

	SET @MinCdSemanaWalmartForecast = (
			SELECT TOP 1 cdSemanaWalmartForecast
			FROM WLMSLP_STAGE..ForecastDiarioCD
			ORDER BY CdSemanaWalmartForecast
			)

	DELETE WLMSLP_STAGE..ForecastDiarioCd
	WHERE CdSemanaWalmartForecast = @MinCdSemanaWalmartForecast
		AND DiaSemana <= DATEPART(WEEKDAY, GETDATE())

	INSERT #DiasForecast (
		IdForecastDiarioCd
		,CdSemanaWalmartForecast
		,Semana
		,DiaMes
		)
	SELECT IdForecastDiarioCd
		,cdSemanaWalmartForecast
		,DENSE_RANK() OVER (
			ORDER BY cdSemanaWalmartForecast ASC
			) AS Semana
		,ROW_NUMBER() OVER (
			PARTITION BY IdLoja
			,IdItemDetalhe ORDER BY cdSemanaWalmartForecast ASC
				,Semana ASC
				,DiaSemana ASC
			) DiaMes
	FROM WLMSLP_STAGE..ForecastDiarioCd

	CREATE INDEX IDX_#DiasForecast ON #DiasForecast (IdForecastDiarioCd) INCLUDE (
		Semana
		,DiaMes
		)

	UPDATE F
	SET Semana = D.Semana
		,DiaMes = D.DiaMes
	FROM WLMSLP_STAGE..ForecastDiarioCd F
	JOIN #DiasForecast D
		ON D.IdForecastDiarioCd = F.IdForecastDiarioCd

	UPDATE WLMSLP_STAGE..ForecastDiarioCd
	SET DataForecast = DtInicioForecast + DiaMes

	--,
	--	DiaSemana = DATEPART(WEEKDAY, DtInicioForecast + DiaMes)
	UPDATE WLMSLP_STAGE..ForecastDiarioCd
	SET PercentualDiario = CASE DiaSemana
			WHEN 1
				THEN SUBSTRING(Alloc, 7, 2)
			WHEN 2
				THEN SUBSTRING(Alloc, 9, 2)
			WHEN 3
				THEN SUBSTRING(Alloc, 11, 2)
			WHEN 4
				THEN SUBSTRING(Alloc, 13, 2)
			WHEN 5
				THEN SUBSTRING(Alloc, 15, 2)
			WHEN 6
				THEN SUBSTRING(Alloc, 17, 2)
			WHEN 7
				THEN SUBSTRING(Alloc, 19, 2)
			END

	UPDATE WLMSLP_STAGE..ForecastDiarioCd
	SET ValorForecast = (VlforecastSemana / 100) * PercentualDiario

	INSERT INTO WLMSLP_STAGE..ForecastSugestaoCd
	SELECT IdCd
		,idItemDetalhePedido
		,idItemDetalheSugestao
		,sum(ValorForecast) AS ValorForecast
	FROM (
		SELECT F.idCD
			,S.idItemDetalhePedido
			,S.idItemDetalheSugestao
			,SUM(F.ValorForecast) AS ValorForecast
		FROM WLMSLP_STAGE..SugestaoPedidoCd S
		INNER JOIN WLMSLP_STAGE..ForecastDiarioCd F
			ON F.idcd = S.idCD
				AND F.iditemdetalhe = S.IdItemDetalheSugestao
				AND F.DataForecast BETWEEN S.dtInicioForecast
					AND S.dtFimForecast
		GROUP BY F.idCD
			,S.idItemDetalhePedido
			,S.idItemDetalheSugestao
		
		UNION
		
		SELECT F.idCD
			,S.idItemDetalhePedido
			,S.idItemDetalheSugestao
			,SUM(((F.ValorForecast * IR.PesoUnitatio) * TotalParticipacaoReceita) / TotalRendimentoReceita) AS ValorForecast
		FROM WLMSLP_STAGE..SugestaoPedidoCD S
		INNER JOIN WLMSLP_STAGE..SugestaoPedidoCdItemRelacionado IR
			ON S.idItemDetalheSugestao = IR.idItemDetalheS
				AND S.idCD = IR.idCd
		INNER JOIN WLMSLP_STAGE..ForecastDiarioCD F
			ON IR.idItemDetalheP = F.IdItemDetalhe
				AND S.idCD = F.IdCd
				AND F.DataForecast BETWEEN S.dtInicioForecast
					AND S.dtFimForecast
		GROUP BY F.idCD
			,S.idItemDetalhePedido
			,S.idItemDetalheSugestao
		) sp
	GROUP BY IdCd
		,idItemDetalhePedido
		,idItemDetalheSugestao

END
