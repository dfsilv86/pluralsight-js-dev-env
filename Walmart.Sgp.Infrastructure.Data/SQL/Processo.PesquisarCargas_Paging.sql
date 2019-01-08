/*
DECLARE @cdSistema int, @idBandeira int, @cdLoja int, @data datetime;
SET @cdSistema = 1;
SET @idBandeira = 1;
--SET @cdLoja = 87;
SET @data = '2016-01-22';
--SELECT COUNT(1) FROM LOJA
*/

CREATE TABLE #Lojas (
		RowNum INT,
		IDBandeira INT,
		dsBandeira varchar(20),
		cdSistema smallint,
		cdLoja int,
		nmLoja varchar(60));

INSERT INTO #Lojas
SELECT RowConstrainedResult.*
	  FROM ( 
			SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
			  FROM (
				SELECT
					B.IDBandeira,
					B.dsBandeira,
					B.cdSistema,
					L.cdLoja,
					L.nmLoja
				FROM
					Bandeira B WITH (NOLOCK)
						INNER JOIN Loja L 
							ON B.IDBandeira = L.IDBandeira										
				WHERE
					B.cdSistema = @cdSistema AND
					B.IDBandeira =ISNULL(@idBandeira, B.IDBandeira) AND
					L.cdLoja = ISNULL(@cdLoja, L.cdLoja) 	
			) __INTERNAL
		) AS RowConstrainedResult        
WHERE   RowNum >= {0}
	AND RowNum < {1}
ORDER BY RowNum;

WITH ProcessosPorLoja AS (
	SELECT
		L.IDBandeira,
		L.dsBandeira,
		L.cdSistema,
		L.cdLoja,
		L.nmLoja,
		P.IdProcesso,
		P.Descricao,
		P.DiasProcessar
	FROM
		#Lojas L WITH (NOLOCK)
			CROSS JOIN LogTipoProcesso P						
),
UltimoLogExecucaoPorProcesso AS (
	SELECT 
		IdProcesso, 
		MAX(IdLogExecucao) as UltimoIdLogExecucao 
	FROM 
		LogExecucao
	WHERE	
		DataInicio IS NULL OR
		CAST(DataInicio as DATE) = @data
	GROUP BY
		IdProcesso
)

SELECT 
	PL.IDBandeira,
	PL.dsBandeira,
	PL.cdSistema,
	NULL AS SplitOn1,
	PL.cdLoja,
	PL.nmLoja,
	NULL AS SplitOn2,
	PL.Descricao AS Nome,
	PL.DiasProcessar AS PrazoExecucaoEstimadoEmDias,
	E.DataInicio as DataInicioExecucao,
	E.DataTermino as DataFimExecucao,	
	EX.Mensagem AS Observacao,
	NULL AS SplitOn3,
	ET.Descricao
FROM
	ProcessosPorLoja PL
		LEFT JOIN UltimoLogExecucaoPorProcesso UE WITH(NOLOCK)
			ON PL.IdProcesso = UE.IdProcesso
		LEFT JOIN LogExecucao E WITH(NOLOCK)
			ON UE.UltimoIdLogExecucao = E.IdLogExecucao
		LEFT JOIN LogExcecao EX WITH(NOLOCK)
			ON E.IdLogExecucao = EX.IdLogExecucao	
		LEFT JOIN LogTipoExcecao ET WITH(NOLOCK)
			ON EX.IdTipoLogExcecao = ET.IdTipoLogExcecao	


DROP TABLE #Lojas;