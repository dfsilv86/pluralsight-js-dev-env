/*
DECLARE @cdItemSaida INT, @dsEstado VARCHAR, @IDRegiaoCompra INT, @IDBandeira INT, @blVinculado BIT, @IDUsuario INT,
@TipoPermissao INT, @cdSistema INT, @idFornecedorParametro INT, @cdV9D INT;

SET @cdItemSaida = NULL; --9488705
SET @dsEstado = NULL;
SET @IDRegiaoCompra = NULL;
SET @IDBandeira = NULL;
SET @blVinculado = 0;
SET @IDUsuario = 2337;
SET @TipoPermissao = 2;
SET @cdV9D = 616750800;
--SET @cdSistema = 1;
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
	SELECT 
		L.IDLoja,
		L.cdLoja,
		L.nmLoja,
		L.dsEstado,
		L.IDBandeira
	  FROM Loja L WITH (NOLOCK)
	 WHERE 
	   (@dsEstado IS NULL OR L.dsEstado = @dsEstado)
	   AND (@IDBandeira IS NULL OR L.IDBandeira = @IDBandeira)
	   AND EXISTS (SELECT TOP 1 1 FROM Lojas LS WITH (NOLOCK) WHERE LS.IDLoja = L.IdLoja)
)

SELECT
	CD.cdCD,
	CASE
		WHEN CD.blConvertido = 1
		THEN (SELECT IDN.cdItem 
			  FROM PESSItemXref PIX
			  INNER JOIN ItemDetalhe IDN WITH(NOLOCK)
				ON IDN.IDItemDetalhe = PIX.IdItemDetalheNovo 
			  WHERE IdItemDetalheAntigo = IDE.IDItemDetalhe)
		ELSE NULL
	END AS cdItemPESS,
	L.cdLoja,
	L.nmLoja,
	NULL AS SplitOn1,
	IDE.cdItem AS CdItemEntrada,
	IDE.dsItem AS DsItemEntrada,
	NULL AS SplitOn2,
	D.cdDepartamento,
	D.dsDepartamento,
	NULL AS SplitOn3,
	IDS.cdItem, 
	IDS.dsItem,
	NULL AS SplitOn4,
	FE.nmFornecedor,
	NULL AS SplitOn5,
	FPE.cdV9D
FROM RelacaoItemLojaCD RILC WITH(NOLOCK)
INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
	ON LCP.IDLojaCDParametro = RILC.IDLojaCDParametro
INNER JOIN CD CD WITH(NOLOCK)
	ON CD.IDCD = LCP.IDCD
INNER JOIN LojasFiltradas L WITH(NOLOCK)
	ON L.IDLoja = LCP.IDLoja
INNER JOIN Bandeira B WITH(NOLOCK)
	ON B.IDBandeira = L.IDBandeira
INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
	ON IDS.IDItemDetalhe = RILC.IDItem
LEFT JOIN RegiaoCompra RC WITH(NOLOCK)
	ON RC.IDRegiaoCompra = IDS.IDRegiaoCompra
INNER JOIN Trait T WITH(NOLOCK)
	ON T.IdLoja = LCP.IDLoja AND T.IdItemDetalhe = RILC.IDItem
LEFT JOIN ItemDetalhe IDE WITH(NOLOCK)
	ON IDE.IDItemDetalhe = RILC.IdItemEntrada
LEFT JOIN Fornecedor FE WITH(NOLOCK)
	ON FE.IDFornecedor = IDE.IDFornecedor
LEFT JOIN FornecedorParametro FPE WITH(NOLOCK)
	ON FPE.IDFornecedorParametro = IDE.idFornecedorParametro
INNER JOIN Departamento D WITH(NOLOCK)
	ON D.IDDepartamento = IDS.IDDepartamento
WHERE IDS.tpVinculado = 'S'
	AND (@cdItemSaida IS NULL OR (IDS.cdItem = @cdItemSaida))	
	AND (@IDRegiaoCompra IS NULL OR RC.IdRegiaoCompra = @IDRegiaoCompra)	
	AND ((@blVinculado = 1 AND RILC.IdItemEntrada IS NULL) OR @blVinculado = 0)
	AND (@idFornecedorParametro IS NULL OR IDE.idFornecedorParametro = @idFornecedorParametro)
	AND (@cdV9D IS NULL OR FPE.cdV9D = @cdV9D)
	AND (@cdSistema IS NULL OR IDS.cdSistema = @cdSistema)
	AND EXISTS(SELECT 1						
		FROM ItemDetalhe IDE2 WITH (NOLOCK)
			INNER JOIN Fornecedor F2 WITH (NOLOCK)
					ON F2.IDFornecedor = IDE2.IDFornecedor
			INNER JOIN RelacionamentoItemSecundario RIS2 WITH(NOLOCK)
					ON RIS2.IDItemDetalhe = IDE2.IDItemDetalhe
			INNER JOIN RelacionamentoItemPrincipal RIP2 WITH(NOLOCK)
					ON RIP2.IDRelacionamentoItemPrincipal = RIS2.IDRelacionamentoItemPrincipal
					AND RIP2.IDTipoRelacionamento = 1
			INNER JOIN ItemDetalhe IDS2 WITH(NOLOCK)
					ON IDS2.IDItemDetalhe = RIP2.IDItemDetalhe
			INNER JOIN Trait TE2 WITH(NOLOCK)
					ON TE2.IDItemDetalhe = IDE2.IDItemDetalhe
			INNER JOIN Trait TS2 WITH(NOLOCK)
					ON TS2.IDLoja = TE2.IDLoja
					AND TS2.IDItemDetalhe = IDS2.IDItemDetalhe
			INNER JOIN Loja L2 WITH(NOLOCK)
					ON L2.IDLoja = TE2.IDLoja
			LEFT JOIN FornecedorParametro FP2 WITH(NOLOCK)
					ON FP2.IDFornecedorParametro = IDE2.IDFornecedorParametro
		WHERE IDE2.tpVinculado = 'E'
			AND IDS2.cdItem = IDS.cdItem
			AND L2.cdLoja = L.cdLoja
			AND IDE2.vlTipoReabastecimento IS NOT NULL
			AND IDE2.blItemTransferencia = 0
			AND IDE2.IdFornecedorParametro IS NOT NULL
			AND (@idFornecedorParametro IS NULL OR IDE2.idFornecedorParametro = @idFornecedorParametro))