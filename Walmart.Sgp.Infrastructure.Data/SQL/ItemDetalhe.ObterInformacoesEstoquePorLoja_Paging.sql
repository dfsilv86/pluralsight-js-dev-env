/*
DECLARE @cdItem BIGINT, @idBandeira INT, @idLoja INT;
SET @cdItem = 8002234;
SET @idBandeira = 1;
--*/

DECLARE @DataAtual DATETIME = GETDATE();

WITH ItemSelecionado AS (

	SELECT IDItemDetalhe FROM ItemDetalhe IDE WITH (NOLOCK) WHERE cdItem = @cdItem
	
), EstoqueMaisRecente AS (

	SELECT E.IDItemDetalhe
		 , E.IDLoja
		 , MAX(E.dtRecebimento) AS dtRecebimento
	  FROM Estoque E WITH (NOLOCK)
		   INNER JOIN ItemSelecionado IDS
				   ON IDS.IDItemDetalhe = E.IDItemDetalhe
		   INNER JOIN Loja L WITH (NOLOCK) 
				   ON L.IDLoja = E.IDLoja
	 WHERE (@idLoja IS NULL OR E.IDLoja = @idLoja)
	   AND (@idBandeira IS NULL OR L.IDBandeira = @idBandeira)
	 GROUP BY E.IDItemDetalhe
			, E.IDLoja
			
), EstoqueSelecionado AS (

	SELECT E.IDEstoque
	     , E.IDLoja
	     , E.IDItemDetalhe
	     , E.vlCustoContabilAtual
	  FROM EstoqueMaisRecente EMR WITH (NOLOCK)
	       INNER JOIN Estoque E
	               ON E.IDItemDetalhe = EMR.IDItemDetalhe
	              AND E.IDLoja = EMR.IDLoja
	              AND E.dtRecebimento = EMR.dtRecebimento
)
SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
				SELECT IDE.IDItemDetalhe
					 , IDE.cdItem
					 , L.cdLoja
					 , L.IDLoja
					 , CONVERT(VARCHAR,L.cdLoja)+ ' - '+ L.nmLoja as Loja
					 , (CASE When ISNULL(IDE.tpUnidadeMedida, 'Q') = 'Q' THEN 'Quilo' ELSE 'Unidade' END) AS tpUnidadeMedida
					 , CONVERT(NUMERIC(10,3), dbo.fnObterPosicaoEstoque(ide.iditemdetalhe, L.IDLoja, @DataAtual)) as qtEstoqueFisico
					 , ISNULL(ES.vlCustoContabilAtual, 0) vlCustoCompra
					 , dbo.fnObterPosicaoEstoque(ES.IDItemDetalhe, ES.IDLoja, @DataAtual) * ISNULL(ES.vlCustoContabilAtual, 0) AS qtEstoqueFinanceiro
				  FROM EstoqueSelecionado ES
					   INNER JOIN ItemDetalhe IDE WITH (NOLOCK)
							   ON IDE.IDItemDetalhe = ES.IDItemDetalhe
					   INNER JOIN Loja L WITH (NOLOCK)
							   ON L.IDLoja = ES.IDLoja
					   INNER JOIN Bandeira B WITH (NOLOCK)
							   ON B.IDBandeira = L.IDBandeira
			   ) __INTERNAL
       ) AS RowConstrainedResult
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum