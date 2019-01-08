/*
DECLARE @id INT

SET @id = 1
--*/

SELECT 
	B.IDBandeira,
	B.DsBandeira,
	B.SgBandeira,
	B.BlAtivo,
	B.BlImportarTodos,
	B.IDFormato,
	B.DhCriacao,
	B.DhAtualizacao,
	B.CdUsuarioCriacao,
	B.CdUsuarioAtualizacao,
	NULL as SplitOn1,
	BD.IDBandeiraDetalhe,
	BD.IDDepartamento,
	BD.IDCategoria,
	NULL as SplitOn2,
	D.IDDepartamento,
	D.dsDepartamento,
	NULL as SplitOn3,
	c.IDCategoria,
	c.dsCategoria,
	NULL as SplitOn4,
	r.IDRegiao,
	r.nmRegiao,
	NULL as SplitOn5,
	distrito.IDDistrito,
	distrito.nmDistrito
FROM
	Bandeira B 
		LEFT JOIN BandeiraDetalhe BD WITH(NOLOCK)
			ON B.IDBandeira = BD.IDBandeira
		LEFT JOIN Departamento D WITH(NOLOCK)
			ON BD.IDDepartamento = D.IDDepartamento
		LEFT JOIN Categoria C WITH(NOLOCK)
			ON BD.IDCategoria = C.IDCategoria
		LEFT JOIN Regiao R WITH(NOLOCK)
			ON B.IDBandeira = R.IDBandeira
		LEFT JOIN Distrito WITH(NOLOCK)
			ON R.IdRegiao = Distrito.IdRegiao
WHERE
	B.IDBandeira = @id	
	