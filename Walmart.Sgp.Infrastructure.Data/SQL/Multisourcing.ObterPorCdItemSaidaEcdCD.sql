/*
DECLARE @cdItemSaida INT, @cdCD INT;
SET @cdItemSaida = 500061913;
SET @cdCD = 7471;
--*/

SELECT MS.* FROM Multisourcing AS MS WITH(NOLOCK)
INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
ON RIS.IDRelacionamentoItemSecundario = MS.IDRelacionamentoItemSecundario
INNER JOIN RelacionamentoItemPrincipal RIP WITH(NOLOCK)
ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
INNER JOIN ItemDetalhe ID WITH(NOLOCK)
ON ID.IDItemDetalhe = RIP.IDItemDetalhe
WHERE ID.cdItem = @cdItemSaida AND MS.IDCD = (SELECT CD.IDCD FROM CD WHERE CD.cdCD = @cdCD)