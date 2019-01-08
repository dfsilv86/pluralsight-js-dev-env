/*
DECLARE @cdSistema AS BIGINT = 1
DECLARE @cdItem AS BIGINT = 500364127
DECLARE @idCD AS INT = NULL
DECLARE @filtroMS AS INT = 1
DECLARE @filtroCadastro AS INT = 2
DECLARE @cddepartamento AS BIGINT = 83
--*/

SELECT 
	*
FROM 
(
	SELECT 
		IDC.IDItemDetalhe, 
		IDC.dsItem, 
		IDC.cdItem, 
		( 	
			SELECT COUNT(1) 
			FROM Multisourcing AS MS WITH (NOLOCK)
			JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
				ON RIS.IDRelacionamentoItemSecundario = MS.IDRelacionamentoItemSecundario
			JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
				ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
			JOIN ItemDetalhe ID WITH (NOLOCK)
				ON ID.IDItemDetalhe = RIP.IDItemDetalhe
			WHERE ID.cdItem = IDC.cdItem 
				AND MS.idCD = CDS.IDCD
		) AS QtdMultisourcing,
		CDS.cdCD,
		CDS.IDCD,
		( 
			SELECT COUNT(1) 
			FROM ItemDetalhe AS IDS WITH (NOLOCK)
			JOIN Departamento D WITH (NOLOCK) 
				ON D.IDDepartamento = IDS.IDDepartamento 
				AND D.cdSistema = 1 AND D.blPerecivel = 'S'
			JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK) 
				ON RIS.IDItemDetalhe = IDS.IDItemDetalhe 
				AND IDS.tpStatus = 'A'
			JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK) 
				ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal 
				AND RIP.IDItemDetalhe = IDC.IDItemDetalhe
			JOIN FornecedorParametro FP WITH (NOLOCK) ON FP.IDFornecedorParametro = IDS.IDFornecedorParametro
				AND FP.cdTipo IN ('D', 'L') -- Nova validação para canal do vendor
				AND FP.tpStoreApprovalRequired IN ('Y', 'R') -- somente itens que podem ser visualizados para as lojas
				AND FP.cdStatusVendor = 'A' -- ativo replenishment
			JOIN Fornecedor F WITH (NOLOCK) ON FP.IDFornecedor = F.IDFornecedor
				AND dbo.fnEVendorWalmart(F.cdFornecedor) <> 1 -- removendo fornecedores Walmart
			WHERE dbo.fnObterTipoReabastecimento(IDS.IDItemDetalhe, CDS.IDCD, 1) in (3,33,94)
		) as QtdItensEntrada -- 05/05/2016 - Retirada validação do cadastro RelacaoItemLojaCD do item de entrada (alinhado com Pedro e Iran) 
	FROM 
	(
		SELECT 
			IDD.IDItemDetalhe,
			IDD.IDDepartamento,
			IDD.dsItem,
			IDD.cdItem
		FROM ItemDetalhe IDD WITH (NOLOCK)
		JOIN Departamento DP
			ON DP.IDDepartamento = IDD.IDDepartamento
			AND DP.blPerecivel = 'S'
			AND DP.blAtivo = 1
			AND (DP.CDDepartamento = @CDdepartamento OR @CDdepartamento IS NULL)
		WHERE IDD.tpVinculado = 'S' 
			AND IDD.tpStatus = 'A' -- mudança de blAtivo para tpStatus
			AND (IDD.cdSistema = @cdSistema OR @cdSistema IS NULL) 
	) AS IDC,
	( 
		SELECT DISTINCT RILC.IDItem,
			CD.CDCD, 
			CD.IDCD
		FROM RelacaoItemLojaCD RILC WITH (NOLOCK)
		JOIN LojaCDParametro lcp WITH (NOLOCK)
			ON lcp.IDLojaCDParametro = RILC.IDLojaCDParametro
			AND lcp.blAtivo = 1
		JOIN CD WITH (NOLOCK)
			ON CD.IDCD = lcp.IDCD
			AND cd.blAtivo = 1
			AND cd.blConvertido=1
		WHERE RILC.blAtivo = 1 
	) AS CDS
	WHERE CDS.IDItem = IDC.IDItemDetalhe
		AND (IDC.cdItem = @cditem OR @cditem IS NULL)
		AND (@idCD IS NULL OR CDS.IDCD = @idCD)
) AS IDDC 
WHERE 
	(@filtroMS = 2 
		OR (@filtroMS = 0 AND IDDC.QtdItensEntrada <= 2) OR (@filtroMS = 1 AND IDDC.QtdItensEntrada > 2)
	)
	AND 
	(@filtroCadastro = 2 
		OR (@filtroCadastro = 0 AND IDDC.QtdMultisourcing <= 1) OR (@filtroCadastro = 1 AND IDDC.QtdMultisourcing > 1)
	)