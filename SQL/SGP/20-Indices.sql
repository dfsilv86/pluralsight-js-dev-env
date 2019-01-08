DROP INDEX IX_RelacaoItemLojaCD_IDLojaCDParametro ON [dbo].[RelacaoItemLojaCD]

CREATE NONCLUSTERED INDEX IX_RelacaoItemLojaCD_IDLojaCDParametro
ON [dbo].[RelacaoItemLojaCD] ([IDLojaCDParametro],[blAtivo],[vlTipoReabastecimento])
INCLUDE ([IDItem],[idItemEntrada])


DROP INDEX IX_ItemDetalhe_cdSistema_tpStatus_tpVinculado_blItemTransferencia_cdDepartamentoVendor_vlTipoReabastecimento ON [dbo].[ItemDetalhe]

CREATE NONCLUSTERED INDEX IX_ItemDetalhe_cdSistema_tpStatus_tpVinculado_blItemTransferencia_cdDepartamentoVendor_vlTipoReabastecimento
ON [dbo].[ItemDetalhe] ([cdSistema],[blAtivo],[tpStatus],[tpVinculado],[blItemTransferencia])
INCLUDE ([IDItemDetalhe],[IDDepartamento],[cdItem],[cdPLU],[tpReceituario],[tpManipulado],[qtVendorPackage],[vlFatorConversao],[vlShelfLife],[vlModulo],[cdSequenciaVendor],[tpCaixaFornecedor],[vlPesoLiquido],[idFornecedorParametro])


CREATE NONCLUSTERED INDEX IX_FornecedorLojaParametro_IDFornecedorParametro
ON [dbo].[FornecedorLojaParametro] ([IDFornecedorParametro],[IDLoja])
INCLUDE ([cdReviewDate],[tpWeek],[tpInterval])

CREATE NONCLUSTERED INDEX IX_FornecedorCDParametro_IDFornecedorParametro
ON [dbo].[FornecedorCDParametro] ([IDFornecedorParametro],[IDCD])
INCLUDE ([vlLeadTime],[cdReviewDate],[tpWeek],[tpInterval])

CREATE NONCLUSTERED INDEX IX_FornecedorParametro_cdV9D
ON [dbo].[FornecedorParametro] ([cdV9D],[blAtivo])
INCLUDE ([IDFornecedorParametro],[cdReviewDate])

CREATE NONCLUSTERED INDEX IX_SugestaoPedido_tpStatusEnvio
ON [dbo].[SugestaoPedido] ([tpStatusEnvio],[blReturnSheet],[qtdPackCompra],[vlTipoReabastecimento])
INCLUDE ([IDSugestaoPedido],[IDItemDetalhePedido],[IdLoja],[dtPedido],[IDFornecedorParametro])
