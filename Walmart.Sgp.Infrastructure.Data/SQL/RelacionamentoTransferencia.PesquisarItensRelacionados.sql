/*
DECLARE @idItemDetalheDestino INT
set @idItemDetalheDestino = 1
--*/

SELECT
	 RT.IDRelacionamentoTransferencia	 
	,RT.IDItemDetalheOrigem
	,RT.IDItemDetalheDestino
	,NULL AS SplitOn1
	,RT.IDLoja
	,L.cdLoja 
	,L.nmLoja
	,NULL AS SplitOn2
	,ID.cdItem
	,ID.dsItem	
FROM RelacionamentoTransferencia RT WITH (NOLOCK) 
INNER JOIN Loja L WITH (NOLOCK) 
	ON L.IDLoja = RT.IDLoja AND L.blAtivo = 1
INNER JOIN ItemDetalhe ID WITH (NOLOCK) 
	ON ID.IDItemDetalhe = RT.IDItemDetalheOrigem AND ID.blAtivo = 1
WHERE RT.IDItemDetalheDestino = @idItemDetalheDestino
AND RT.blAtivo = 1