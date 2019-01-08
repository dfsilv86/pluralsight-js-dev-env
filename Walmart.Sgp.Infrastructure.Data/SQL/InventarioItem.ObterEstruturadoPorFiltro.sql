/*
DECLARE @idInventario INT = 98989;
DECLARE @cdOldNumber INT;
DECLARE @dsItem VARCHAR(200);
DECLARE @cdPlu INT;
DECLARE @filtro INT;
*/

SELECT      I.*
			,ISNULL(E.VlCUstoContabilAtual, 0) * I.qtItem AS vlCustoTotal
			,(	SELECT		TOP 1
							_II.qtItem
				FROM		InventarioItem _II WITH (NOLOCK)
				INNER JOIN	Inventario _I WITH (NOLOCK)
				ON			_II.IDInventario = _I.IDInventario
				WHERE		_II.IDItemDetalhe = ID.IDItemDetalhe
				AND			_I.dhInventario < INV.dhInventario
				ORDER BY	_I.dhInventario DESC) AS qtItemInventarioAnterior 
			,NULL AS SplitOn1
			,ID.cdOldNumber
			,ID.dsItem
			,ID.cdPLU
			,ID.tpStatus
			,ID.cdItem
			,NULL AS SplitOn2
			,E.vlCustoContabilAtual
			,E.vlCustoCadastroAtual	
FROM        InventarioItem I WITH (NOLOCK)
INNER JOIN  ItemDetalhe ID WITH (NOLOCK)
ON          ID.IDItemDetalhe = I.IDItemDetalhe
INNER JOIN  Inventario INV WITH (NOLOCK)
ON          INV.IDInventario = I.IDInventario
LEFT JOIN   Estoque E WITH (NOLOCK)
ON          E.IDLoja = INV.IDLoja
AND         E.IDItemDetalhe = I.IDItemDetalhe
AND         E.dtRecebimento = ( SELECT      TOP 1
											E2.dtRecebimento
								FROM        Estoque E2 WITH (NOLOCK)
								WHERE       E2.IDLoja = INV.IDLoja
								AND         E2.IDItemDetalhe = I.IDItemDetalhe
								AND         E2.dtRecebimento <= INV.dhInventario
								ORDER BY    E2.dtRecebimento DESC)
WHERE       I.IDInventario = @idInventario
AND         (@cdOldNumber IS NULL OR ID.cdOldNumber = @cdOldNumber)
AND         (@cdPlu IS NULL OR ID.cdPLU = @cdPlu)
AND         (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
AND         (COALESCE(@filtro, 0) = 0 OR
(
-- ITENS COM CUSTO ZERO
			(@filtro = 1 AND COALESCE(E.VlCustoContabilAtual, 0) = 0)
OR          (@filtro = 2 AND COALESCE(E.VlCustoCadastroAtual, 0) <> 0)
-- ITENS MODIFICADOS APÓS IMPORTACAO
OR          (@filtro = 3 AND I.dhAlteracao IS NOT NULL)
-- ITENS INATIVOS/DELETADOS QUE ESTÃO NO INVENTÁRIO
OR          (@filtro = 4 AND ID.tpStatus IN ('D', 'I'))
-- ITENS VINCULADOS DE ENTRADA QUE ESTÃO NO INVENTÁRIO
OR          (@filtro = 5 AND ID.tpVinculado = 'E')
-- ITENS QUE ESTÃO NO INVENTÁRIO E NÃO ESTÃO NO SORTIMENTO DA LOJA
OR          (@filtro = 6 AND NOT EXISTS(SELECT  TOP 1
												1
										FROM    Trait T WITH (NOLOCK)
										WHERE   T.IDItemDetalhe = I.IDItemDetalhe
										AND     T.IDLoja = INV.IDLoja))

))
/*ORDER BY    (COALESCE(E.VlCUstoContabilAtual, 0) * I.qtItem) DESC*/