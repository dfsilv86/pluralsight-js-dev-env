/*
DECLARE @idDepartamento INT, @cdSistema INT, @idFornecedorParametro INT, @idItemDetalheSaida INT, @blPossuiCadastro BIT;
SET @idDepartamento = 13;
SET @cdSistema = 1;
SET @idFornecedorParametro = null;
SET @idItemDetalheSaida = null;
SET @blPossuiCadastro = 1;
--*/

SELECT 
	D.cdDepartamento,
	D.dsDepartamento,
	D.blPerecivel,
	NULL AS SplitOn1,
	ID_S.cdItem AS CdItemSaida,
	ID_S.dsItem AS DsItemSaida,
    ID_S.IDItemDetalhe AS IdItemDetalheSaida,
	NULL AS SplitOn2,
	FP.IDFornecedorParametro,
	FP.cdV9D,
	F.nmFornecedor,
	FP.cdTipo,
	NULL AS SplitOn3,
    ID_E.IDItemDetalhe,
	ID_E.cdItem,
    ID_E.cdSistema,
	ID_E.dsItem,
	ID_E.vlTipoReabastecimento,
	ID_E.qtVendorPackage,
	ID_E.vlCustoUnitario,
	CAST((CASE WHEN COUNT(CC_Filho.idCompraCasada) = 0 THEN 0 ELSE 1 END) AS BIT) AS FilhoCompraCasada,
	CAST((CASE WHEN COUNT(CC_Pai.idCompraCasada) = 0 THEN 0 ELSE 1 END) AS BIT) AS PaiCompraCasada,
	COUNT(L.cdLoja) AS Traits,
    CC.idCompraCasada,
    NULL As SplitOn4,
    M.idMultisourcing
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
    ON L.IdLoja = T.IdLoja AND L.blAtivo = 1 AND L.cdSistema = @cdSistema
LEFT JOIN Multisourcing M WITH(NOLOCK)    
	ON M.idRelacionamentoItemSecundario = RIS.IDRelacionamentoItemSecundario AND M.vlPercentual > 0
WHERE RIP.IDTipoRelacionamento = 1
  AND ID_S.tpStatus = 'A'
  AND ID_S.cdSistema = @cdSistema
  AND ID_S.blAtivo = 1
  AND (@idDepartamento IS NULL OR ID_S.IDDepartamento = @idDepartamento)
  AND (@idItemDetalheSaida IS NULL OR ID_S.IDItemDetalhe = @idItemDetalheSaida)
  AND (@idFornecedorParametro IS NULL OR FP.IDFornecedorParametro = @idFornecedorParametro)
  AND FP.tpStoreApprovalRequired IN ('Y', 'R')
  AND (@blPossuiCadastro IS NULL OR
  (@blPossuiCadastro = 1 AND (SELECT COUNT(*) FROM CompraCasada CC WHERE CC.idItemDetalheSaida = ID_S.IDItemDetalhe AND CC.blAtivo = 1 AND CC.idFornecedorParametro = FP.IDFornecedorParametro) > 0)
  OR
  (@blPossuiCadastro = 0 AND (SELECT COUNT(*) FROM CompraCasada CC WHERE CC.idItemDetalheSaida = ID_S.IDItemDetalhe AND CC.blAtivo = 1 AND CC.idFornecedorParametro = FP.IDFornecedorParametro) = 0)
  )
GROUP BY 
	ID_E.IDItemDetalhe,
	ID_E.cdItem,
	ID_E.dsItem,
	ID_E.vlTipoReabastecimento,
	ID_E.qtVendorPackage,
	ID_E.vlCustoUnitario,
    ID_E.cdSistema,
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
	CC.idCompraCasada,
	M.idMultisourcing