/*
DECLARE @cdPLU INT, @cdSistema INT;
SET @cdPLU = 12345;
SET @cdSistema = 1;
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
 WHERE ID.cdPLU = @cdPLU
   AND ID.cdSistema = @cdSistema
