/*
DECLARE @cdDepartamento INT, @cdSistema INT, @cdDivisao INT, @modoPereciveis VARCHAR(20);
SET @cdDepartamento = 1;
SET @cdSistema = 1;
SET @modoPereciveis = 'qualquer'
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
	 , NULL AS SplitOn1
	 , DI.IDDivisao
	 , DI.cdDivisao
	 , DI.dsDivisao
	 , DI.cdSistema
  FROM dbo.Departamento DE WITH (NOLOCK)
       LEFT JOIN dbo.Divisao DI WITH (NOLOCK)
	          ON DI.IDDivisao = DE.IDDivisao
 WHERE DE.cdDepartamento = @cdDepartamento
   AND (@cdDivisao IS NULL OR DI.cdDivisao = @cdDivisao)
   AND DE.cdSistema = @cdSistema
   AND (blPerecivel = 'S' OR cdDepartamento = 0 OR @modoPereciveis = 'qualquer') -- Retorna apenas se blPerecivel='S' (comportamento padrão das lookups de departamento) ou se código de departamento = 0 (para evitar fluxo estranho em tela)
