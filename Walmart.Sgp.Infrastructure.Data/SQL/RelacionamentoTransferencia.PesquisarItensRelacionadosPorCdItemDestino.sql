/*
DECLARE @cdItemDestino INT
set @cdItemDestino = 1
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
	,IDO.cdItem
	,IDO.dsItem	
FROM RelacionamentoTransferencia RT WITH (NOLOCK) 
INNER JOIN Loja L WITH (NOLOCK) 
	ON L.IDLoja = RT.IDLoja AND L.blAtivo = 1
INNER JOIN ItemDetalhe IDO WITH (NOLOCK) 
	ON IDO.IDItemDetalhe = RT.IDItemDetalheOrigem AND IDO.blAtivo = 1
INNER JOIN ItemDetalhe IDD WITH (NOLOCK) 
	ON IDD.IDItemDetalhe = RT.IDItemDetalheDestino AND IDD.blAtivo = 1
WHERE 
	IDD.cdItem = @cdItemDestino
AND 
	RT.blAtivo = 1