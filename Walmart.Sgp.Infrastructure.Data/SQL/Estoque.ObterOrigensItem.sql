/*
DECLARE @IDItemDetalhe INT;
SET @IDItemDetalhe = 79470;
--*/

-- INICIO: manter sincronizado com Estoque.ObterOsCincoUltimosRecebimentosDoItemPorLoja.sql e Estoque.ObterUltimoCustoDoItemNaLoja.sql

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
)
-- FIM: manter sincronizado com Estoque.ObterOsCincoUltimosRecebimentosDoItemPorLoja.sql e Estoque.ObterUltimoCustoDoItemNaLoja.sql
SELECT IDItemDetalhe
  FROM ObterOrigemRecursiva  -- o filtro por ID está no nível 0 do ObterOrigemRecursiva, acima
OPTION (MAXRECURSION 10) 
