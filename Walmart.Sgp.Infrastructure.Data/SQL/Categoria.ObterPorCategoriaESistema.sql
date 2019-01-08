/*
DECLARE @cdDepartamento INT, @cdSistema INT, @cdCategoria INT, @cdDivisao INT;
SET @cdDepartamento = 1;
SET @cdSistema = 1;
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
	 , DE.IDDepartamento
     , DE.dsDepartamento
	 , DE.cdDepartamento
	 , DE.IDDivisao
	 , DE.cdSistema
	 , DE.blPerecivel
	 , NULL AS SplitOn2
	 , DI.IDDivisao
	 , DI.cdDivisao
	 , DI.dsDivisao
	 , DI.cdSistema
  FROM dbo.Categoria CA WITH (NOLOCK)
       LEFT JOIN dbo.Departamento DE WITH (NOLOCK)
	          ON DE.IDDepartamento = CA.IDDepartamento
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	         ON DI.IDDivisao = DE.IDDivisao
 WHERE CA.cdCategoria = @cdCategoria
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@cdDivisao IS NULL OR DI.cdDivisao = @cdDivisao)
   AND CA.cdSistema = @cdSistema
