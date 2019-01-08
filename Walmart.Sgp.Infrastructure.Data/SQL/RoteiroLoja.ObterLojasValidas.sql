/*
DECLARE @cdV9D BIGINT, @dsEstado VARCHAR(2), @idRoteiro INT;
SET @cdV9D = NULL;
SET @dsEstado = NULL;
SET @idRoteiro = NULL;
--*/

SELECT 
	TMP.idRoteiroLoja,
	TMP.idRoteiro,
	TMP.idloja,
	TMP.blativo,
	TMP.nmLoja,
	TMP.cdLoja,
	TMP.dsEstado,
	(CASE WHEN TMP.cdReviewDate LIKE '%1%' THEN 1 ELSE 0 END) blDomingo,
	(CASE WHEN TMP.cdReviewDate LIKE '%2%' THEN 1 ELSE 0 END) blSegunda,
	(CASE WHEN TMP.cdReviewDate LIKE '%3%' THEN 1 ELSE 0 END) blTerca,
	(CASE WHEN TMP.cdReviewDate LIKE '%4%' THEN 1 ELSE 0 END) blQuarta,
	(CASE WHEN TMP.cdReviewDate LIKE '%5%' THEN 1 ELSE 0 END) blQuinta,
	(CASE WHEN TMP.cdReviewDate LIKE '%6%' THEN 1 ELSE 0 END) blSexta,
	(CASE WHEN TMP.cdReviewDate LIKE '%7%' THEN 1 ELSE 0 END) blSabado
FROM (
	SELECT 
		RL.idRoteiroLoja,
		RL.idRoteiro,
		RL.idloja,
		RL.blativo,
		L.nmLoja,
		L.cdLoja,
		L.dsEstado,
		ISNULL(FLP.cdReviewDate, FP.cdReviewDate) as cdReviewDate
	FROM Roteiro R WITH(NOLOCK)
	JOIN RoteiroLoja RL WITH(NOLOCK) ON RL.idRoteiro = R.idRoteiro
	JOIN FornecedorParametro FP WITH(NOLOCK) ON FP.IDFornecedorParametro = 
		(SELECT TOP 1 IDFornecedorParametro 
		 FROM FornecedorParametro
		 WHERE cdV9D = R.cdV9D
			AND blAtivo = 1
			AND cdTipo IN('L','D')
		 ORDER BY cdTipo DESC)
	LEFT JOIN FornecedorLojaParametro FLP WITH(NOLOCK) ON FLP.IDFornecedorParametro = FP.IDFornecedorParametro AND FLP.IDLoja = RL.idloja
	JOIN Loja L WITH(NOLOCK) ON L.IDLoja = RL.idloja
	WHERE R.idRoteiro = @idRoteiro
	AND (@dsEstado IS NULL OR L.dsEstado = @dsEstado)
	AND R.cdV9D = @cdV9D
	AND (RL.blAtivo = 1 OR NOT EXISTS(
		SELECT 1 
		FROM Roteiro R
		JOIN RoteiroLoja RL ON RL.idRoteiro = R.idRoteiro
		WHERE RL.idloja = L.IDLoja 
			AND RL.blativo = 1
			AND R.idRoteiro <> @idRoteiro
			AND R.cdV9D = @cdV9D))

	UNION	

	SELECT DISTINCT
		0 as idRoteiroLoja,
		0 as idRoteiro,
		T.idloja,
		0 as blativo,
		L.nmLoja,
		L.cdLoja,
		L.dsEstado,
		ISNULL(FLP.cdReviewDate, FP.cdReviewDate) as cdReviewDate
	FROM ItemDetalhe I WITH(NOLOCK)
	JOIN Trait T WITH(NOLOCK) ON T.IDItemDetalhe = I.IDItemDetalhe AND T.blAtivo = 1
	JOIN FornecedorParametro FP WITH(NOLOCK) ON FP.IDFornecedorParametro = I.idFornecedorParametro AND FP.IDFornecedorParametro = 
		(SELECT TOP 1 IDFornecedorParametro 
		 FROM FornecedorParametro WITH(NOLOCK)
		 WHERE cdV9D = @cdV9D
			AND blAtivo = 1
			AND cdTipo IN('L','D')
		 ORDER BY cdTipo DESC)
	LEFT JOIN FornecedorLojaParametro FLP WITH(NOLOCK) ON FLP.IDFornecedorParametro = FP.IDFornecedorParametro AND FLP.IDLoja = T.IdLoja
	JOIN Loja L WITH(NOLOCK) ON L.IDLoja = T.IdLoja AND L.blAtivo = 1
	WHERE
	   I.blAtivo = 1
	   AND (@dsEstado IS NULL OR L.dsEstado = @dsEstado)
	   AND NOT EXISTS(
			SELECT 1
			FROM Roteiro R WITH(NOLOCK)
			JOIN RoteiroLoja RL WITH(NOLOCK) ON RL.idRoteiro = R.idRoteiro
			WHERE R.cdV9D = @cdV9D
				AND RL.idloja = L.IDLoja
				AND (RL.blativo = 1 OR (RL.blativo = 0 AND R.idRoteiro = @idRoteiro)))
) TMP