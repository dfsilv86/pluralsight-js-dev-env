/*
DECLARE @cdOldNumber BIGINT, @dsItem NVARCHAR(50), @cdSistema SMALLINT, @idDepartamento INT, @idCategoria INT, @idSubcategoria INT, @idFineline INT, @tpStatus CHAR(1), @cdPLU BIGINT;
DECLARE @IDUsuario INT;
SET @IDUsuario = 20;
SET @idDepartamento = 17;
--*/

SELECT ID.IDItemDetalhe
     , ID.IDFineline
     , ID.IDCategoria
     , ID.IDSubcategoria
     , ID.IDDepartamento
     , ID.IDFornecedor
     , ID.cdItem
     , ID.cdOldNumber
     , ID.vlCustoUnitario
     , ID.cdSistema
     , ID.cdUPC
     , ID.dsItem
     , ID.dsHostItem
     , ID.blAtivo
     , ID.dhHostCreate
     , ID.dhHostUpdate
     , ID.blPesadoCaixa
     , ID.blPesadoRetaguarda
     , ID.cdPLU
     , ID.dsTamanhoItem
     , ID.dsCor
     , ID.dhCriacao
     , ID.dhAtualizacao
     , ID.tpStatus
     , ID.dhAtualizacaoStatus
     , ID.cdUsuarioCriacao
     , ID.cdUsuarioAtualizacao
     , ID.tpVinculado
     , ID.tpReceituario
     , ID.tpManipulado
     , ID.qtVendorPackage
     , ID.qtWarehousePackage
     , ID.vlFatorConversao
     , ID.tpUnidadeMedida
     , ID.vlTipoReabastecimento
     , ID.vlShelfLife
     , ID.blItemTransferencia
     , ID.vlModulo
     , ID.cdDepartamentoVendor
     , ID.cdSequenciaVendor         
                 , NULL AS SplitOn1
                , F.cdFornecedor
                , F.nmFornecedor
                , NULL AS SplitOn2
                , FL.cdFineLine
                , FL.dsFineLine
                , NULL AS SplitOn3
                , SC.cdSubcategoria
                , SC.dsSubcategoria
                , NULL AS SplitOn4
                , CA.cdCategoria
                , CA.dsCategoria
                , NULL AS SplitOn5
                , DE.cdDepartamento
                , DE.dsDepartamento
                , NULL AS SplitOn6
                , DI.cdDivisao
                , DI.dsDivisao
  FROM ItemDetalhe ID WITH (NOLOCK)
                   INNER JOIN Fornecedor F WITH (NOLOCK)
                           ON F.IDFornecedor = ID.IDFornecedor
                   INNER JOIN FineLine FL WITH (NOLOCK)
                           ON FL.IDFineLine = ID.IDFineLine
                   INNER JOIN Subcategoria SC WITH (NOLOCK)
                           ON SC.IDSubcategoria = ID.IDSubcategoria
                   INNER JOIN Categoria CA WITH (NOLOCK)
                           ON CA.IDCategoria = ID.IDCategoria
                   INNER JOIN Departamento DE WITH (NOLOCK)
                           ON DE.IDDepartamento = ID.IDDepartamento
                   INNER JOIN Divisao DI WITH (NOLOCK)
                           ON DI.IDDivisao = DE.IDDivisao
WHERE (@cdItem IS NULL OR ID.cdOldNumber = @cdItem)
   AND (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
   AND (@cdSistema IS NULL OR ID.cdSistema = @cdSistema)
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
   AND (@cdFineline IS NULL OR FL.cdFineLine = @cdFineline)
   AND (@tpStatus IS NULL OR ID.tpStatus = @tpStatus)
   AND (@cdPLU IS NULL OR ID.cdPLU = @cdPLU)
   AND (DE.blPerecivel = 'S')
   AND (@idRegiaoCompra IS NULL OR ID.idRegiaoCompra = @idRegiaoCompra)
   AND ID.vlTipoReabastecimento IN (20, 22, 40, 42, 43, 3, 33, 37, 7)
