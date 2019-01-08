/*
DECLARE @cdItemEntrada INT, @cdItemSaida INT, @cdSistema INT;
SET @cdItemEntrada = 500145783;
SET @cdItemSaida = 12321;
SET @cdSistema = 1;
--*/

SELECT COUNT(1)
FROM (
SELECT 1 Itens
FROM RelacionamentoItemPrincipal AS RIP WITH(NOLOCK)
INNER JOIN ItemDetalhe ID_S WITH(NOLOCK)
	ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND ID_S.blAtivo = 1 AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
	ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
INNER JOIN ItemDetalhe ID_E WITH(NOLOCK)
	ON RIS.IDItemDetalhe = ID_E.IDItemDetalhe AND ID_E.blAtivo = 1
WHERE ID_S.cdItem = @cdItemSaida
	AND ID_S.cdSistema = @cdSistema
	AND ID_E.cdItem = @cdItemEntrada
	AND ID_E.cdSistema = @cdSistema
UNION	
	SELECT 1 Itens
	FROM RelacionamentoItemPrincipal RIP WITH(NOLOCK)
 JOIN ItemDetalhe ID_S WITH(NOLOCK)
	ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND ID_S.blAtivo = 1 AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
	JOIN RelacionamentoItemPrime ITP WITH(NOLOCK)
		ON ITP.idItemDetalhe = RIP.IDItemDetalhe 
			AND ITP.Sequencial = 1
WHERE ID_S.cdItem = @cdItemSaida
	AND ID_S.cdSistema = @cdSistema ) AS ITENS