/*
DECLARE @idDepartamento INT, @cdSistema INT, @idFornecedorParametro INT, @idItemDetalheSaida INT;
SET @idDepartamento = 13;
SET @cdSistema = 1;
SET @idFornecedorParametro = 32589;
SET @idItemDetalheSaida = 6436;
--*/

SELECT COUNT(*) 
FROM ItemDetalhe ID_S WITH(NOLOCK)
INNER JOIN RelacionamentoItemPrincipal RIP WITH(NOLOCK)
	ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe
INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
	ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
INNER JOIN ItemDetalhe ID_E WITH(NOLOCK)
	ON ID_E.IDItemDetalhe = RIS.IDItemDetalhe AND ID_E.blAtivo = 1 AND ID_E.tpStatus = 'A' 
INNER JOIN FornecedorParametro FP WITH(NOLOCK)
	ON FP.IDFornecedorParametro = ID_E.idFornecedorParametro AND FP.blAtivo = 1
INNER JOIN Fornecedor F WITH(NOLOCK)
	ON F.IDFornecedor = FP.IDFornecedor AND F.blAtivo = 1
LEFT JOIN CompraCasada CC_Filho WITH(NOLOCK)
	ON CC_Filho.idItemDetalheSaida = ID_S.IDItemDetalhe 
	AND CC_Filho.idItemDetalheEntrada = ID_E.IDItemDetalhe 
	AND CC_Filho.idFornecedorParametro = FP.IDFornecedorParametro
	AND CC_Filho.blItemPai = 0 
	AND CC_Filho.blAtivo = 1
LEFT JOIN CompraCasada CC_Pai WITH(NOLOCK)
	ON CC_Pai.idItemDetalheSaida = ID_S.IDItemDetalhe 
	AND CC_Pai.idItemDetalheEntrada = ID_E.IDItemDetalhe 
	AND CC_Pai.idFornecedorParametro = FP.IDFornecedorParametro
	AND CC_Pai.blItemPai = 1
	AND CC_Pai.blAtivo = 1
LEFT JOIN CompraCasada CC WITH(NOLOCK)
    ON CC.idItemDetalheSaida = ID_S.IDItemDetalhe 
	AND CC.idItemDetalheEntrada = ID_E.IDItemDetalhe 
	AND CC.idFornecedorParametro = FP.IDFornecedorParametro
	AND CC.blAtivo = 1
INNER JOIN Departamento D WITH(NOLOCK)
	ON D.IDDepartamento = ID_E.IDDepartamento AND D.blPerecivel = 'S' AND D.blAtivo = 1
LEFT JOIN Trait T WITH (NOLOCK)
    ON T.IdItemDetalhe = ID_E.IDItemDetalhe AND T.blAtivo = 1
LEFT JOIN Loja L WITH (NOLOCK)
    ON L.IdLoja = T.IdLoja AND L.blAtivo = 1
WHERE RIP.IDTipoRelacionamento = 1
  AND ID_S.tpStatus = 'A'
  AND ID_S.cdSistema = @cdSistema
  AND ID_S.blAtivo = 1
  AND (@idDepartamento IS NULL OR ID_S.IDDepartamento = @idDepartamento)
  AND (@idItemDetalheSaida IS NULL OR ID_S.IDItemDetalhe = @idItemDetalheSaida)
  AND (@idFornecedorParametro IS NULL OR FP.IDFornecedorParametro = @idFornecedorParametro)
  AND FP.tpStoreApprovalRequired IN ('Y', 'R')
  AND CC.blAtivo = 1
GROUP BY 
	ID_E.IDItemDetalhe,
	ID_E.cdItem,
	ID_E.dsItem,
	ID_E.vlTipoReabastecimento,
	ID_E.qtVendorPackage,
	ID_E.vlCustoUnitario,
	ID_S.IDItemDetalhe,
	ID_S.cdItem,
	ID_S.dsItem,
	FP.IDFornecedorParametro,
	FP.cdV9D,
	FP.cdTipo,
	F.nmFornecedor,
	D.cdDepartamento,
	D.dsDepartamento,
	D.blPerecivel,
	CC.idCompraCasada