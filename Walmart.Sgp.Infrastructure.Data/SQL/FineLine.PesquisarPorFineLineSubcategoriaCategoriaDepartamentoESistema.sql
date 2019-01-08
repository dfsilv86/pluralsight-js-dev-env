/*
DECLARE @cdFineLine INT, @dsFineLine NVARCHAR(50), @cdSubcategoria INT, @cdCategoria INT, @cdDepartamento INT, @cdSistema INT;
SET @cdFineLine = 1;
SET @dsFineLine = NULL;
SET @cdSubcategoria = 1;
SET @cdDepartamento = 1;
SET @cdSistema = 1;
SET @cdCategoria = 1;
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
     --, SU.IDSubcategoria
	 , SU.cdSubcategoria
	 , SU.dsSubcategoria
	 , NULL AS SplitOn2
     --, CA.IDCategoria
	 , CA.cdCategoria
	 , CA.dsCategoria
	 , NULL AS SplitOn3
	 --, DE.IDDepartamento
     , DE.dsDepartamento
	 , DE.cdDepartamento
	 , DE.IDDivisao
	 , NULL AS SplitOn4
	 --, DI.IDDivisao
	 , DI.cdDivisao
	 , DI.dsDivisao
  FROM FineLine FI WITH (NOLOCK)
       LEFT JOIN Subcategoria SU WITH (NOLOCK)
	          ON SU.IDSubcategoria = FI.IDSubcategoria
       LEFT JOIN dbo.Categoria CA WITH (NOLOCK)
	          ON CA.IDCategoria = FI.IDCategoria
       LEFT JOIN dbo.Departamento DE WITH (NOLOCK)
	          ON DE.IDDepartamento = FI.IDDepartamento
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	          ON DI.IDDivisao = DE.IDDivisao
 WHERE FI.cdSistema = @cdSistema
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@cdFineLine IS NULL OR FI.cdFineLine = @cdFineline)
   AND (@cdSubcategoria IS NULL OR SU.cdSubcategoria = @cdSubcategoria)
   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
   AND (@dsFineLine IS NULL OR FI.dsFineLine LIKE '%' + @dsFineLine + '%')
