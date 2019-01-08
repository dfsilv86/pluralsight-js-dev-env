/*
DECLARE @idUsuario INT, @dhInclusaoInicio DATE, @dhInclusaoFim DATE, @idBandeira INT, @idCategoria INT, @idDepartamento INT, @idLoja INT
SET @idUsuario = 2337;
SET @dhInclusaoInicio = '2016-01-31 00:00:00'
SET @dhInclusaoFim = '2016-12-31 23:59:59'
SET @idBandeira = 1;
SET @idCategoria = NULL;
SET @idDepartamento = NULL;
SET @idLoja = NULL;
*/

SELECT DISTINCT
	IC.IDInventarioCritica,
	IC.IDInventario,
	IC.dhInclusao,
	IC.dhInventario,
	IC.dsCritica,
	NULL AS SplitOn1,
	ICT.IDInventarioCriticaTipo,
	ICT.nmTipo,
	NULL AS SplitOn2,
	L.IDLoja,
	L.cdLoja,
	L.nmLoja,
	NULL AS SplitOn3,
	D.IDDepartamento,
	D.cdDepartamento,
	D.dsDepartamento,
	NULL AS SplitOn4,
	C.IDCategoria,
	C.cdCategoria,
	C.dsCategoria
FROM 
	InventarioCritica IC WITH (NOLOCK)
		LEFT JOIN InventarioCriticaTipo ICT WITH (NOLOCK)
			ON IC.IDInventarioCriticaTipo = ICT.IDInventarioCriticaTipo
		LEFT JOIN Departamento D WITH (NOLOCK)
			ON IC.IDDepartamento = D.IDDepartamento
		LEFT JOIN Categoria C WITH (NOLOCK)
			ON IC.IDCategoria = C.IDCategoria
		LEFT JOIN Loja L WITH (NOLOCK)
			ON IC.IDLoja = L.IDLoja
		LEFT JOIN PermissaoBandeira PB WITH (NOLOCK)
			ON L.IDBandeira = PB.IDBandeira
		LEFT JOIN Permissao PPB WITH (NOLOCK)
			ON PB.IDPermissao = PPB.IDPermissao
		LEFT JOIN PermissaoLoja PL WITH (NOLOCK)
			ON L.IDLoja = PL.IDLoja
		LEFT JOIN Permissao PPL WITH (NOLOCK)
			ON PL.IDPermissao = PPL.IDPermissao
WHERE 
	(PPB.IDUsuario = @idUsuario OR PPL.IDUsuario = @idUsuario)
AND IC.blAtivo = 1
AND ICT.blVisualizaHO = 1
AND l.IDBandeira = @idBandeira
AND IC.IDLoja = ISNULL(@idLoja, IC.IDLoja)
AND IC.IDDepartamento = ISNULL(@idDepartamento, IC.IDDepartamento)
AND IC.IDCategoria = ISNULL(@idCategoria, IC.IDCategoria)
AND IC.dhInclusao >= @dhInclusaoInicio AND IC.dhInclusao < @dhInclusaoFim