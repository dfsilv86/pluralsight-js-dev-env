/*
DECLARE @cdSistema INT, @cdV9D INT, @nmFornecedor VARCHAR(50);
SET @cdSistema = 1;
--*/

WITH FornecedorParametroPerecivel AS 
(
	SELECT 
	   IdFornecedor,
	   cdSistema,
	   cdV9D,
	   cdTipo
	FROM
		FornecedorParametro FP
	WHERE
		EXISTS 
		(
			SELECT 
				1
			FROM
				Departamento D WITH(NOLOCK)				
			WHERE
				-- TODO: quando for feito merge com o PESS utilizar a coluna IDDepartamento de FornecedorParametro
				D.cdDepartamento = LEFT(RIGHT(FP.cdV9D, 3), 2)
			AND D.cdSistema = @cdSistema
			AND D.blPerecivel = 'S'
		)	
)

SELECT 
	   count (1)
  FROM FornecedorParametroPerecivel FP WITH (NOLOCK) 
       INNER JOIN Fornecedor F WITH (NOLOCK) 
	           ON FP.IdFornecedor = F.IdFornecedor 
 WHERE FP.cdSistema = @cdSistema
   AND (@cdV9D IS NULL OR FP.cdV9D = @cdV9D)
   AND (@cdTipo IS NULL OR FP.cdTipo = @cdTipo)
   AND (@nmFornecedor IS NULL OR F.nmFornecedor LIKE '%' + @nmFornecedor + '%')