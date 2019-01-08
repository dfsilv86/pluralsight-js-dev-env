/*
DECLARE @cdSistema INT, @cdV9D INT, @cdTipo NVARCHAR(1), @nmFornecedor VARCHAR(50);
SET @cdSistema = 1;
SET @cdV9D = 43690940;
SET @cdTipo = 'L';
--*/

WITH FornecedorParametroPerecivel AS 
(
	SELECT 
	   FP.IDFornecedorParametro
     , FP.IDFornecedor
     , FP.cdV9D
     , FP.blAtivo
     , FP.dhCriacao
     , FP.dhAtualizacao
     , FP.cdUsuarioCriacao
     , FP.cdUsuarioAtualizacao
     , FP.cdSistema
     , FP.vlLeadTime
     , FP.vlFillRate
     , FP.tpPedidoMinimo
     , FP.vlValorMinimo
     , FP.tpWeek
     , FP.tpInterval
     , FP.cdReviewDate
     , FP.cdTipo
     , FP.cdStatusVendor
     , FP.tpStoreApprovalRequired
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

SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
			SELECT 
					FP.*
					, NULL AS SplitOn1
					, F.nmFornecedor
					, F.cdFornecedor
				FROM FornecedorParametroPerecivel FP WITH (NOLOCK) 
					INNER JOIN Fornecedor F WITH (NOLOCK) 
							ON FP.IdFornecedor = F.IdFornecedor 
				WHERE FP.cdSistema = @cdSistema
				AND (@cdV9D IS NULL OR FP.cdV9D = @cdV9D)
				AND (@cdTipo IS NULL OR FP.cdTipo = @cdTipo)
				AND (@nmFornecedor IS NULL OR F.nmFornecedor LIKE '%' + @nmFornecedor + '%')
			   ) __INTERNAL
       ) AS RowConstrainedResult
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum