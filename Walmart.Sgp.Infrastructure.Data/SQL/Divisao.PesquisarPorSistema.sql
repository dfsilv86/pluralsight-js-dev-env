/*
DECLARE @cdDivisao INT, @cdSistema INT, @dsDivisao NVARCHAR(50);
SET @cdDivisao = 1;
SET @dsDivisao = '';
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
 WHERE cdSistema = @cdSistema
   AND (@cdDivisao IS NULL OR cdDivisao = @cdDivisao)
   AND (@dsDivisao IS NULL OR dsDivisao LIKE '%' + @dsDivisao + '%')