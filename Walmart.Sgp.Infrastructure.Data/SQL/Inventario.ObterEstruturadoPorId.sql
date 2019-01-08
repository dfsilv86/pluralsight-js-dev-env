/*
DEClARE @id INT
SET @id = 76533
--*/

SELECT		I.IDInventario
			,I.dhInventario
			,I.dhAberturaLoj
			,I.dhImportacao
			,I.dhFinalizacaoLoj
			,I.dhAprovacaoLoj
			,I.stInventario
			,COALESCE(Custos.totalCusto, 0) AS TotalCusto
			,COALESCE(Custos.totalCustoAprovacao, 0) AS TotalCustoAprovacao
			,COALESCE(Custos.totalCustoFinalizacao, 0) AS TotalCustoFinalizacao
			,COALESCE(Custos.totalItens, 0) AS TotalItens
			,NULL AS SplitOn1
			,L.IDLoja
			,L.nmLoja
			,L.cdLoja
			,L.cdSistema
			,NULL AS SplitOn2
			,B.IDBandeira
			,B.cdSistema
			,B.dsBandeira
			,NULL AS SplitOn3
			,D.IDDepartamento
			,D.cdDepartamento
			,D.dsDepartamento
			,D.blPerecivel      -- usado na importacao automatica
			,NULL AS SplitOn4
			,C.IDCategoria
			,C.cdCategoria
			,C.dsCategoria
			,NULL AS SplitOn5
			,UAL.Id AS UsuarioAberturaLojaId
			,UAL.Username as UsuarioAberturaLojaUsername
			,UI.Id AS UsuarioImportacaoId
			,UI.Username AS UsuarioImportacaoUsername
			,UFL.Id AS UsuarioFinalizacaoLojaId
			,UFL.Username AS UsuarioFinalizacaoLojaUsername
			,UAPL.Id AS UsuarioAprovacaoLojaId	
			,UAPL.Username AS UsuarioAprovacaoLojaUsername				
FROM		Inventario I WITH (NOLOCK)
LEFT JOIN
			(SELECT		_I.IDInventario
						,COUNT(1) totalItens
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCusto
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCustoFinalizacao
						,CONVERT(DECIMAL(18, 2), SUM(ISNULL(e.vlCustoContabilAtual, 0) * ISNULL(ii.qtItem, 0))) AS totalCustoAprovacao
			FROM		Inventario _I WITH (NOLOCK)
			INNER JOIN	InventarioItem II WITH (NOLOCK)
			ON			_I.IDInventario = II.IDInventario
			LEFT JOIN	Estoque e WITH (NOLOCK)
			ON			e.IDLoja = _I.IDLoja
			AND			e.IDItemDetalhe = ii.IDItemDetalhe
			AND			e.dtRecebimento =
						(SELECT		TOP 1 
									e2.dtRecebimento
						FROM		Estoque e2 WITH (NOLOCK)
						WHERE		e2.IDLoja = _I.IDLoja
						AND			e2.IDItemDetalhe = ii.IDItemDetalhe
						AND			e2.dtRecebimento <= (
										CASE
										WHEN _I.stInventario = 1 THEN _I.dhInventario
										WHEN _I.stInventario = 2 THEN _I.dhAprovacaoLoj
										WHEN _I.stInventario = 3 THEN _I.dhFinalizacaoLoj
										END)
						ORDER BY	e2.dtRecebimento DESC)
			GROUP BY	_I.IDInventario) Custos
ON			Custos.IDInventario = I.IDInventario
INNER JOIN	Loja L WITH (NOLOCK)
ON			L.IDLoja = I.IDLoja
INNER JOIN	Bandeira B WITH (NOLOCK)
ON			B.IDBandeira = I.IDBandeira
LEFT JOIN	Departamento D WITH (NOLOCK)
ON			D.IDDepartamento = I.IDDepartamento
LEFT JOIN	Categoria C WITH (NOLOCK)
ON			C.IDCategoria = I.IDCategoria
LEFT JOIN	CWIUser UAL WITH (NOLOCK)
ON			UAL.Id = I.cdUsuarioAberturaLoj
LEFT JOIN	CWIUser UI WITH (NOLOCK)
ON			UI.Id = I.cdUsuarioImportacao
LEFT JOIN	CWIUser UFL WITH (NOLOCK)
ON			UFL.Id = I.cdUsuarioFinalizacaoLoj
LEFT JOIN	CWIUser UAPL WITH (NOLOCK)
ON			UAPL.Id = I.cdUsuarioAprovacaoLoj
WHERE		I.IDInventario = @id