/*
DECLARE @cdItemFilho INT, @cdSistema INT;
SET @cdItemFilho = 500128015;
SET @cdSistema = 1;
--*/

DECLARE @idItemFilho BIGINT = (SELECT IDItemDetalhe FROM ItemDetalhe WITH (NOLOCK) WHERE cdItem = @cdItemFilho AND cdSistema = @cdSistema AND blAtivo = 1 AND tpStatus = 'A');

DECLARE @idFornecedorParametroCC BIGINT = (SELECT idFornecedorParametro FROM CompraCasada AS CC WITH (NOLOCK) WHERE CC.blAtivo = 1 AND CC.idItemDetalheEntrada = @idItemFilho);
DECLARE @idItemDetalheSaida BIGINT = (SELECT idItemDetalheSaida FROM CompraCasada AS CC WITH (NOLOCK) WHERE CC.blAtivo = 1 AND CC.idItemDetalheEntrada = @idItemFilho);

SELECT ID_Pai.cdItem
FROM CompraCasada AS CC WITH (NOLOCK)
INNER JOIN ItemDetalhe ID_Pai WITH(NOLOCK)
	ON ID_Pai.IDItemDetalhe = CC.idItemDetalheEntrada
WHERE CC.blAtivo = 1 AND CC.blItemPai = 1 AND CC.idFornecedorParametro = @idFornecedorParametroCC AND CC.idItemDetalheSaida = @idItemDetalheSaida AND CC.idItemDetalheEntrada <> @idItemFilho