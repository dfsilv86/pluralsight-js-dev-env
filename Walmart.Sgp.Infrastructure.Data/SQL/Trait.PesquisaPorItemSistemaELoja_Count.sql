/*
DECLARE @cdItem INT, @cdSistema INT, @cdLoja INT;
SET @cdSistema = NULL;
SET @cdLoja = NULL;
SET @cdItem = NULL;
--*/

SELECT COUNT(*)
FROM Trait T WITH(NOLOCK)
INNER JOIN ItemDetalhe ID WITH(NOLOCK)
	ON ID.IDItemDetalhe = T.IdItemDetalhe AND ID.blAtivo = 1
INNER JOIN Loja L WITH(NOLOCK)
	ON L.IDLoja = T.IdLoja AND L.blAtivo = 1
WHERE ID.cdItem = @cdItem
	AND ID.cdSistema = @cdSistema
	AND L.cdSistema = @cdSistema
	AND L.cdLoja = @cdLoja
	AND T.blAtivo = 1