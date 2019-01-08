
ALTER FUNCTION [dbo].[fnReabSugestaoPedidoItensStaple] (@Data DATE)
RETURNS @Result TABLE
(
	IdFornecedor BIGINT NOT NULL,
	ID_ITEM_ENTRADA BIGINT NOT NULL,
	CD_ITEM_ENTRADA INT NOT NULL,
	ID_ITEM_SAIDA BIGINT NOT NULL,
	CD_ITEM_SAIDA INT NOT NULL,
	IdLoja INT NOT NULL,
	cdLoja INT NOT NULL,
	VlLeadTime SMALLINT NULL,
	CdReviewdate VARCHAR(30) NULL,
	TPWEEK SMALLINT NULL,
	cdSequenciaVendor SMALLINT NULL,
	cdDepartamentoVendor SMALLINT NULL,
	vlModulo DECIMAL (11,4) NULL,
	qtVendorPackage INT NULL,
	TpInterval SMALLINT NULL,
	vlTipoReabastecimento SMALLINT NULL,
	IDFornecedorParametro BIGINT NULL,
	vlShelfLife SMALLINT NULL,
	vlFatorConversao FLOAT NULL,
	blPossuiVendasUltimaSemana BIT NULL,
	tpCalculoSeguranca CHAR(1),
	IDOrigemDadosCalculo INt,
	IDCD INT,
	tpCaixaFornecedor CHAR,
	vlPesoLiquido DECIMAL(11,4),
	blCDConvertido BIT
)
AS
BEGIN

	
	DECLARE @TpWeek smallint;
	
	SELECT @TpWeek = (SELECT dsText 
						FROM CWIDOMAINVALUE  WITH (NOLOCK)
					   WHERE IDDOMAIN = (SELECT IDDOMAIN 
										   FROM CWIDOMAIN WITH (NOLOCK)
										  WHERE nmDomain = 'SemanaWalMart') 
											AND dsValue = 'W');	


	IF (@Data IS NULL)
	BEGIN 
		SELECT @Data = GETDATE();
	END 
	ELSE
	BEGIN
		IF ((ABS(DATEPART(wk, GETDATE() + 20) - DATEPART(wk, GETDATE())) % 2) = 1)  
			SELECT @TpWeek = CASE @TpWeek WHEN 1 THEN 2 ELSE 1 END 	
	END

	INSERT @Result
	SELECT 
			F.IdFornecedor
		,	IDE.IdItemDetalhe ID_ITEM_ENTRADA 
		,	IDE.cdItem CD_ITEM_ENTRADA
		,	IDS.IdItemDetalhe ID_ITEM_SAIDA
		,	IDS.cdItem CD_ITEM_SAIDA
		,	L.IdLoja
		,	L.cdLoja
		,	ISNULL(LCP.VLLEADTIME, 0) VLLEADTIME 
		,	CASE WHEN CD.blConvertido = 0
				THEN CAST(RDC.CDREVIEWDATE as varchar)
				ELSE CAST(COALESCE(FLP.cdReviewDate, FCP.cdReviewDate, FP.cdReviewDate) as varchar)
			 END as CDREVIEWDATE
		,	CASE WHEN CD.blConvertido = 0
				THEN LCP.TPWEEK
				ELSE COALESCE(FLP.TPWEEK, FCP.TPWEEK, FP.TPWEEK)
			 END as TPWEEK
		,	IDE.cdSequenciaVendor
		,	DE.cdDepartamento as cdDepartamentoVendor
		,	IDE.vlModulo
		,	IDE.qtVendorPackage
		,	CASE WHEN CD.blConvertido = 0
				THEN LCP.TPINTERVAL
				ELSE COALESCE(FLP.TPINTERVAL, FCP.TPINTERVAL, FP.TPINTERVAL)
			 END as TPINTERVAL
		,	RILC.vlTipoReabastecimento
		,	IDE.IDFornecedorParametro
		,	IDE.vlShelfLife
		,	IDE.vlFatorConversao
		,	0  blPossuiVendasUltimaSemana
		,	CASE WHEN (ISNULL(IDE.cdPLU, 0) = 0) AND ((IDE.tpReceituario = 'I' AND IDE.tpManipulado IS NULL) OR (IDE.tpReceituario IS NULL AND IDE.tpManipulado = 'P') OR (IDE.tpReceituario = 'I' AND IDE.tpManipulado = 'P'))
				 THEN 'I'				
				 ELSE ''
			END tpCalculoSeguranca
		,	3 IDOrigemDadosCalculo
		,	CD.IDCD
		,	IDE.tpCaixaFornecedor
		,	IDE.vlPesoLiquido
		,	CD.blConvertido as blCDConvertido
		FROM RelacaoItemLojaCD RILC WITH(NOLOCK)
		INNER JOIN ItemDetalhe IDE WITH(NOLOCK)
			ON IDE.IDItemDetalhe = RILC.idItemEntrada
			AND IDE.tpStatus = 'A'
			AND IDE.cdSistema = 1
			AND IDE.blItemTransferencia = 0
		INNER JOIN Departamento DE WITH (NOLOCK)
			ON DE.IDDepartamento = IDE.IDDepartamento
		INNER JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IdItemDetalhe = RILC.IdItem
			AND IDS.tpStatus = 'A'
		INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
			ON LCP.IDLojaCDParametro = RILC.IDLojaCDParametro
		INNER JOIN CD CD WITH(NOLOCK)
			ON CD.IDCD = LCP.IDCD
		INNER JOIN FornecedorParametro FP WITH(NOLOCK)
			ON FP.IDFornecedorParametro = IDE.idFornecedorParametro
			AND FP.cdStatusVendor = 'A'
			AND FP.tpStoreApprovalRequired IN ('Y', 'R')
		INNER JOIN Fornecedor F WITH(NOLOCK)
			ON F.IDFornecedor = FP.IDFornecedor
			AND F.blAtivo = 1
		LEFT JOIN FornecedorCDParametro FCP WITH(NOLOCK)
			ON FCP.IDFornecedorParametro = FP.IDFornecedorParametro
			AND FCP.IDCD = CD.IDCD
		LEFT JOIN FornecedorLojaParametro FLP WITH(NOLOCK)
			ON FLP.IDFornecedorParametro = FP.IDFornecedorParametro
			AND FLP.IDLoja = LCP.IDLoja
		LEFT JOIN ReviewDateCD RDC WITH(NOLOCK)
			ON RDC.IDLojaCDParametro = LCP.IDLojaCDParametro
			AND RDC.IDDepartamento = IDE.IDDepartamento
			AND RDC.tpReabastecimento = 'S'
			AND RDC.blAtivo = 1
		INNER JOIN Loja L WITH(NOLOCK)
			ON L.IDLoja = LCP.IDLoja
			AND L.blAtivo = 1
			AND L.blCalculaSugestao = 1
		WHERE RILC.vlTipoReabastecimento IN (20, 22, 40, 42, 43, 81)
		AND ((CD.blConvertido = 0 AND CAST(RDC.CDREVIEWDATE AS NVARCHAR) like '%' + CAST(DATEPART(WEEKDAY,@Data) AS NVARCHAR) + '%'
				AND (LCP.TPINTERVAL IN (1, 0) OR (LCP.TPINTERVAL = '2' AND LCP.TPWEEK = @TpWeek)))
			OR
			(CD.blConvertido = 1 AND CAST(COALESCE(FLP.cdReviewDate, FCP.cdReviewDate, FP.cdReviewDate) AS NVARCHAR) like '%' + CAST(DATEPART(WEEKDAY,@Data) AS NVARCHAR) + '%'
				AND (COALESCE(FLP.tpInterval, FCP.tpInterval, FP.tpInterval) IN (1, 0) OR (COALESCE(FLP.tpInterval, FCP.tpInterval, FP.tpInterval) = '2' AND COALESCE(FLP.tpWeek, FCP.tpWeek, FP.tpWeek) = @TpWeek))))
	
	RETURN
END																  														  
GO


