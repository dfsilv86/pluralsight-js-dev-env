/*
DECLARE @idMs INT;
SET @idMs = 1;
--*/

SELECT MS.IDMultisourcing, MS.IDCD, MS.IDRelacionamentoItemSecundario, MS.vlPercentual,
MS.cdUsuarioInclusao, MS.dtInclusao, MS.cdUsuarioAlteracao, MS.dtAlteracao,
NULL AS SplitOn1, (
SELECT TOP 1 FP.IDFornecedorParametro
  FROM Fornecedor F WITH(NOLOCK)
	   INNER JOIN FornecedorParametro FP WITH(NOLOCK)
		       ON FP.IDFornecedor = F.IDFornecedor
 WHERE FP.cdSistema = IDS.cdSistema
   AND FP.cdStatusVendor = 'A'
   AND F.cdSistema = IDS.cdSistema
   AND F.stFornecedor = 'A'
) AS IDFornecedorParametro,
NULL AS SplitOn2,
IDE.IDItemDetalhe,
NULL AS SplitOn3,
IDS.IDItemDetalhe FROM RelacionamentoItemSecundario AS RIS
INNER JOIN Multisourcing MS
	ON MS.IDRelacionamentoItemSecundario = RIS.IDRelacionamentoItemSecundario
INNER JOIN RelacionamentoItemPrincipal AS RIP
	ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
INNER JOIN ItemDetalhe IDE
	ON RIS.IDItemDetalhe = IDE.IDItemDetalhe
INNER JOIN ItemDetalhe IDS
	ON RIP.IDItemDetalhe = IDS.IDItemDetalhe
WHERE MS.IDMultisourcing = @idMs