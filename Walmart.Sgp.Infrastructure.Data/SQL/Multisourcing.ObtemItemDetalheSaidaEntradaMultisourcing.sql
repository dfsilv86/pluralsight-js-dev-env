/*
DECLARE @idDepartamento INT, @cdSistema INT, @idItemDetalheSaida BIGINT, @idCD INT, @filtroMS INT, @filtroCadastro INT;
SET @idDepartamento = 7;
SET @cdSistema = 1;
SET @idItemDetalheSaida = 537310;
SET @idCD = NULL;
SET @filtroMS = 1;
SET @filtroCadastro = 2;
--*/

SELECT *
FROM (
	SELECT
		ID_SAIDA.IDItemDetalhe,
		ID_SAIDA.cdItem,
		ID_SAIDA.dsItem,
		ID_ENTRADA.IDItemDetalhe idItemDetalheEntrada,
		ID_ENTRADA.cdItem cdItemDetalheEntrada,
		ID_ENTRADA.dsItem dsItemDetalheEntrada,
		ID_ENTRADA.qtVendorPackage,
		ID_ENTRADA.vlPesoLiquido,
		NULL AS SplitOn2,
		FP.cdV9D,
		FP.cdTipo,
		NULL AS SplitOn3,
		F.nmFornecedor,
		NULL AS SplitOn4,
		CDS.IDCD,
		CDS.cdCD,
		NULL AS SplitOn5,
		M.vlPercentual,
		( 
			SELECT COUNT(1) 
			  FROM ItemDetalhe AS IDS WITH (NOLOCK)
			  JOIN Departamento D WITH (NOLOCK) ON D.IDDepartamento = IDS.IDDepartamento
			  JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK) ON RIS.IDItemDetalhe = IDS.IDItemDetalhe 
			  JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK) ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal 
			  JOIN FornecedorParametro FP WITH (NOLOCK) ON FP.IDFornecedorParametro = IDS.IDFornecedorParametro
			  JOIN Fornecedor F WITH (NOLOCK) ON F.IDFornecedor = IDS.IDFornecedor AND dbo.fnEVendorWalmart(F.cdFornecedor) <> 1
			 WHERE dbo.fnObterTipoReabastecimento(IDS.IDItemDetalhe, CDS.IDCD, 1) in (3,33,94)
			   AND FP.cdTipo IN ('D', 'L')
			   AND FP.tpStoreApprovalRequired IN ('Y', 'R') 
			   AND FP.cdStatusVendor = 'A'
			   AND D.cdSistema = @cdSistema AND D.blPerecivel = 'S'
			   AND IDS.tpStatus = 'A'
			   AND RIP.IDItemDetalhe = ID_SAIDA.IDItemDetalhe
		) as QtdItensEntrada,
		( 
			SELECT COUNT(1) 
			  FROM Multisourcing AS MS WITH (NOLOCK)
			  JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK) ON RIS.IDRelacionamentoItemSecundario = MS.IDRelacionamentoItemSecundario
			  JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK) ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
			  JOIN ItemDetalhe IDS WITH (NOLOCK) ON IDS.IDItemDetalhe = RIP.IDItemDetalhe
			 WHERE IDS.idItemDetalhe = ID_SAIDA.IDItemDetalhe AND MS.idCD = CDS.IDCD
		) AS QtdMultisourcing
	FROM ItemDetalhe ID_SAIDA WITH(NOLOCK)
	JOIN Departamento D WITH(NOLOCK) ON D.IDDepartamento = ID_SAIDA.IDDepartamento
	JOIN RelacionamentoItemPrincipal RIP WITH(NOLOCK) ON RIP.IDItemDetalhe = ID_SAIDA.IDItemDetalhe
	JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK) ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
	JOIN ItemDetalhe ID_ENTRADA ON ID_ENTRADA.IDItemDetalhe = RIS.IDItemDetalhe
	JOIN Departamento DEP_ENT WITH(NOLOCK) ON DEP_ENT.IDDepartamento = ID_ENTRADA.IDDepartamento
	JOIN Fornecedor F WITH(NOLOCK) ON F.IDFornecedor = ID_ENTRADA.IDFornecedor AND dbo.fnEVendorWalmart(F.cdFornecedor) <> 1
	JOIN FornecedorParametro FP WITH(NOLOCK) ON FP.IDFornecedorParametro = ID_ENTRADA.idFornecedorParametro
	JOIN (
			SELECT DISTINCT RILC.IDItem,
							CD.CDCD,
							CD.IDCD
					   FROM RelacaoItemLojaCD RILC WITH (NOLOCK)
					   JOIN LojaCDParametro LCP WITH (NOLOCK) ON LCP.IDLojaCDParametro = RILC.IDLojaCDParametro
					   JOIN CD WITH (NOLOCK) ON CD.IDCD = LCP.IDCD
					  WHERE RILC.blAtivo = 1
					    AND LCP.blAtivo = 1
					    AND CD.blAtivo = 1 
					    AND CD.blConvertido = 1
		  ) AS CDS ON CDS.IDItem = ID_SAIDA.IDItemDetalhe
	LEFT JOIN Multisourcing M WITH(NOLOCK) ON M.idRelacionamentoItemSecundario = RIS.IDRelacionamentoItemSecundario AND M.idCD = CDS.IDCD
	WHERE 
		ID_SAIDA.tpVinculado = 'S' 
		AND ID_SAIDA.tpStatus = 'A' 
		AND ID_SAIDA.cdSistema = @cdSistema 
		AND ID_SAIDA.IDItemDetalhe = ISNULL(@idItemDetalheSaida, ID_SAIDA.IDItemDetalhe)
		AND D.IDDepartamento = @idDepartamento 
		AND D.blPerecivel = 'S' AND D.blAtivo = 1
		AND ID_ENTRADA.tpStatus = 'A' 
		AND ID_ENTRADA.blAtivo = 1
		AND DEP_ENT.blPerecivel = 'S'
		AND F.stFornecedor = 'A'
		AND FP.cdTipo IN ('D', 'L') 
		AND FP.tpStoreApprovalRequired IN ('Y', 'R') 
		AND FP.cdStatusVendor = 'A'
		AND CDS.IDCD = ISNULL(@idCD, CDS.IDCD)
		AND dbo.fnObterTipoReabastecimento(ID_ENTRADA.IDItemDetalhe, CDS.IDCD, 1) in (3,33,94)
) AS TEMP
WHERE 
	(@filtroMS = 2 OR 
		(@filtroMS = 0 AND TEMP.QtdItensEntrada <= 1) OR 
		(@filtroMS = 1 AND TEMP.QtdItensEntrada > 1)
	 ) AND 
	(@filtroCadastro = 2 OR 
			(@filtroCadastro = 0 AND TEMP.QtdMultisourcing <= 1) OR 
			(@filtroCadastro = 1 AND TEMP.QtdMultisourcing > 1)
	)
ORDER BY cdItem