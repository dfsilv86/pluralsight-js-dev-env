/*
DECLARE @cdDivisao INT, @cdSistema INT;
SET @cdDivisao = 1;
SET @cdSistema = 1;
--*/

SELECT IDDivisao
     , cdSistema
	 , cdDivisao
	 , dsDivisao
	 , blAtivo
	 , dhCriacao
	 , dhAtualizacao
     , cdUsuarioCriacao
     , cdUsuarioAtualizacao
  FROM dbo.Divisao WITH (NOLOCK)
 WHERE cdDivisao = @cdDivisao
   AND cdSistema = @cdSistema