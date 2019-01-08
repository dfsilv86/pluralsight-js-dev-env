/*
DECLARE @idItemDetalheDestino INT
set @idItemDetalheDestino = 1
--*/

SELECT 
	COUNT(*)
 FROM RelacionamentoTransferencia WITH (NOLOCK)
WHERE IDItemDetalheDestino = @idItemDetalheDestino
  AND blAtivo = 1