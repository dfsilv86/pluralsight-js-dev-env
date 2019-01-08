/*
DECLARE @cdLoja INT, @cdItem INT, @dtSolicitacao DATETIME
--*/

SELECT COUNT(*)
FROM NotaFiscal NF WITH (NOLOCK)
JOIN NotaFiscalItem NFI WITH (NOLOCK) on NF.IDNotaFiscal = NFI.IDNotaFiscal
JOIN ItemDetalhe ID WITH (NOLOCK) on NFI.IDItemDetalhe = ID.IDItemDetalhe
JOIN Loja L WITH (NOLOCK) on NF.IDLoja = L.IDLoja
WHERE L.cdLoja = @cdLoja
AND ID.cdItem = @cdItem
AND NF.dtRecebimento <= @dtSolicitacao
AND NFI.IdNotaFiscalItemStatus = 3