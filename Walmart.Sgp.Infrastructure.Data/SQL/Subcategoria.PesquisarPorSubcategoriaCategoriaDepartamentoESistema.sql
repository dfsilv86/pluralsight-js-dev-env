/*
DECLARE @cdSubcategoria INT, @dsSubcategoria NVARCHAR(50), @cdCategoria INT, @cdDepartamento INT, @cdSistema INT;
SET @cdSubcategoria = 1;
SET @dsSubcategoria = NULL;
SET @cdDepartamento = 1;
SET @cdSistema = 1;
SET @cdCategoria = 1;
--*/

SELECT SU.IDSubcategoria
     , SU.IDCategoria
	 , SU.IDDepartamento
	 , SU.cdSistema
	 , SU.cdSubcategoria
	 , SU.dsSubcategoria
	 , SU.blAtivo
	 , SU.dhCriacao
	 , SU.dhAlteracao
	 , SU.cdUsuarioCriacao
	 , SU.cdUsuarioAlteracao
	 , NULL AS SplitOn1
     --, CA.IDCategoria
	 , CA.cdCategoria
	 , CA.dsCategoria
	 , NULL AS SplitOn2
	 --, DE.IDDepartamento
     , DE.dsDepartamento
	 , DE.cdDepartamento
	 , DE.IDDivisao
	 , NULL AS SplitOn3
	 --, DI.IDDivisao
	 , DI.cdDivisao
	 , DI.dsDivisao
  FROM Subcategoria SU WITH (NOLOCK)
       LEFT JOIN dbo.Categoria CA WITH (NOLOCK)
	          ON CA.IDCategoria = SU.IDCategoria
       LEFT JOIN dbo.Departamento DE WITH (NOLOCK)
	          ON DE.IDDepartamento = SU.IDDepartamento
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	          ON DI.IDDivisao = DE.IDDivisao
 WHERE (@cdSubcategoria IS NULL OR SU.cdSubcategoria = @cdSubcategoria)
   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@cdSistema IS NULL OR CA.cdSistema = @cdSistema)
   AND (@dsSubcategoria IS NULL OR SU.dsSubcategoria LIKE '%' + @dsSubcategoria + '%')
