/*
DECLARE @cdItem INT, @cdSistema INT, @cdLoja INT;
SET @cdSistema = NULL;
SET @cdLoja = NULL;
SET @cdItem = NULL;
--*/

SELECT TOP 1 1
FROM Trait T WITH(NOLOCK)
INNER JOIN ItemDetalhe ID WITH(NOLOCK)
	ON ID.IDItemDetalhe = T.IdItemDetalhe
INNER JOIN Loja L WITH(NOLOCK)
	ON L.IDLoja = T.IdLoja
WHERE ID.cdItem = @cdItem
	AND ID.cdSistema = @cdSistema
	AND L.cdSistema = @cdSistema
	AND L.cdLoja = @cdLoja
	AND T.blAtivo = 1