/*
DECLARE @idRip INT;
SET @idRip = 27316;
--*/

SELECT COUNT(*)
FROM Multisourcing AS MS WITH(NOLOCK)
INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
	ON RIS.IDRelacionamentoItemSecundario = MS.idRelacionamentoItemSecundario
WHERE RIS.IDRelacionamentoItemPrincipal = @idRip