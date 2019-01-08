/*
DECLARE @cdDepartamento INT, @cdSistema INT, @dsCategoria NVARCHAR(50), @cdCategoria INT;
SET @cdDepartamento = 1;
SET @cdSistema = 1;
SET @dsCategoria = NULL;
SET @cdCategoria = 1;
--*/

SELECT CA.IDCategoria
     , CA.IDDepartamento
	 , CA.cdSistema
	 , CA.cdCategoria
	 , CA.dsCategoria
	 , CA.blPerecivel
	 , CA.blAtivo
	 , CA.dhCriacao
	 , CA.dhAtualizacao
	 , CA.cdUsuarioCriacao
	 , CA.cdUsuarioAtualizacao
	 , NULL AS SplitOn1
     , DE.dsDepartamento
	 , DE.cdDepartamento
	 , DE.IDDivisao
	 , NULL AS SplitOn2
	 , DI.cdDivisao
	 , DI.dsDivisao
  FROM dbo.Categoria CA WITH (NOLOCK)
       LEFT JOIN dbo.Departamento DE WITH (NOLOCK)
	          ON DE.IDDepartamento = CA.IDDepartamento
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	          ON DI.IDDivisao = DE.IDDivisao
 WHERE 
	   DE.cdDepartamento = ISNULL(@cdDepartamento, DE.cdDepartamento) 
   AND CA.cdSistema = @cdSistema
   AND CA.cdCategoria = ISNULL(@cdCategoria, CA.cdCategoria)
   AND (@dsCategoria IS NULL OR CA.dsCategoria LIKE '%' + @dsCategoria + '%')
