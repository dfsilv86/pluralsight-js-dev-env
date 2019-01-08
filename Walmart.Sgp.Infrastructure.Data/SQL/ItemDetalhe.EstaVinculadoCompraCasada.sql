
/*
DECLARE @cdSistema INT, @cdItemDetalheEntrada INT;
SET @cdSistema = 1;
SET @cdItemDetalheEntrada = 500113349
--*/

SELECT COUNT(*)
FROM CompraCasada CC WITH(NOLOCK)
JOIN ItemDetalhe ID_E WITH(NOLOCK) ON ID_E.IDItemDetalhe = CC.idItemDetalheEntrada
WHERE CC.blAtivo = 1
	AND ID_E.cdItem = @cdItemDetalheEntrada
	AND ID_E.cdSistema = @cdSistema