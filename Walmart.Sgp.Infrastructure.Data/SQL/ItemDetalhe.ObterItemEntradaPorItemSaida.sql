/*
DECLARE @cdItem INT, @cdSistema INT;
SET @cdItem = 12345;
SET @cdSistema = 1;
--*/

SELECT IDE.IDItemDetalhe
		, IDE.IDFineline
		, IDE.IDCategoria
		, IDE.IDSubcategoria
		, IDE.IDDepartamento
		, IDE.IDFornecedor
		, IDE.cdItem
		, IDE.cdOldNumber
		, IDE.vlCustoUnitario
		, IDE.cdSistema
		, IDE.cdUPC
		, IDE.dsItem
		, IDE.dsHostItem
		, IDE.blAtivo
		, IDE.dhHostCreate
		, IDE.dhHostUpdate
		, IDE.blPesadoCaixa
		, IDE.blPesadoRetaguarda
		, IDE.cdPLU
		, IDE.dsTamanhoItem
		, IDE.dsCor
		, IDE.dhCriacao
		, IDE.dhAtualizacao
		, IDE.tpStatus
		, IDE.dhAtualizacaoStatus
		, IDE.cdUsuarioCriacao
		, IDE.cdUsuarioAtualizacao
		, IDE.tpVinculado
		, IDE.tpReceituario
		, IDE.tpManipulado
		, IDE.qtVendorPackage
		, IDE.qtWarehousePackage
		, IDE.vlFatorConversao
		, IDE.tpUnidadeMedida
		, IDE.vlTipoReabastecimento
		, IDE.vlShelfLife
		, IDE.blItemTransferencia
		, IDE.vlModulo
		, IDE.cdDepartamentoVendor
		, IDE.cdSequenciaVendor
		, IDE.IDRegiaoCompra
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
	FROM ItemDetalhe IDE WITH (NOLOCK)
		INNER JOIN Fornecedor F WITH (NOLOCK)
				ON F.IDFornecedor = IDE.IDFornecedor
		INNER JOIN FineLine FL WITH (NOLOCK)
				ON FL.IDFineLine = IDE.IDFineLine
		INNER JOIN Subcategoria SC WITH (NOLOCK)
				ON SC.IDSubcategoria = IDE.IDSubcategoria
		INNER JOIN Categoria CA WITH (NOLOCK)
				ON CA.IDCategoria = IDE.IDCategoria
		INNER JOIN Departamento DE WITH (NOLOCK)
				ON DE.IDDepartamento = IDE.IDDepartamento
		INNER JOIN Divisao DI WITH (NOLOCK)
				ON DI.IDDivisao = DE.IDDivisao
		INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
				ON RIS.IDItemDetalhe = IDE.IDItemDetalhe
		INNER JOIN RelacionamentoItemPrincipal RIP WITH(NOLOCK)
				ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
				AND RIP.IDTipoRelacionamento = 1
		INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
				ON IDS.IDItemDetalhe = RIP.IDItemDetalhe
	WHERE IDE.cdSistema = @cdSistema
		AND IDE.tpVinculado = 'E'
		AND IDS.cdItem = @cdItemSaida
		AND IDE.IDFornecedorParametro IS NOT NULL
		AND (@idFornecedorParametro IS NULL OR IDE.idFornecedorParametro = @idFornecedorParametro)
		AND IDE.vlTipoReabastecimento IS NOT NULL
		AND IDE.blItemTransferencia = 0