/*
DECLARE @evento VARCHAR(MAX), @idDepartamento BIGINT, @ativos BIGINT, @inicioReturn DATE, @finalReturn DATE, @idRegiaoCompra BIGINT;
SET @evento = NULL;
SET @idDepartamento = 5;
SET @ativos = 2;
SET @inicioReturn = NULL;
SET @finalReturn = NULL;
SET @idRegiaoCompra = NULL;
--*/

SELECT 
	RS.IdReturnSheet, 
	RS.DhInicioReturn, 
	RS.DhFinalReturn, 
	RS.DhInicioEvento,
	RS.DhFinalEvento, 
	RS.idDepartamento, 
	RS.Descricao, 
	RS.IdUsuarioCriacao, 
	RS.DhAtualizacao, 
	RS.DhCriacao, 
	RS.BlAtivo, 
	(
		SELECT CAST(COUNT(1) As bit) 
		FROM SugestaoReturnSheet AS SRS2 WITH(NOLOCK)
		INNER JOIN ReturnSheetItemLoja RSIL2 WITH(NOLOCK) 
			ON SRS2.IdReturnSheetItemLoja = RSIL2.IdReturnSheetItemLoja
		INNER JOIN ReturnSheetItemPrincipal RSIP2 WITH(NOLOCK)
			ON RSIP2.IdReturnSheetItemPrincipal = RSIL2.IdReturnSheetItemPrincipal
		INNER JOIN ReturnSheet RS2 WITH(NOLOCK)
			ON RS2.IdReturnSheet = RSIP2.IdReturnSheet
		WHERE SRS2.BlExportado = 1 AND RS2.IdReturnSheet = RS.IdReturnSheet
	 ) As Exportada,
	NULL As SplitOn1,
	RC.IdRegiaoCompra, 
	RC.dsRegiaoCompra, 
	NULL As SplitOn2, 
	CU.Id, 
	CU.Username, 
	CU.FullName
FROM ReturnSheet AS RS WITH(NOLOCK)
INNER JOIN CWIUser CU WITH(NOLOCK)
	ON CU.Id = RS.IdUsuarioCriacao
INNER JOIN RegiaoCompra RC WITH(NOLOCK)
	ON RC.IdRegiaoCompra = RS.IdRegiaoCompra
WHERE 
(@inicioReturn IS NULL OR (@inicioReturn IS NOT NULL AND (CAST(@inicioReturn AS DATE) <= CAST(RS.DhInicioReturn AS DATE))))
AND (@finalReturn IS NULL OR (@finalReturn IS NOT NULL AND (CAST(@finalReturn AS DATE) >= CAST(RS.DhFinalReturn AS DATE))))
AND RS.idDepartamento = ISNULL(@idDepartamento, RS.idDepartamento)
AND ((@ativos = 1 AND RS.BlAtivo = 1) OR (@ativos = 0 AND RS.BlAtivo = 0) OR @ativos = 2)
AND (@evento IS NULL OR RS.Descricao LIKE '%' + @evento + '%')
AND RC.IdRegiaoCompra = ISNULL(@idRegiaoCompra, RC.IdRegiaoCompra)