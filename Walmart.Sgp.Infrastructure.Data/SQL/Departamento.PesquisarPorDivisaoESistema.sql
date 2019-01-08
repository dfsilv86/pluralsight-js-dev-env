/*
DECLARE @cdDepartamento INT, @cdSistema INT, @dsDepartamento NVARCHAR(50), @blPerecivel CHAR(1), @cdDivisao INT;
SET @cdDepartamento = NULL;
SET @cdSistema = 1;
SET @dsDepartamento = NULL;
SET @blPerecivel = NULL;
SET @cdDivisao = NULL;
--*/

SELECT DE.IDDepartamento
     , DE.IDDivisao
	 , DE.cdSistema
	 , DE.cdDepartamento
	 , DE.dsDepartamento
	 , DE.blPerecivel
	 , DE.blAtivo
	 , DE.dhCriacao
	 , DE.dhAtualizacao
	 , DE.cdUsuarioCriacao
	 , DE.cdUsuarioAtualizacao
	 , DE.pcDivergenciaNF
	 , NULL AS SplitOn
	 , DI.cdDivisao
	 , DI.dsDivisao
  FROM dbo.Departamento DE WITH (NOLOCK)
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	           ON DI.IDDivisao = DE.IDDivisao
 WHERE DE.cdSistema = @cdSistema
   AND (@cdDivisao IS NULL OR DI.cdDivisao = @cdDivisao)
   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
   AND (@dsDepartamento IS NULL OR DE.dsDepartamento LIKE '%' + @dsDepartamento + '%')
   AND (@blPerecivel IS NULL OR DE.blPerecivel = @blPerecivel)