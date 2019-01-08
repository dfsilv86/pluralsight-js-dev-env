drop INDEX IX_ItemDetalhe_IDDepartamento2 ON [dbo].[ItemDetalhe]

CREATE NONCLUSTERED INDEX IX_ItemDetalhe_IDDepartamento2
ON [dbo].[ItemDetalhe] ([IDDepartamento],[blAtivo])
INCLUDE ([IDItemDetalhe],[IDFineline],[IDCategoria],[IDSubcategoria],[IDFornecedor],[cdItem],[cdUPC],[dsItem],[dsHostItem],[cdPLU],[dsTamanhoItem],[tpStatus],[tpVinculado],[tpReceituario],[tpManipulado],[qtVendorPackage],[vlFatorConversao],[tpUnidadeMedida],[vlTipoReabastecimento],[tpCaixaFornecedor],[vlPesoLiquido],[tpAlinhamentoCD],[idAreaCD],[idRegiaoCompra])
GO

