/*
DECLARE @cdSistema SMALLINT, @idItemDetalhe INT;
SET @idItemDetalhe = 1;
SET @cdSistema = 1;
--*/

SELECT TOP 1 RIP.pcRendimentoReceita
  FROM RelacionamentoItemPrincipal RIP
 WHERE RIP.IDTipoRelacionamento = 2
   AND RIP.IDItemDetalhe = @idItemDetalhe
   AND RIP.cdSistema = @cdSistema