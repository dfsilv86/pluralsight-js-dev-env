/*DECLARE	@stInventario INT;
DECLARE @IDLoja INT;
DECLARE @IDBandeira INT = 1;
DECLARE @IDCategoria BIGINT;
DECLARE @IDDepartamento INT;
DECLARE @DhInventario DATE;*/

SELECT		INV.IDInventario
			,INV.stInventario
			,INV.dhInventario
			,CAST(COALESCE(
			(SELECT 1
			WHERE EXISTS(
			SELECT 1
			FROM InventarioItem _II
			WHERE _II.IDInventario = INV.IDInventario
				AND _II.dhAlteracao IS NOT NULL)), 0)
			AS BIT) AS PossuiItensAlterados
			,COALESCE(Custos.totalCusto, 0) AS TotalCusto
			,COALESCE(Custos.totalCustoAprovacao, 0) AS TotalCustoAprovacao
			,COALESCE(Custos.totalCustoFinalizacao, 0) AS TotalCustoFinalizacao
			,COALESCE(Custos.totalItens, 0) AS TotalItens
			,NULL AS SplitOn1
			,Loj.IDLoja
			,Loj.cdLoja
			,Loj.nmLoja
			,NULL AS SplitOn2
			,DEP.IDDepartamento
			,DEP.cdDepartamento
			,DEP.dsDepartamento		
			,NULL AS SplitOn3
			,CAT.IDCategoria
			,CAT.cdCategoria
			,CAT.dsCategoria
FROM		Inventario Inv WITH (NOLOCK)
INNER JOIN	Loja Loj WITH (NOLOCK)
ON			Loj.IDLoja = INV.IDLoja
INNER JOIN	Departamento DEP WITH (NOLOCK)
ON			DEP.IDDepartamento = INV.IDDepartamento
LEFT JOIN	Categoria CAT
ON			INV.IDCategoria = CAT.IDCategoria
LEFT JOIN
			(SELECT		I.IDInventario
						,COUNT(1) totalItens
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCusto
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCustoFinalizacao
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCustoAprovacao
			FROM		Inventario i WITH (NOLOCK)
			INNER JOIN	InventarioItem II
			ON			I.IDInventario = II.IDInventario
			LEFT JOIN	Estoque e
			ON			e.IDLoja = i.IDLoja
			AND			e.IDItemDetalhe = ii.IDItemDetalhe
			AND			e.dtRecebimento =
						(SELECT		TOP 1 
									e2.dtRecebimento
						FROM		Estoque e2
						WHERE		e2.IDLoja = i.IDLoja
						AND			e2.IDItemDetalhe = ii.IDItemDetalhe
						AND			e2.dtRecebimento <= (
										CASE
										WHEN i.stInventario = 1 THEN i.dhInventario
										WHEN i.stInventario = 2 THEN i.dhAprovacaoLoj
										WHEN i.stInventario = 3 THEN i.dhFinalizacaoLoj
										END)
						ORDER BY	e2.dtRecebimento DESC)
			GROUP BY	i.IDInventario) Custos
ON			Custos.IDInventario = INV.IDInventario
WHERE		(@stInventario IS NULL OR INV.stInventario = @stInventario)
AND			(@IDLoja IS NULL OR INV.IDLoja = @IDLoja)
AND			(@IDBandeira IS NULL OR INV.IDBandeira = @IDBandeira)
AND			(@IDDepartamento IS NULL OR INV.IDDepartamento = @IDDepartamento)
AND			(@IDCategoria IS NULL OR INV.IDCategoria = @IDCategoria)
AND			(@DhInventario IS NULL OR (CAST(INV.dhInventario AS DATE) = CAST(@DhInventario AS DATE)))	