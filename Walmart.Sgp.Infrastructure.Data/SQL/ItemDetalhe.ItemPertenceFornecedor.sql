
/*
DECLARE @cdItem BIGINT = 9408269
DECLARE @cdV9D BIGINT = 514425940
DECLARE @cdSistema INT = 1
*/

	SELECT COUNT(*)
      FROM ItemDetalhe IDD WITH(NOLOCK)
INNER JOIN FornecedorParametro FP WITH(NOLOCK)
		ON FP.IDFornecedorParametro = IDD.IDFornecedorParametro
	 WHERE IDD.cdItem = @cdItem
	   AND IDD.cdSistema = @cdSistema
	   AND FP.cdV9D = @cdV9D