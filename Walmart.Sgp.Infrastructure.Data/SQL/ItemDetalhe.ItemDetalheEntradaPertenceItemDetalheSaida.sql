
/*
DECLARE @cdItemEntrada BIGINT = 500064132
DECLARE @cdItemSaida BIGINT = 500064209
DECLARE @cdSistema INT = 1
*/

	SELECT RIS.IDRelacionamentoItemSecundario
      FROM ItemDetalhe IDD_E WITH(NOLOCK)
INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
	    ON IDD_E.IDItemDetalhe = RIS.IDItemDetalhe
INNER JOIN RelacionamentoItemPrincipal RIP WITH(NOLOCK)
		ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
INNER JOIN ItemDetalhe IDD_S WITH(NOLOCK)
		ON IDD_S.IDItemDetalhe = RIP.IDItemDetalhe
	 WHERE IDD_E.cdItem = @cdItemEntrada
	   AND IDD_S.cdItem = @cdItemSaida
	   AND IDD_E.cdSistema = @cdSistema
	   AND IDD_S.cdSistema = @cdSistema