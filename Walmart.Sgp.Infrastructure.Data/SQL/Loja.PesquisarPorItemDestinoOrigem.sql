/*
DECLARE @cdSistema INT, @idItemDetalheDestino INT, @idItemDetalheOrigem INT, @idLoja INT
SET @cdSistema = 1;
SET @idItemDetalheDestino = NULL
SET @idItemDetalheOrigem = NULL;
SET @idLoja = NULL;
--*/

IF @cdSistema = 1
BEGIN

	SELECT
		 L.IDLoja 
		,L.cdLoja
		,L.nmLoja
	FROM LOJA L WITH (NOLOCK) 
	JOIN Trait T WITH (NOLOCK) ON T.IdLoja = L.IDLoja AND T.IdItemDetalhe = @idItemDetalheDestino
	JOIN Trait T2 WITH (NOLOCK) ON T2.IdLoja = L.IDLoja 		
	WHERE NOT EXISTS (SELECT * FROM  RelacionamentoTransferencia r WITH (NOLOCK) WHERE l.IDLoja = r.IDLoja AND r.IDItemDetalheDestino = @idItemDetalheDestino AND r.blAtivo = 1)
		AND T.blAtivo = 1
		AND L.cdSistema = 1
		AND L.blAtivo = 1
		AND (@idItemDetalheOrigem IS NULL OR T2.IdItemDetalhe = @idItemDetalheOrigem)
		AND (@idLoja IS NULL OR L.IDLoja = @idLoja)
	ORDER BY
		cdLoja
END
ELSE IF @cdSistema = 2
BEGIN

	SELECT 
		 L.IDLoja 
		,L.cdLoja
		,L.nmLoja
	FROM LOJA L WITH (NOLOCK) 
	WHERE NOT EXISTS (SELECT * FROM  RelacionamentoTransferencia r WITH (NOLOCK) WHERE l.IDLoja = r.IDLoja AND r.IDItemDetalheDestino = @idItemDetalheDestino AND r.blAtivo = 1)
		AND (@idLoja IS NULL OR L.IDLoja = @idLoja)
		AND L.cdSistema = 2	
		AND L.blAtivo = 1	
	ORDER BY
		cdLoja
END
ELSE
BEGIN
	SELECT
		 0 AS IDLoja 
		,0 AS cdLoja
		,'' AS nmLoja		
	FROM LOJA L
	WHERE L.IDLoja = 0		
END