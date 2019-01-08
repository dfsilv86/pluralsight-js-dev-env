/*
DECLARE @idUsuario INT = 13172;
DECLARE @idPapel INT = 11;
DECLARE @idLoja INT = 663;
--*/
DECLARE @dtAtual DATE = (
		SELECT CAST(GETDATE() AS DATE)
		);

SELECT COUNT(*)
FROM SugestaoReturnSheet SRS WITH (NOLOCK)
JOIN ReturnSheetItemLoja RSIL WITH (NOLOCK) ON RSIL.idReturnSheetItemLoja = SRS.idReturnSheetItemLoja
JOIN ReturnSheetItemPrincipal RSIP WITH (NOLOCK) ON RSIP.idReturnSheetItemPrincipal = RSIL.idReturnSheetItemPrincipal
JOIN ReturnSheet RS WITH (NOLOCK) ON RS.idReturnSheet = RSIP.idReturnSheet
WHERE CAST(RS.dhInicioReturn AS DATE) <= @dtAtual
	AND CAST(RS.dhFinalReturn AS DATE) >= @dtAtual
	AND SRS.QtdLoja IS NULL
	AND RSIL.idLoja = @idLoja
	AND SRS.blAtivo = 1
	AND EXISTS (
		SELECT TOP 1 1
		FROM CWIRole R
		WHERE R.Id = @idPapel
			AND R.IsLoja = 1
		)
	AND NOT EXISTS (
		SELECT TOP 1 1
		FROM SugestaoReturnSheet SRS2 WITH (NOLOCK)
		JOIN ReturnSheetItemLoja RSIL2 WITH (NOLOCK) ON RSIL2.idReturnSheetItemLoja = SRS2.idReturnSheetItemLoja
		JOIN ReturnSheetItemPrincipal RSIP2 WITH (NOLOCK) ON RSIP2.idReturnSheetItemPrincipal = RSIL2.idReturnSheetItemPrincipal
		JOIN ReturnSheet RS2 WITH (NOLOCK) ON RS2.idReturnSheet = RSIP2.idReturnSheet
		WHERE RS2.idReturnSheet = RS.idReturnSheet
			AND (
				SRS2.blAutorizado = 1
				OR SRS2.blExportado = 1
				)
			AND SRS2.blAtivo = 1
		)
