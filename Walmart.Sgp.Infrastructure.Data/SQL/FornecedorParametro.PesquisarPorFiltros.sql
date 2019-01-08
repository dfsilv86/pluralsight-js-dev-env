 /*
 DECLARE	@cdSistema INT = 1;
 DECLARE	@cdV9D BIGINT = NULL;
 DECLARE	@stFornecedor VARCHAR(1) = NULL;
 DECLARE	@nmFornecedor VARCHAR(50) = NULL;
 --*/
 
 SELECT     DISTINCT 
			FoPa.IDFornecedor		
			,FoPa.IDFornecedorParametro
			,FoPa.cdV9D
			,FoPa.cdTipo
            ,FoPa.cdStatusVendor
            ,FoPa.tpStoreApprovalRequired
            ,NULL AS SplitOn1
            ,Forn.nmFornecedor
            ,Forn.stFornecedor            
FROM
	Fornecedor Forn WITH (NOLOCK)
		INNER JOIN	FornecedorParametro FoPa WITH (NOLOCK) 
			ON FoPa.IDFornecedor = Forn.IDFornecedor 
		INNER JOIN	Departamento Depa  WITH (NOLOCK)
			-- TODO: quando for feito merge com o PESS utilizar a coluna IDDepartamento de FornecedorParametro
			ON Depa.cdDepartamento = LEFT(RIGHT(FoPa.cdV9D, 3), 2)
WHERE		
	Forn.cdSistema = @cdSistema
AND	Depa.cdSistema = @cdSistema
AND	Depa.blPerecivel = 'S'
AND	(@stFornecedor IS NULL
	OR	Forn.stFornecedor = @stFornecedor)
AND	(@cdV9D IS NULL
	OR	FoPa.cdV9D = @cdV9D)
AND	(@nmFornecedor IS NULL
OR	Forn.nmFornecedor LIKE '%' + @nmFornecedor + '%')