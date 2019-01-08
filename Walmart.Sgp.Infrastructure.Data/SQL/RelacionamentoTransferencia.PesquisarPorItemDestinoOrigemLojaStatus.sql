/*
DECLARE @idItemDetalheDestino INT, @idItemDetalheOrigem INT, @idLoja INT,@blAtivo BIT
SET @idItemDetalheDestino = NULL
SET @idItemDetalheOrigem = NULL
SET @idLoja = NULL
SET @blAtivo = 1
--*/

SELECT 
	 IDRelacionamentoTransferencia
	,IDItemDetalheDestino
	,IDItemDetalheOrigem
	,IDLoja
	,dtCriacao
	,IDUsuario
	,blAtivo
	,dtInativo
 FROM RelacionamentoTransferencia RT WITH (NOLOCK)
WHERE RT.IDItemDetalheDestino = @idItemDetalheDestino
  AND RT.IDItemDetalheOrigem = @idItemDetalheOrigem
  AND IDLoja = @idLoja
  AND blAtivo = @blAtivo