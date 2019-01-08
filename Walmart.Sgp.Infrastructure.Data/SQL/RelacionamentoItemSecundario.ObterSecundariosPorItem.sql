/*
DECLARE @IDItemDetalhe INT, @cdSistema INT;
SET @IDItemDetalhe = 120805;
SET @cdSistema = 1;
--*/

SELECT RIP.IDRelacionamentoItemPrincipal
     , RIP.IDItemDetalhe
     , RIP.IDTipoRelacionamento
     , RIP.pcRendimentoReceita
     , NULL AS SplitOn1
     , ID1.IDItemDetalhe
     , ID1.cdItem
     , ID1.dsItem
	 , ID1.vlFatorConversao
     , NULL AS SplitOn2
     , RIS.IDRelacionamentoItemSecundario
	 , RIS.IDItemDetalhe
     , RIS.tpItem
	 , RIS.psItem
	 , RIS.pcRendimentoDerivado
     , NULL AS SplitOn3
     , ID2.IDItemDetalhe
     , ID2.cdItem
     , ID2.dsItem
	 , ID2.vlFatorConversao
  FROM RelacionamentoItemPrincipal RIP WITH (NOLOCK)
       INNER JOIN ItemDetalhe ID1 WITH (NOLOCK)
               ON ID1.IDItemDetalhe = RIP.IDItemDetalhe
       INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
	           ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal       
       INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
               ON ID2.IDItemDetalhe = RIS.IDItemDetalhe
 WHERE RIS.IDItemDetalhe = @IDItemDetalhe
 ORDER BY ID2.cdItem