/*
DECLARE @idRelacionamentoItemPrincipal INT
SET @idRelacionamentoItemPrincipal = 1
--*/

SELECT 
	RIP.IDRelacionamentoItemPrincipal,
	RIP.cdSistema,
	RIP.IDTipoRelacionamento,
	RIP.IDItemDetalhe,
	RIP.IDDepartamento,
	RIP.IDCategoria,
	RIP.qtProdutoBruto,
	RIP.pcRendimentoReceita,
	RIP.qtProdutoAcabado,
	RIP.pcQuebra,
	RIP.dhCadastro,
	RIP.dhAlteracao,
	RIP.psUnitario,
	RIP.blReprocessamentoManual,
	RIP.statusReprocessamentoCusto,
	RIP.dtInicioReprocessamentoCusto,
	RIP.dtFinalReprocessamentoCusto,
	RIP.idUsuarioReprocessamento,
	RIP.descErroReprocessamento,
	RIP.idUsuarioAlteracao,
	NULL AS SplitOn1,		
	PItem.IDItemDetalhe,
	PItem.cdItem,
	PItem.dsItem,
	PItem.TpUnidadeMedida,
	PItem.tpVinculado,
    PItem.tpReceituario,
    PItem.tpManipulado,
	PItem.tpIndPesoReal,
	PItem.tpCaixaFornecedor,
	PItem.vlPesoLiquido,
	PItem.tpAlinhamentoCD,
	PItem.tempoMinimoCD,
	NULL AS SplitOn2,
	PD.dsDepartamento,
	NULL AS SplitOn3,
	RIS.IDRelacionamentoItemSecundario,
	RIS.IDRelacionamentoItemPrincipal,
	RIS.psItem,
	RIS.tpItem,
	RIS.pcRendimentoDerivado,
	RIS.IDItemDetalhe,
	RIS.qtItemUn,
	NULL AS SplitOn4,
	SItem.IDItemDetalhe,
	SItem.cdItem,
	SItem.dsItem,
    SItem.dsTamanhoItem,
    SItem.tpVinculado,
    SItem.tpReceituario,
    SItem.tpManipulado,
    NULL AS SplitOn5,
    RC.IdRegiaoCompra,
    RC.dsRegiaoCompra,
    NULL AS SplitOn6,
    ACD.IdAreaCD,
    ACD.dsAreaCD
FROM 
	RelacionamentoItemPrincipal RIP  WITH (NOLOCK)
		INNER JOIN ItemDetalhe PItem WITH (NOLOCK)
				ON PItem.IDItemDetalhe = RIP.IDItemDetalhe
		INNER JOIN Departamento PD WITH (NOLOCK)
				ON PD.IDDepartamento = PItem.IDDepartamento
		INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
				ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
		INNER JOIN ItemDetalhe SItem WITH (NOLOCK)
				ON SItem.IDItemDetalhe = RIS.IDItemDetalhe
		LEFT JOIN RegiaoCompra RC WITH (NOLOCK)
			    ON RC.IdRegiaoCompra = PItem.idRegiaoCompra
		LEFT JOIN AreaCD ACD WITH (NOLOCK)
				ON ACD.IdAreaCD = PItem.idAreaCD
WHERE
	RIP.IdRelacionamentoItemPrincipal = @idRelacionamentoItemPrincipal