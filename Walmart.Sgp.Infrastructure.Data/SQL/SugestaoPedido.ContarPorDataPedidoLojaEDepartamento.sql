/*
DECLARE @cdSistema INT, @idLoja INT, @dtPedido DATETIME, @idDepartamento INT;

SET @dtPedido = '2016-01-22';
-- SELECT TOP 10 dtPedido FROM SugestaoPedido ORDER BY dtPedido DESC
SET @idLoja = 1033;
SET @idDepartamento = 94;
--*/

SELECT COUNT(1)
  FROM SugestaoPedido SP WITH (NOLOCK)
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
 WHERE (@dtPedido IS NULL OR SP.dtPedido = @dtPedido)
   AND (@cdSistema IS NULL OR LJ.cdSistema = @cdSistema)
   AND (@idDepartamento IS NULL OR ID.idDepartamento = @idDepartamento)
   AND (@idLoja IS NULL OR SP.idLoja = @idLoja)