/*
DECLARE @dtInicioReturn DATE, @dtFinalReturn DATE, @cdV9D BIGINT, @evento VARCHAR(50), @cdItemDetalhe INT, @cdDepartamento INT, @cdLoja INT, @idRegiaoCompra INT, @blExportado INT, @blAutorizado INT
SET @dtInicioReturn = '2016-10-20'
SET @dtFinalReturn = '2016-11-04'
SET @cdV9D = NULL
SET @evento = NULL
SET @cdItemDetalhe = NULL
SET @cdDepartamento = 80;
SET @cdLoja = NULL
SET @idRegiaoCompra = NULL
SET @blExportado = NULL
SET @blAutorizado = NULL
--*/

SELECT 
	SRS.IdSugestaoReturnSheet,
	SRS.BlExportado,
	SRS.BlAutorizado,
	U_EXP.FullName as UsuarioAutorizacao,
	SRS.DhExportacao,
	SRS.DhAutorizacao,
	R.Descricao,
	R.DhInicioEvento,
	R.DhFinalEvento,
	R.DhInicioReturn,
	R.DhFinalReturn,
	R.BlAtivo,
	ID_SAIDA.cdItem as cdItemDetalheSaida,
	ID_SAIDA.dsItem as dsItemDetalheSaida,
	FP.cdV9D,
	FP.cdTipo,
	F.nmFornecedor,
	ID_ENTRADA.cdItem as cdItemDetalheEntrada,
	ID_ENTRADA.dsItem as dsItemDetalheEntrada,
	SRS.vlCustoContabilItemVenda,
	SRS.EstoqueItemVenda,
	L.cdLoja,
	SRS.qtVendorPackageItemCompra,
	SRS.vlPesoLiquidoItemCompra,
	dbo.fnObterTipoReabastecimento(ID_ENTRADA.IDItemDetalhe, LP.IDCD, L.IDLoja) as TipoRA,
	SRS.QtdLoja,
	SRS.BlAtivo as SRSBlAtivo,
	SRS.PrecoVenda,
	SRS.qtdRA,
	ID_ENTRADA.tpCaixaFornecedor,
	CAST(ROUND((
		CASE WHEN ID_ENTRADA.tpCaixaFornecedor = 'F'
		THEN ISNULL(SRS.qtdRA, 0)
		ELSE 
			(CASE WHEN SRS.vlPesoLiquidoItemCompra = 0
				THEN ISNULL(SRS.qtdRA, 0)
				ELSE ISNULL(SRS.qtdRA, 0) / SRS.vlPesoLiquidoItemCompra 
			END) 
		END),0) as INT) as Subtotal,
	SRS.DhAtualizacao,
	U_ATUALIZACAO.FullName as UsuarioAtualizacao
FROM SugestaoReturnSheet SRS WITH(NOLOCK)
INNER JOIN ReturnSheetItemLoja RSIL WITH(NOLOCK)
	ON RSIL.IdReturnSheetItemLoja = SRS.IdReturnSheetItemLoja
INNER JOIN ReturnSheetItemPrincipal RSIP WITH(NOLOCK)
	ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
INNER JOIN ReturnSheet R WITH(NOLOCK)
	ON R.IdReturnSheet = RSIP.IdReturnSheet AND R.blAtivo = 1
INNER JOIN ItemDetalhe ID_ENTRADA WITH(NOLOCK)
	ON ID_ENTRADA.IDItemDetalhe = RSIL.IdItemDetalhe
INNER JOIN ItemDetalhe ID_SAIDA WITH(NOLOCK)
	ON ID_SAIDA.IDItemDetalhe = RSIP.IdItemDetalhe
LEFT JOIN CWIUser U_EXP WITH(NOLOCK)
	ON U_EXP.Id = SRS.idUsuarioAutorizacao
INNER JOIN FornecedorParametro FP WITH(NOLOCK)
	ON FP.IDFornecedorParametro = ID_ENTRADA.IDFornecedorParametro
INNER JOIN Fornecedor F WITH(NOLOCK)
	ON F.IDFornecedor = FP.IDFornecedor
INNER JOIN Loja L WITH(NOLOCK)
	ON L.IDLoja = RSIL.IdLoja
INNER JOIN LojaCDParametro LP WITH(NOLOCK)
	ON LP.IDLoja = L.IDLoja AND LP.blAtivo = 1
INNER JOIN RelacaoItemLojaCD RL WITH(NOLOCK)
    ON RL.IDLojaCDParametro = LP.IDLojaCDParametro AND 
		CASE WHEN coalesce(ID_ENTRADA.tpvinculado, ID_ENTRADA.tpmanipulado, ID_ENTRADA.tpreceituario) IS NULL 
		THEN RL.IDItem
		ELSE RL.IDItemEntrada END = ID_ENTRADA.IDItemDetalhe AND 
		RL.blAtivo = 1
LEFT JOIN CWIUser U_ATUALIZACAO WITH(NOLOCK)
	ON U_ATUALIZACAO.Id = SRS.IdUsuarioAtualizacao
INNER JOIN Departamento D WITH(NOLOCK)
	ON D.IDDepartamento = R.idDepartamento	
WHERE
	SRS.BlAtivo = 1
	AND (@dtInicioReturn IS NULL OR (@dtInicioReturn IS NOT NULL AND (CAST(@dtInicioReturn AS DATE) <= CAST(R.DhInicioReturn AS DATE))))
	AND (@dtFinalReturn IS NULL OR (@dtFinalReturn IS NOT NULL AND (CAST(@dtFinalReturn AS DATE) >= CAST(R.DhFinalReturn AS DATE))))
	AND (@cdV9D IS NULL OR FP.cdV9D = @cdV9D)
	AND (@evento IS NULL OR R.Descricao LIKE '%' + @evento + '%')
	AND (@cdItemDetalhe IS NULL OR ID_ENTRADA.cdItem = @cdItemDetalhe)
	AND (@cdDepartamento IS NULL OR D.cdDepartamento = @cdDepartamento)
	AND (@cdLoja IS NULL OR L.cdLoja = @cdLoja)
	AND (@idRegiaoCompra IS NULL OR ID_ENTRADA.idRegiaoCompra = @idRegiaoCompra)
	AND (@blExportado IS NULL OR SRS.BlExportado = @blExportado)
	AND (@blAutorizado IS NULL OR SRS.BlAutorizado = @blAutorizado)