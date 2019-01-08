/*
DECLARE @cdItem BIGINT, @dsItem NVARCHAR(50), @cdSistema SMALLINT, @cdDepartamento INT, @cdCategoria INT, @cdSubcategoria INT, @cdFineline INT, @tpStatus CHAR(1), @cdPLU BIGINT;
DECLARE @IDUsuario INT, @tpVinculado CHAR(1);
SET @IDUsuario = 2;
SET @cdSistema = 1;
SET @cdDepartamento = NULL;
SET @cdCategoria = NULL;
SET @cdSubcategoria = NULL;
SET @cdFineline = NULL;
SET @tpStatus = 'A';
--*/

SELECT RowConstrainedResult.*
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
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
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
					 , CASE WHEN ID.blPesadoCaixa = 1 OR ID.blPesadoRetaguarda = 1 THEN 'V' ELSE 'F' END AS tipoItem
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
				  FROM ItemDetalhe ID WITH (NOLOCK)
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
				 WHERE (@cdItem IS NULL OR ID.cdItem = @cdItem)
				   AND (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
				   AND (@cdSistema IS NULL OR ID.cdSistema = @cdSistema)
				   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
				   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
				   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
				   AND (@cdFineline IS NULL OR FL.cdFineLine = @cdFineline)
				   AND (@tpVinculado IS NULL OR ID.tpVinculado = @tpVinculado)
				   AND (@tpStatus IS NULL OR ID.tpStatus = @tpStatus)
				   AND (@cdPLU IS NULL OR ID.cdPLU = @cdPLU)
			   ) __INTERNAL
       ) AS RowConstrainedResult        
	   INNER JOIN Fornecedor F WITH (NOLOCK)
	           ON F.IDFornecedor = RowConstrainedResult.IDFornecedor
	   INNER JOIN FineLine FL WITH (NOLOCK)
	           ON FL.IDFineLine = RowConstrainedResult.IDFineLine
	   INNER JOIN Subcategoria SC WITH (NOLOCK)
	           ON SC.IDSubcategoria = RowConstrainedResult.IDSubcategoria
	   INNER JOIN Categoria CA WITH (NOLOCK)
	           ON CA.IDCategoria = RowConstrainedResult.IDCategoria
	   INNER JOIN Departamento DE WITH (NOLOCK)
	           ON DE.IDDepartamento = RowConstrainedResult.IDDepartamento
	   INNER JOIN Divisao DI WITH (NOLOCK)
	           ON DI.IDDivisao = DE.IDDivisao        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum