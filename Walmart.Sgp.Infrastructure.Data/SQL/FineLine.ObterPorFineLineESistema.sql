/*
DECLARE @cdFineLine INT, @cdSistema INT, @cdSubcategoria INT, @cdCategoria INT, @cdDepartamento INT, @cdDivisao INT;
SET @cdFineLine = 1;
SET @cdSistema = 1;
--*/

SELECT FI.IDFineLine
     , FI.IDSubcategoria
	 , FI.IDCategoria
	 , FI.IDDepartamento
	 , FI.cdSistema
	 , FI.cdFineLine
	 , FI.dsFineLine
	 , FI.blAtivo
	 , FI.dhCriacao
	 , FI.dhAtualizacao
	 , FI.cdUsuarioCriacao
	 , FI.cdUsuarioAtualizacao
     , NULL AS SplitOn1
     , SU.IDSubcategoria
	 , SU.cdSubcategoria
	 , SU.dsSubcategoria
	 , SU.cdSistema
	 , NULL AS SplitOn2
     , CA.IDCategoria
	 , CA.cdCategoria
	 , CA.dsCategoria
	 , CA.cdSistema
	 , CA.blPerecivel
	 , NULL AS SplitOn3
	 , DE.IDDepartamento
     , DE.dsDepartamento
	 , DE.cdDepartamento
	 , DE.IDDivisao
	 , DE.cdSistema
	 , DE.blPerecivel
	 , NULL AS SplitOn4
	 , DI.IDDivisao
	 , DI.cdDivisao
	 , DI.dsDivisao
	 , DI.cdSistema
  FROM FineLine FI WITH (NOLOCK)
       LEFT JOIN Subcategoria SU WITH (NOLOCK)
	          ON SU.IDSubcategoria = FI.IDSubcategoria
       LEFT JOIN dbo.Categoria CA WITH (NOLOCK)
	          ON CA.IDCategoria = SU.IDCategoria
       LEFT JOIN dbo.Departamento DE WITH (NOLOCK)
	          ON DE.IDDepartamento = SU.IDDepartamento
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	          ON DI.IDDivisao = DE.IDDivisao
 WHERE FI.cdFineLine = @cdFineLine
   AND (@cdSubcategoria IS NULL OR SU.cdSubcategoria = @cdSubcategoria)
   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@cdDivisao IS NULL OR DI.cdDivisao = @cdDivisao)
   AND FI.cdSistema = @cdSistema
   --AND (@dsFineline IS NULL OR FI.dsFineline LIKE '%' + @dsFineline + '%')
