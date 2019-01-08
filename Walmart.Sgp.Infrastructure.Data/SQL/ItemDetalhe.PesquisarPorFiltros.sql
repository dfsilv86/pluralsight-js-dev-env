/*
DECLARE @cdOldNumber BIGINT, @dsItem NVARCHAR(50), @cdSistema SMALLINT, @idDepartamento INT, @idCategoria INT, @idSubcategoria INT, @idFineline INT, @tpStatus CHAR(1), @cdPLU BIGINT;
DECLARE @IDUsuario INT, @tpVinculado CHAR(1);
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
	 , F.IDFornecedor
	 , F.cdFornecedor
	 , F.nmFornecedor
	 , NULL AS SplitOn2
	 , FL.IDFineLine
	 , FL.cdFineLine
	 , FL.dsFineLine
	 , NULL AS SplitOn3
	 , SC.IDSubcategoria
	 , SC.cdSubcategoria
	 , SC.dsSubcategoria
	 , NULL AS SplitOn4
	 , CA.IDCategoria
	 , CA.cdCategoria
	 , CA.dsCategoria
	 , NULL AS SplitOn5
	 , DE.IDDepartamento
	 , DE.cdDepartamento
	 , DE.dsDepartamento
	 , NULL AS SplitOn6
	 , DI.IDDivisao
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
 WHERE (@cdOldNumber IS NULL OR ID.cdOldNumber = @cdOldNumber)
   AND (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
   AND (@cdSistema IS NULL OR ID.cdSistema = @cdSistema)
   AND (@idDepartamento IS NULL OR ID.idDepartamento = @idDepartamento)
   AND (@tpVinculado IS NULL OR ID.tpVinculado = @tpVinculado)
   AND (@idCategoria IS NULL OR ID.idCategoria = @idCategoria)
   AND (@idSubcategoria IS NULL OR ID.idSubcategoria = @idSubcategoria)
   AND (@idFineline IS NULL OR ID.idFineLine = @idFineline)
   AND (@tpStatus IS NULL OR ID.tpStatus = @tpStatus)
   AND (@cdPLU IS NULL OR ID.cdPLU = @cdPLU)