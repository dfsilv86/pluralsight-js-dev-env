/*
DECLARE @cdItem INT, @cdSistema INT;
SET @cdItem = 12345;
SET @cdSistema = 1;
--*/

SELECT DISTINCT IDS.IDItemDetalhe
		, IDS.IDFineline
		, IDS.IDCategoria
		, IDS.IDSubcategoria
		, IDS.IDDepartamento
		, IDS.IDFornecedor
		, IDS.cdItem
		, IDS.cdOldNumber
		, IDS.vlCustoUnitario
		, IDS.cdSistema
		, IDS.cdUPC
		, IDS.dsItem
		, IDS.dsHostItem
		, IDS.blAtivo
		, IDS.dhHostCreate
		, IDS.dhHostUpdate
		, IDS.blPesadoCaixa
		, IDS.blPesadoRetaguarda
		, IDS.cdPLU
		, IDS.dsTamanhoItem
		, IDS.dsCor
		, IDS.dhCriacao
		, IDS.dhAtualizacao
		, IDS.tpStatus
		, IDS.dhAtualizacaoStatus
		, IDS.cdUsuarioCriacao
		, IDS.cdUsuarioAtualizacao
		, IDS.tpVinculado
		, IDS.tpReceituario
		, IDS.tpManipulado
		, IDS.qtVendorPackage
		, IDS.qtWarehousePackage
		, IDS.vlFatorConversao
		, IDS.tpUnidadeMedida
		, IDS.vlTipoReabastecimento
		, IDS.vlShelfLife
		, IDS.blItemTransferencia
		, IDS.vlModulo
		, IDS.cdDepartamentoVendor
		, IDS.cdSequenciaVendor
		, IDS.IDRegiaoCompra
		, NULL AS SplitOn1
		, F.IDFornecedor
		, F.cdFornecedor
		, F.nmFornecedor
		, F.blAtivo
		, F.dhCriacao
		, F.dhAtualizacao
		, F.cdUsuarioCriacao
		, F.cdUsuarioAtualizacao
		, F.stFornecedor
		, F.cdSistema
		, NULL AS SplitOn2
		, FL.IDFineLine
		, FL.IDSubcategoria
		, FL.IDCategoria
		, FL.IDDepartamento
		, FL.cdSistema
		, FL.cdFineLine
		, FL.dsFineLine
		, FL.blAtivo
		, FL.dhCriacao
		, FL.dhAtualizacao
		, FL.cdUsuarioCriacao
		, FL.cdUsuarioAtualizacao
		, NULL AS SplitOn3
		, SC.IDSubcategoria
		, SC.IDCategoria
		, SC.IDDepartamento
		, SC.cdSistema
		, SC.cdSubcategoria
		, SC.dsSubcategoria
		, SC.blAtivo
		, SC.dhCriacao
		, SC.dhAlteracao
		, SC.cdUsuarioCriacao
		, SC.cdUsuarioAlteracao
		, NULL AS SplitOn4
		, CA.IDCategoria
		, CA.IDDepartamento
		, CA.cdSistema
		, CA.cdCategoria
		, CA.dsCategoria
		, CA.blPerecivel
		, CA.blAtivo
		, CA.dhCriacao
		, CA.dhAtualizacao
		, CA.cdUsuarioCriacao
		, CA.cdUsuarioAtualizacao
		, NULL AS SplitOn5
		, DE.IDDepartamento
		, DE.IDDivisao
		, DE.cdSistema
		, DE.cdDepartamento
		, DE.dsDepartamento
		, DE.blPerecivel
		, DE.blAtivo
		, DE.dhCriacao
		, DE.dhAtualizacao
		, DE.cdUsuarioCriacao
		, DE.cdUsuarioAtualizacao
		, DE.pcDivergenciaNF
		, NULL AS SplitOn6
		, DI.IDDivisao
		, DI.cdSistema
		, DI.cdDivisao
		, DI.dsDivisao
		, DI.blAtivo
		, DI.dhCriacao
		, DI.dhAtualizacao
		, DI.cdUsuarioCriacao
		, DI.cdUsuarioAtualizacao
	FROM ItemDetalhe IDS WITH (NOLOCK)
		INNER JOIN Fornecedor F WITH (NOLOCK)
				ON F.IDFornecedor = IDS.IDFornecedor
		INNER JOIN FineLine FL WITH (NOLOCK)
				ON FL.IDFineLine = IDS.IDFineLine
		INNER JOIN Subcategoria SC WITH (NOLOCK)
				ON SC.IDSubcategoria = IDS.IDSubcategoria
		INNER JOIN Categoria CA WITH (NOLOCK)
				ON CA.IDCategoria = IDS.IDCategoria
		INNER JOIN Departamento DE WITH (NOLOCK)
				ON DE.IDDepartamento = IDS.IDDepartamento
		INNER JOIN Divisao DI WITH (NOLOCK)
				ON DI.IDDivisao = DE.IDDivisao
		INNER JOIN RelacionamentoItemPrincipal RP WITH(NOLOCK)
				ON RP.IDItemDetalhe = IDS.IDItemDetalhe
				AND RP.IDTipoRelacionamento = 1
		INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
				ON RS.IDRelacionamentoItemPrincipal = RP.IDRelacionamentoItemPrincipal
		INNER JOIN ItemDetalhe IDE WITH(NOLOCK)
				ON RS.IDItemDetalhe = IDE.IDItemDetalhe
		INNER JOIN Fornecedor FE WITH(NOLOCK)
				ON FE.IDFornecedor = IDE.IDFornecedor
	WHERE IDS.cdSistema = @cdSistema
	AND IDS.tpVinculado = 'S'
	AND IDE.IDFornecedorParametro IS NOT NULL
	AND (@cdItem IS NULL OR IDS.cdItem = @cdItem)
	AND (@cdPLU IS NULL OR IDS.cdPLU = @cdPLU)
	AND (@dsItem IS NULL OR IDS.dsItem LIKE '%' + @dsItem + '%')
	AND (@tpStatus IS NULL OR IDS.tpStatus = @tpStatus)
	AND (@cdFineLine IS NULL OR FL.cdFineline = @cdFineline)
	AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
	AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
	AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
	AND (@idFornecedorParametro IS NULL OR IDE.idFornecedorParametro = @idFornecedorParametro)
	AND (@IDRegiaoCompra IS NULL OR IDS.IDRegiaoCompra = @IDRegiaoCompra)
    AND (@blPerecivel IS NULL OR DE.blPerecivel = @blPerecivel)