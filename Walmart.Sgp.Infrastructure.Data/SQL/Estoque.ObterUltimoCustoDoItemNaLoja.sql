/*
DECLARE @IDItemDetalhe INT, @IDLoja INT;
SET @IDItemDetalhe = 451617;
SET @IDLoja = 982;
--*/

-- INICIO: manter sincronizado com Estoque.ObterOrigensItem.sql e Estoque.ObterOsCincoUltimosRecebimentosDoItemPorLoja.sql

WITH Relacionamentos AS (

	SELECT RIP.cdSistema
	     , RIP.IDItemDetalhe AS IDItemPrincipal
	     , RIP.IDRelacionamentoItemPrincipal
	     , RIP.IDTipoRelacionamento
	     , RIS.IDItemDetalhe AS IDItemSecundario
	     , RIS.IDRelacionamentoItemSecundario
	  FROM RelacionamentoItemPrincipal RIP WITH (NOLOCK)
		   INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
				   ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
				   
), ObterEntradasVinculado AS (

	SELECT 0 AS IDTipoRelacionamento
	     , ID.IDItemDetalhe AS IDItemChave
	     , ID.IDItemDetalhe AS IDItemDetalhe
	  FROM ItemDetalhe ID WITH (NOLOCK)
	 WHERE ID.tpVinculado = 'S' OR (ID.tpReceituario IS NULL AND ID.tpManipulado IS NULL)
	UNION
	SELECT RPB.IDTipoRelacionamento
	     , RPB.IDItemPrincipal AS IDItemChave
	     , RPB.IDItemSecundario AS IDItemDetalhe
	  FROM Relacionamentos RPB
	       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	               ON ID.IDItemDetalhe = RPB.IDItemPrincipal
	 WHERE (ID.tpVinculado = 'S' OR (ID.tpReceituario IS NULL AND ID.tpManipulado IS NULL))
	   AND RPB.IDTipoRelacionamento = 1
	   
), ObterEntradasReceituario AS (

	SELECT 0 AS IDTipoRelacionamento
	     , ID.IDItemDetalhe AS IDItemChave
	     , ID.IDItemDetalhe AS IDItemDetalhe
	  FROM ItemDetalhe ID WITH (NOLOCK)
	 WHERE ID.tpReceituario = 'T' AND (ID.tpManipulado IS NULL OR ID.tpManipulado <> 'S')
	UNION
	SELECT RPB.IDTipoRelacionamento
	     , RPB.IDItemPrincipal AS IDItemChave
	     , RPB.IDItemSecundario AS IDItemDetalhe
	  FROM Relacionamentos RPB
	       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	               ON ID.IDItemDetalhe = RPB.IDItemPrincipal
	       INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
	               ON ID2.IDItemDetalhe = RPB.IDItemSecundario
	 WHERE (ID.tpReceituario = 'T' AND (ID.tpManipulado IS NULL OR ID.tpManipulado <> 'S'))
	   AND RPB.IDTipoRelacionamento = 2
	   AND (ID2.tpReceituario = 'T' OR ID2.tpManipulado = 'D' OR (ID2.tpVinculado = 'S' OR (ID2.tpReceituario IS NULL AND ID2.tpManipulado IS NULL)))
	   
), ObterEntradasManipulado AS (

	SELECT 0 AS IDTipoRelacionamento
	     , ID.IDItemDetalhe AS IDItemChave
	     , ID.IDItemDetalhe AS IDItemDetalhe
	  FROM ItemDetalhe ID WITH (NOLOCK)
	 WHERE ID.tpManipulado = 'D' AND (ID.tpVinculado IS NULL OR ID.tpVinculado <> 'S') AND (ID.tpReceituario IS NULL OR ID.tpReceituario <> 'T')
	UNION
	SELECT RPB.IDTipoRelacionamento
	     , RPB.IDItemSecundario AS IDItemChave
	     , RPB.IDItemPrincipal AS IDItemDetalhe
	  FROM Relacionamentos RPB
	       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	               ON ID.IDItemDetalhe = RPB.IDItemSecundario
	       INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
	               ON ID2.IDItemDetalhe = RPB.IDItemPrincipal
	 WHERE (ID.tpManipulado = 'D' AND (ID.tpVinculado IS NULL OR ID.tpVinculado <> 'S') AND (ID.tpReceituario IS NULL OR ID.tpReceituario <> 'T'))
	   AND RPB.IDTipoRelacionamento = 3
	   AND (ID2.tpReceituario = 'T' OR ID2.tpManipulado = 'D' OR (ID2.tpVinculado = 'S' OR (ID2.tpReceituario IS NULL AND ID2.tpManipulado IS NULL)))
	   
), ObterEntradas AS (
	
	SELECT *
	  FROM ObterEntradasVinculado
	UNION 
	SELECT * 
	  FROM ObterEntradasReceituario
	UNION
	SELECT *
	  FROM ObterEntradasManipulado
	  
), ObterOrigemRecursiva AS (

    SELECT 0 AS Lvl
         , O.IDItemChave AS IDItemRaiz
         , O.IDTipoRelacionamento
         , O.IDItemDetalhe
      FROM ObterEntradas O
     WHERE O.IDItemChave = @IDItemDetalhe
    UNION ALL
    SELECT ORR.Lvl + 1 AS LVL
         , ORR.IDItemRaiz
         , O.IDTipoRelacionamento
         , O.IDItemDetalhe
      FROM ObterOrigemRecursiva ORR
           INNER JOIN ObterEntradas O
                   ON O.IDItemChave = ORR.IDItemDetalhe
                  AND ORR.IDTipoRelacionamento = 2
                  AND O.IDTipoRelacionamento <> 0
    UNION ALL
    SELECT ORR.Lvl + 1 AS LVL
         , ORR.IDItemRaiz
         , O.IDTipoRelacionamento
         , O.IDItemDetalhe
      FROM ObterOrigemRecursiva ORR
           INNER JOIN ObterEntradas O
                   ON O.IDItemChave = ORR.IDItemDetalhe
                  AND ORR.IDTipoRelacionamento = 3
                  AND O.IDTipoRelacionamento <> 0

-- FIM: manter sincronizado com Estoque.ObterOrigensItem.sql e Estoque.ObterOsCincoUltimosRecebimentosDoItemPorLoja.sql

), ObterOsCincoUltimosRecebimentosDoItemPorLoja AS (

	SELECT TOP 1 NF.IDNotaFiscal
	     , NF.dtRecebimento
		 , NFI.IDNotaFiscalItem
	  FROM NotaFiscal NF WITH (NOLOCK)
		   INNER JOIN NotaFiscalItem NFI WITH (NOLOCK)
				   ON NFI.IDNotaFiscal = NF.IDNotaFiscal
	 WHERE NFI.IDItemDetalhe IN (SELECT IDItemDetalhe FROM ObterOrigemRecursiva)
	   AND NF.IDLoja = @IDLoja
     GROUP BY NF.IDNotaFiscal
            , NF.dtRecebimento
		    , NFI.IDNotaFiscalItem
     ORDER BY NF.dtRecebimento DESC
     
), UltimoEstoque AS (

	SELECT TOP 1 * FROM Estoque WHERE IDItemDetalhe = @IDItemDetalhe AND IDLoja = @IDLoja ORDER BY dtRecebimento DESC
)
SELECT TOP 1 NF.IDNotaFiscal
     , NF.nrNotaFiscal
     , NF.dtRecebimento
     , NF.dtEmissao
     , NF.IDLoja
     , NULL AS SplitOn1
	 , LJ.IDLoja
     , LJ.cdLoja
     , LJ.nmLoja
     , NULL AS SplitOn2
	 , NFI.IDNotaFiscalItem
	 , NFI.IDItemDetalhe
     , NFI.vlCusto
     , NULL AS SplitOn3
	 , ID.IDItemDetalhe
     , ID.cdItem
     , ID.dsItem
	 , NULL AS SplitOn4
	 , EE.IDEstoque
	 , EE.vlCustoCadastroAtual
	 , EE.dtCustoCadastro
	 , EE.vlCustoContabilAtual
	 , EE.dtRecebimento
	 , EE.blCustoCadastro
  FROM ObterOsCincoUltimosRecebimentosDoItemPorLoja OOC
       INNER JOIN NotaFiscal NF WITH (NOLOCK)
               ON NF.IDNotaFiscal = OOC.IDNotaFiscal
       INNER JOIN Loja LJ WITH (NOLOCK)
               ON LJ.IDLoja = NF.IDLoja
       INNER JOIN NotaFiscalItem NFI WITH (NOLOCK)
               ON NFI.IDNotaFiscalItem = OOC.IDNotaFiscalItem
              AND NFI.IDNotaFiscal = NF.IDNotaFiscal
       INNER JOIN ItemDetalhe ID WITH (NOLOCK)
               ON ID.IDItemDetalhe = NFI.IDItemDetalhe
	    LEFT JOIN UltimoEstoque EE WITH (NOLOCK)
		       ON EE.IDItemDetalhe = NFI.IDItemDetalhe
