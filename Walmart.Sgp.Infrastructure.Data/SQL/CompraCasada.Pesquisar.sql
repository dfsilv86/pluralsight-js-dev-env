/*
DECLARE @idDepartamento INT, @cdSistema INT, @idFornecedorParametro INT, @idItemDetalheSaida INT, @blPossuiCadastro BIT;
SET @idDepartamento = 13;
SET @cdSistema = 1;
SET @idFornecedorParametro = NULL;
SET @idItemDetalheSaida = 621758;
SET @blPossuiCadastro = NULL;
--*/

SELECT
	D.IDDepartamento,
	D.cdDepartamento,
	D.dsDepartamento,
	D.blPerecivel,
	NULL AS SplitOn1,
	FP.cdV9D,
	FP.cdTipo,
	FP.IDFornecedorParametro,
	F.nmFornecedor,
    NULL AS SplitOn2,
	ID_S.IDItemDetalhe,
	ID_S.cdItem,
	ID_S.dsItem,
	ID_S.cdSistema,
	COUNT(*) qtItensEntrada,
	(SELECT COUNT(*) FROM CompraCasada CC WHERE CC.idItemDetalheSaida = ID_S.IDItemDetalhe AND CC.blAtivo = 1 AND CC.idFornecedorParametro = FP.IDFornecedorParametro) AS qtItensCadastradosCompraCasada
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
INNER JOIN Departamento D WITH(NOLOCK)
	ON D.IDDepartamento = ID_S.IDDepartamento AND D.blAtivo = 1 AND D.blPerecivel = 'S'
WHERE RIP.IDTipoRelacionamento = 1
  AND ID_S.tpStatus = 'A'
  AND ID_S.cdSistema = @cdSistema
  AND ID_S.blAtivo = 1
  AND ID_S.IDDepartamento = @idDepartamento
  AND ID_S.IDItemDetalhe = ISNULL(@idItemDetalheSaida, ID_S.IDItemDetalhe)
  AND FP.IDFornecedorParametro = ISNULL(@idFornecedorParametro, FP.IDFornecedorParametro)
  AND FP.tpStoreApprovalRequired IN ('Y', 'R')
  AND (@blPossuiCadastro IS NULL OR
  (@blPossuiCadastro = 1 AND (SELECT COUNT(*) FROM CompraCasada CC WHERE CC.idItemDetalheSaida = ID_S.IDItemDetalhe AND CC.blAtivo = 1 AND CC.idFornecedorParametro = FP.IDFornecedorParametro) > 0)
  OR
  (@blPossuiCadastro = 0 AND (SELECT COUNT(*) FROM CompraCasada CC WHERE CC.idItemDetalheSaida = ID_S.IDItemDetalhe AND CC.blAtivo = 1 AND CC.idFornecedorParametro = FP.IDFornecedorParametro) = 0)
  )
GROUP BY 
	FP.cdV9D,
	FP.cdTipo,
	FP.IDFornecedorParametro,
	F.nmFornecedor,
	ID_S.IDItemDetalhe,
	ID_S.cdItem,
	ID_S.dsItem,
	ID_S.cdSistema,
	D.IDDepartamento,
	D.cdDepartamento,
	D.dsDepartamento,
	D.blPerecivel