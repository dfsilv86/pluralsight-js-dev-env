/*
DECLARE @idBandeira INT, @idLoja INT, @idDepartamento INT, @cdItem INT, @dsItem INT
SET @idBandeira = 1
SET @idLoja = NULL
SET @idDepartamento = NULL
SET @cdItem = NULL
SET @dsItem = NULL
--*/

SELECT
	RT.IDRelacionamentoTransferencia AS idRelacionamento, 
	B.dsBandeira,
	CONVERT(VARCHAR(100), cdLoja) + ' - ' + nmLoja AS dsLoja,
	CONVERT(VARCHAR(100), D2.cdDepartamento) + ' - ' + D2.dsDepartamento AS dsDepartamentoItemDestino,
	ID2.cdItem AS cdItemDestino,
	ID2.dsItem AS dsItemDestino,
	ID2.vlFatorConversao AS fatorConversaoItemDestino, 
	CASE ID2.tpUnidadeMedida 
		WHEN 'Q' THEN 'kg'
		WHEN 'U' THEN 'Unidade'
		ELSE 'Kg' 
	END	AS unidadeMedidaItemDestino,
	CONVERT(VARCHAR(100), D.cdDepartamento) + ' - ' + D.dsDepartamento AS dsDepartamentoItemOrigem,
	ID.cdItem AS cdItemOrigem,
	ID.dsItem AS dsItemOrigem,
	ID.vlFatorConversao AS fatorConversaoItemOrigem, 
	CASE ID.tpUnidadeMedida 
		WHEN 'Q' THEN 'kg'
		WHEN 'U' THEN 'Unidade'
		ELSE 'kg' 
	END	AS unidadeMedidaItemOrigem	
FROM RelacionamentoTransferencia RT WITH (NOLOCK) 
INNER JOIN Loja L WITH (NOLOCK) 
	ON L.IDLoja = RT.IDLoja AND L.blAtivo = 1
INNER JOIN Bandeira B WITH (NOLOCK) 
	ON B.IDBandeira = L.IDBandeira
INNER JOIN ItemDetalhe ID WITH (NOLOCK) 
	ON ID.IDItemDetalhe = RT.IDItemDetalheOrigem AND ID.blAtivo = 1
INNER JOIN ItemDetalhe ID2 WITH (NOLOCK) 
	ON ID2.IDItemDetalhe = RT.IDItemDetalheDestino
INNER JOIN Departamento D WITH (NOLOCK) 
	ON D.IDDepartamento = ID.IDDepartamento
INNER JOIN Departamento D2 WITH (NOLOCK) 
	ON D2.IDDepartamento = ID2.IDDepartamento
WHERE RT.blAtivo = 1
  AND (@idBandeira IS NULL OR B.IDBandeira = @idBandeira)
  AND (@idLoja IS NULL OR L.IDLoja = @idLoja)
  AND (@idDepartamento IS NULL OR (D.IDDepartamento = @idDepartamento OR D2.IDDepartamento = @idDepartamento))
  AND (@cdItem IS NULL OR ID2.cdItem = @cdItem)
  AND (@dsItem IS NULL OR (@dsItem IS NOT NULL AND  UPPER(ID2.dsItem) LIKE '%' + UPPER(@dsItem) + '%'))
  
  