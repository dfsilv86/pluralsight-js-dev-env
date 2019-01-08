/*
DECLARE @cdItemSaida BIGINT, @cdCD BIGINT, @cdSistema INT;
SET @cdItemSaida = 9302618;
SET @cdCD = 7471;
SET @cdSistema = 1;
--*/

	SELECT COUNT(1)
	  FROM CD WITH(NOLOCK)
INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
		ON LCP.IDCD = CD.IDCD AND LCP.blAtivo = 1
INNER JOIN RelacaoItemLojaCD RILC WITH(NOLOCK)
		ON RILC.IDLojaCDParametro = LCP.IDLojaCDParametro AND RILC.blAtivo = 1
INNER JOIN ItemDetalhe IDD_S WITH(NOLOCK)
		ON IDD_S.IDItemDetalhe = RILC.IDItem
	 WHERE IDD_S.cdItem = @cdItemSaida
	   AND CD.blAtivo = 1
	   AND CD.blConvertido = 1
	   AND CD.cdCD = @cdCD         
	   AND CD.cdSistema = @cdSistema