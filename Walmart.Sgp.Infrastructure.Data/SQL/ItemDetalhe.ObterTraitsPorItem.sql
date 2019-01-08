/*
DECLARE @IDItemDetalhe BIGINT, @cdSistema BIGINT;
SET @IDItemDetalhe = 48346;
SET @cdSistema = 1;
--*/

SELECT L.cdLoja, L.nmLoja
FROM ItemDetalhe ID WITH (NOLOCK)
	JOIN Trait T WITH (NOLOCK)
	ON T.IdItemDetalhe = ID.IdItemDetalhe AND T.blAtivo = 1 AND T.cdSistema = @cdSistema
	JOIN Loja L WITH (NOLOCK)
	ON L.IdLoja = T.IdLoja AND L.blAtivo = 1 AND L.cdSistema = @cdSistema
WHERE ID.idItemDetalhe = @IDItemDetalhe