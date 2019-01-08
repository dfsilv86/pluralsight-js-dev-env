/* Pesquisa dos Itens de Entrada do Item de Saída selecionado na pesquisa multisourcing
DECLARE @cdSistema BIGINT = 1
DECLARE @cdItem BIGINT = 500263116
DECLARE @cdCd BIGINT = 7400*/

SELECT DISTINCT 
       CD.cdCD as cdCD, 
       ID.cdItem, 
       ID.dsItem, 
       F.nmFornecedor, 
       FP.cdV9D, 
       ID.IDItemDetalhe, 
       FP.cdTipo, 
       IDP.cdItem as cdItemSaida, 
       RIS.IDRelacionamentoItemSecundario, 
       MS.IDMultisourcing, 
       MS.vlPercentual,
       CD.idCD,
       ID.qtVendorPackage,
       ID.vlPesoLiquido,
       CC.idCompraCasada
FROM ItemDetalhe IDP WITH (NOLOCK) --ItemSaída que veio da tela de pesquisa
INNER JOIN CD WITH (NOLOCK) --CD que veio da tela de pesquisa
      ON CD.cdCD = @cdCD
      AND CD.blConvertido = 1
INNER JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
      ON IDP.IDItemDetalhe = RIP.IDITEMDETALHE
INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK) --ItemEntrada
      ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal --ItemSaída
      and dbo.fnObterTipoReabastecimento(RIS.IDItemDetalhe, CD.idCD, 1) in (3,33,94)
join ItemDetalhe AS ID WITH (NOLOCK) --ItemEntrada 
      on ID.IDItemDetalhe = RIS.IDItemDetalhe
      AND ID.tpStatus = 'A'
      AND ID.blAtivo = 1
INNER JOIN Departamento D WITH (NOLOCK)
      ON D.IDDepartamento = ID.IDDepartamento
      AND D.cdSistema     = @cdSistema
      AND D.blPerecivel   = 'S'
INNER JOIN Fornecedor F WITH (NOLOCK)
      ON F.IDFornecedor = ID.IDFornecedor
      AND F.stFornecedor = 'A' -- ativo vendor master
	  AND dbo.fnEVendorWalmart(F.cdFornecedor) <> 1 -- removendo fornecedores Walmart
INNER JOIN FornecedorParametro FP WITH (NOLOCK)
      ON FP.IDFornecedorParametro = ID.IDFornecedorParametro
            AND FP.cdTipo IN ('D', 'L') -- Nova validação para canal do vendor (explicação no email)
            AND FP.tpStoreApprovalRequired IN ('Y', 'R') -- somente itens que podem ser visualizados para as lojas
            AND FP.cdStatusVendor = 'A' -- ativo replenishment
--_Verifica se o relacionamento já possui multisourcing
LEFT JOIN Multisourcing AS MS WITH (NOLOCK)
   ON MS.IDRelacionamentoItemSecundario = RIS.IDRelacionamentoItemSecundario AND MS.idCD = CD.IDCD
--Verifica se o item de entrada participa de uma compra casada
LEFT JOIN CompraCasada CC WITH(NOLOCK)
	ON CC.idItemDetalheEntrada = ID.IDItemDetalhe AND CC.blAtivo = 1
WHERE idp.cdItem = @cdItem