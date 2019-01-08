/*
DECLARE @cdCD INT, @cdLoja INT, @cdSistema BIGINT;
SET @cdCD = 7472;
SET @cdLoja = 1039;
SET @cdSistema = 1;
--*/

SELECT COUNT(*) FROM LojaCDParametro AS LCP WITH (NOLOCK)
JOIN Loja L WITH (NOLOCK)
	ON L.IDLoja = LCP.IDLoja AND L.blAtivo = 1 AND L.cdSistema = @cdSistema
JOIN CD WITH (NOLOCK)
	ON CD.IDCD = LCP.IDCD AND CD.blAtivo  = 1 AND CD.cdSistema = @cdSistema
WHERE CD.cdCD = @cdCD AND L.cdLoja = @cdLoja AND LCP.blAtivo = 1 AND LCP.cdSistema = @cdSistema