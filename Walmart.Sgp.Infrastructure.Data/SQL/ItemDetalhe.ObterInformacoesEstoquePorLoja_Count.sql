/*
DECLARE @cdItem BIGINT, @idBandeira INT, @idLoja INT;
SET @cdItem = 8002234;
SET @idBandeira = 1;
--*/

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
)
SELECT COUNT(1)
  FROM EstoqueMaisRecente  