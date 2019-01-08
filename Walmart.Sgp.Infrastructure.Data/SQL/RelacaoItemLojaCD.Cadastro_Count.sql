/*DECLARE @cdItemSaida INT, @cdSistema INT, @cdCD INT, @cdLoja INT;
SET @cdLoja = 74;
SET @cdCD = 7400;
SET @cdItemSaida = 9488866;
SET @cdSistema = 1;
--*/

SELECT COUNT(*)
FROM RelacaoItemLojaCD AS RILC WITH (NOLOCK)
INNER JOIN LojaCDParametro LCP WITH (NOLOCK)
	ON LCP.IDLojaCDParametro = RILC.IDLojaCDParametro AND LCP.blAtivo = 1 AND LCP.cdSistema = @cdSistema
INNER JOIN Loja L WITH (NOLOCK)
	ON L.IDLoja = LCP.IDLoja AND L.blAtivo = 1 AND L.cdSistema = @cdSistema
INNER JOIN CD CD WITH (NOLOCK)
	ON CD.IDCD = LCP.IDCD AND CD.blAtivo = 1 AND CD.cdSistema = @cdSistema
INNER JOIN ItemDetalhe ID_S WITH (NOLOCK)
	ON ID_S.IDItemDetalhe = RILC.IDItem AND ID_S.blAtivo = 1 AND ID_S.cdSistema = @cdSistema
WHERE ID_S.cdItem = @cdItemSaida
  AND L.cdLoja = @cdLoja
  AND CD.cdCD = @cdCD
  AND RILC.idItemEntrada IS NOT NULL