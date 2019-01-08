/*
DECLARE @idDepartamento INT
SET @idDepartamento = 1
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
	 , DI.cdSistema
  FROM dbo.Departamento DE WITH (NOLOCK)
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	           ON DI.IDDivisao = DE.IDDivisao
 WHERE DE.IDDepartamento = @idDepartamento