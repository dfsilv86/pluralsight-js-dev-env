/*
DECLARE @cdSistema INT, @cdFornecedor INT, @nmFornecedor varchar(100);
SET @cdSistema = 1;
SET @cdFornecedor = null;
SET @nmFornecedor = 'ALIMENTOS';
--*/

SELECT *
FROM Fornecedor (nolock)
WHERE cdSistema = @cdSistema
	AND (@cdFornecedor IS NULL OR cdFornecedor = @cdFornecedor)
	AND (@nmFornecedor IS NULL OR nmFornecedor LIKE '%' + @nmFornecedor + '%')