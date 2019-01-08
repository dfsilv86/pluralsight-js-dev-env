/*
DECLARE @cdSistema SMALLINT, @idItemDetalhe INT;
SET @idItemDetalhe = 1;
SET @cdSistema = 1;
--*/

SELECT TOP 1 RIS.pcRendimentoDerivado
  FROM RelacionamentoItemPrincipal RIP WITH (NOLOCK)
       INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
	           ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
 WHERE RIP.IDTipoRelacionamento = 3
   AND RIS.IDItemDetalhe = @idItemDetalhe
   AND RIP.cdSistema = @cdSistema