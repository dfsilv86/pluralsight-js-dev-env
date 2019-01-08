

/*
DECLARE @IDUsuario INT, @idBandeira INT, @cdSistema INT, @TipoPermissao INT;
DECLARE @cdLoja INT, @dtPedido DATETIME;
DECLARE @cdItem INT, @cdFineline INT, @cdSubcategoria INT, @cdCategoria INT, @cdDepartamento INT, @cdDivisao INT;
DECLARE @cdOrigemCalculo INT, @cdVendor INT;

SET @IDUsuario = 20;
SET @TipoPermissao = 2;
SET @dtPedido = '2016-01-22';
-- SELECT TOP 10 dtPedido FROM SugestaoPedido ORDER BY dtPedido DESC
SET @cdLoja = 1033;
SET @cdDepartamento = 94;
--*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Lojas AS (
	-- Busca conforme permissao por bandeira
	SELECT L.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
				  AND (@idBandeira IS NULL OR PB.IDBandeira = @idBandeira)
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDBandeira = PB.IDBandeira
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 2)
	UNION
	-- Busca conforme permissao por loja
	SELECT PL.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND (@idBandeira IS NULL OR L.IDBandeira = @idBandeira)
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
), LojasFiltradas AS (
	SELECT L.IDLoja
	  FROM Loja L WITH (NOLOCK)
	 WHERE L.cdLoja = @cdLoja
	   AND EXISTS (SELECT TOP 1 1 FROM Lojas LS WITH (NOLOCK) WHERE LS.IDLoja = L.IdLoja)
)
SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (

SELECT SP.IDSugestaoPedido
     , SP.IDItemDetalhePedido
     , SP.IDItemDetalheSugestao
     , SP.IdLoja
     , SP.dtPedido
     --, SP.tpWeek
     --, SP.tpInterval
     --, SP.cdReviewDate
     --, SP.vlLeadTime
     , SP.qtVendorPackage
     --, SP.dtProximoReviewDate
     , SP.dtInicioForecast
     , SP.dtFimForecast
     --, SP.vlEstoqueSeguranca
     --, SP.vlShelfLife
     --, SP.vlLeadTimeReal
     --, SP.blAtendePedidoMinimo
     , SP.IDFornecedorParametro
     , SP.qtdPackCompra
     , SP.qtdPackCompraOriginal
     , SP.cdOrigemCalculo
     --, SP.vlPackSugerido1
     , SP.vlModulo
     , SP.vlEstoque
     , SP.vlTotalPedidosAberto
     --, SP.vlPipeline
     , SP.vlForecast
     , SP.vlForecastMedio
     , SP.vlEstoqueSegurancaQtd
     --, SP.vlQtdDiasEstoque
     --, SP.vlSugestaoPedido
     , SP.vlEstoqueOriginal
     , SP.vlFatorConversao
     , SP.blPossuiVendasUltimaSemana
     --, SP.tpStatusEnvio
     --, SP.dhEnvioSugestao
	 , ISNULL(sp.blReturnSheet, 0) as blReturnSheet
	 , ISNULL(sp.blCDConvertido, 0) as blCDConvertido
	 , ISNULL(sp.TpCaixaFornecedor, 'F') as TpCaixaFornecedor
	 , sp.vlPesoLiquido
	 , SP.vlSaldoOO
	 , SP.vlSaldoIW
	 , SP.vlSaldoIT
	 , SP.vlTipoReabastecimento
     , NULL AS SplitOn1
     , FP.cdV9D
     , FP.cdTipo
     , NULL AS SplitOn2
     , FO.nmFornecedor
     , NULL AS SplitOn3
     , ID.cdItem
     , ID.dsItem
     , NULL AS SplitOn4
     , FL.cdFineLine
     , FL.dsFineLine
	 , NULL AS SplitOn5
	 , LJ.IDBandeira
	 , NULL AS SplitOn6
	 , ID2.cdSistema
	 , ID2.IDDepartamento
  FROM SugestaoPedido SP WITH (NOLOCK)
       INNER JOIN LojasFiltradas LSF
               ON LSF.IDLoja = SP.IdLoja
       INNER JOIN Loja LJ WITH (NOLOCK)
               ON LJ.IDLoja = SP.IdLoja
       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
               ON ID.IDItemDetalhe = SP.IDItemDetalheSugestao
              AND ID.cdSistema = LJ.cdSistema
              AND ID.vlTipoReabastecimento IN (20, 22, 40, 42, 43, 3, 33, 37, 7) -- Personalizadas\SugestaoPedidoData.cs linha 236
       INNER JOIN FornecedorParametro FP WITH (NOLOCK)
               ON FP.IDFornecedorParametro = SP.IDFornecedorParametro
              AND FP.cdSistema = LJ.cdSistema
              AND FP.tpStoreApprovalRequired IN ('Y', 'R')                       -- Personalizadas\SugestaoPedidoData.cs linha 287
       INNER JOIN Fornecedor FO WITH (NOLOCK)
               ON FO.IDFornecedor = FP.IDFornecedor
       INNER JOIN Fineline FL WITH (NOLOCK)
               ON FL.IDFineLine = ID.IDFineline
              AND FL.cdSistema = ID.cdSistema
       INNER JOIN Subcategoria SC WITH (NOLOCK)
               ON SC.IDSubcategoria = ID.IDSubcategoria
              AND SC.cdSistema = ID.cdSistema
       INNER JOIN Categoria CA WITH (NOLOCK)
               ON CA.IDCategoria = ID.IDCategoria
              AND CA.cdSistema = ID.cdSistema
       INNER JOIN Departamento DE WITH (NOLOCK)
               ON DE.IDDepartamento = ID.IDDepartamento
              AND DE.cdSistema = ID.cdSistema
              AND DE.blPerecivel = 'S'                                           -- Personalizadas\SugestaoPedidoData.cs linha 282
       INNER JOIN Divisao DI WITH (NOLOCK)
               ON DI.IDDivisao = DE.IDDivisao
              AND DI.cdSistema = ID.cdSistema
	   INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
	           ON ID2.IDItemDetalhe = SP.IDItemDetalhePedido
			  AND ID2.cdSistema = LJ.cdSistema
       /*INNER JOIN Fornecedor VD WITH (NOLOCK)
               ON VD.IDFornecedor = ID.IDFornecedor*/
                         
 WHERE SP.dtPedido = @dtPedido
   AND LJ.cdSistema = @cdSistema
   AND DE.cdDepartamento = @cdDepartamento
   AND (@cdDivisao IS NULL OR DI.cdDivisao = @cdDivisao)   
   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
   AND (@cdFineline IS NULL OR FL.cdFineLine = @cdFineline)
   AND (@cdItem IS NULL OR ID.cdItem = @cdItem)
   AND (@cdOrigemCalculo IS NULL OR SP.cdOrigemCalculo = @cdOrigemCalculo)
   AND (@cdVendor IS NULL OR  FP.cdV9D = @cdVendor) 
   
			   ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum