/****** Object:  StoredProcedure [dbo].[PR_ReportItensDerivados]    Script Date: 05/10/2016 14:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[PR_ReportItensDerivados]
	@cdSistema				tinyint,
	@IDTipoRelacionamento	int,
	@IDItemDetalhe			bigint = null,
	@IDDepartamento			int    = null,
	@IDCategoria			bigint = null,
	@dsItem					varchar(100) = null,
	@IDSubcategoria			int = null,
	@IDFineline				int = null,
	@IDRegiaoCompra			int = null
AS  
  
BEGIN   
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
 SET NOCOUNT ON;  
  
SELECT   
 IDRelacionamentoItemPrincipal,   
 dsDeptoCategoria,
 dsCategoria,
 dsSubcategoria,
 dsFineline,
 ItemPai,
 ItemDerivado,  
 dsItem,
 dsHostItem,
 cdUPC,
 cdFornecedor,
 nmFornecedor,  
 dsStatusItem,  
 cdPLU,  
 dsTamanhoItem,
 pcRendimentoDerivado,  
 Tipo,
 dsRegiaoCompra,
 TipoItemRA,
 dsAreaCD,
 ItemMTR,
 tpUnidadeMedida
FROM (   
   
 SELECT  
   RIP.IDRelacionamentoItemPrincipal,  
   CASE  
		WHEN (ID.cdSistema = 1) THEN CONVERT(VARCHAR,D.cdDepartamento) + ' - ' + D.dsDepartamento
		WHEN (ID.cdSistema = 2) THEN CONVERT(VARCHAR,C.cdCategoria)    + ' - ' + C.dsCategoria
		ELSE ''
   END AS dsDeptoCategoria,
   CONVERT(VARCHAR, C.cdCategoria) + ' - ' + C.dsCategoria AS dsCategoria,
   CONVERT(VARCHAR, sc.cdSubcategoria) + ' - ' + SC.dsSubcategoria AS dsSubcategoria,
   CONVERT(VARCHAR, FL.cdFineLine) + ' - ' + FL.dsFineLine AS dsFineline,
   ID.cdOldNumber as ItemPai,
   NULL as ItemDerivado,
   ID.dsItem,
   ID.dsHostItem,
   ID.cdUPC,
   F.cdFornecedor,
   F.nmFornecedor,  
   CASE ID.tpStatus  
    WHEN 'A' THEN 'Ativo'  
    WHEN 'I' THEN 'Inativo'  
    WHEN 'D' THEN 'Deletado'  
    ELSE ''  
   END AS dsStatusItem,  
   ID.cdPLU,  
   ID.dsTamanhoItem,
   0 AS pcRendimentoDerivado,
   0 AS Tipo,
   dsRegiaoCompra,
   ID.vlTipoReabastecimento as 'TipoItemRA',
   ACD.dsAreaCD,
   ItemMTR = CASE WHEN ID.blItemTransferencia = 1 THEN 'Sim' ELSE 'Não' END,
   ID.tpUnidadeMedida
 FROM RelacionamentoItemPrincipal RIP
	INNER JOIN ItemDetalhe ID	on (ID.IDItemDetalhe = RIP.IDItemDetalhe)
	INNER JOIN Departamento D	on (D.IDDepartamento = RIP.IDDepartamento)
	INNER JOIN Fornecedor F		ON (F.IDFornecedor = ID.IDFornecedor)
	INNER JOIN Categoria C		on (C.IDCategoria	 = RIP.IDCategoria)
	LEFT  JOIN RegiaoCompra RC	on (RC.idRegiaoCompra = ID.idRegiaoCompra)
	INNER JOIN Subcategoria SC  on (SC.IDSubcategoria = ID.IDSubcategoria)
	INNER JOIN Fineline FL		on (FL.IDFineLine = ID.IDFineline)
	LEFT  JOIN AreaCD ACD       on (ACD.idAreaCD = ID.idAreaCD)
	WHERE (RIP.cdSistema = @cdSistema)
	  AND (RIP.IDTipoRelacionamento = @IDTipoRelacionamento)
	  AND (RIP.IDItemDetalhe  = @IDItemDetalhe  OR @IDItemDetalhe  IS NULL)
	  AND (RIP.IDDepartamento = @IDDepartamento OR @IDDepartamento IS NULL)
	  AND (RIP.IDCategoria    = @IDCategoria    OR @IDCategoria IS NULL)
	  AND (UPPER(ID.dsItem) LIKE('%' + UPPER(@dsItem) + '%') OR @dsItem IS NULL)
	  AND (ID.IDSubcategoria = @IDSubcategoria OR @IDSubcategoria IS NULL)
	  AND (ID.IDFineline = @IDFineline OR @IDFineline IS NULL)
	  AND (ID.idRegiaoCompra = @IDRegiaoCompra OR @IDRegiaoCompra IS NULL)
    
  UNION ALL  
  
  SELECT  
   RIS.IDRelacionamentoItemPrincipal,  
   '' AS dsDeptoCategoria,
   CONVERT(VARCHAR, C.cdCategoria) + ' - ' + C.dsCategoria AS dsCategoria,
   CONVERT(VARCHAR, sc.cdSubcategoria) + ' - ' + SC.dsSubcategoria AS dsSubcategoria,
   CONVERT(VARCHAR, FL.cdFineLine) + ' - ' + FL.dsFineLine AS dsFineline,
   IDP.cdOldNumber as ItemPai,
   IDS.cdOldNumber as ItemDerivado,  
   IDS.dsItem,
   IDS.dsHostItem,
   IDS.cdUPC,
   F.cdFornecedor,
   F.nmFornecedor,  
   CASE IDS.tpStatus  
    WHEN 'A' THEN 'Ativo'  
    WHEN 'I' THEN 'Inativo'  
    WHEN 'D' THEN 'Deletado'  
    ELSE ''  
   END AS dsStatusItem,  
   IDS.cdPLU,  
   IDS.dsTamanhoItem,
   pcRendimentoDerivado,
   1 AS Tipo,
   RC.dsRegiaoCompra,
   IDS.vlTipoReabastecimento as 'TipoItemRA',
   ACD.dsAreaCD,
   ItemMTR = CASE WHEN IDS.blItemTransferencia = 1 THEN 'Sim' ELSE 'Não' END,
   IDS.tpUnidadeMedida
  FROM RelacionamentoItemPrincipal RIP
	INNER JOIN RelacionamentoItemSecundario RIS on (RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal)
	INNER JOIN ItemDetalhe IDP on (IDP.IDItemDetalhe = RIP.IDItemDetalhe)
	INNER JOIN ItemDetalhe IDS on (IDS.IDItemDetalhe = RIS.IDItemDetalhe)
	INNER JOIN Departamento D  on (D.IDDepartamento  = RIP.IDDepartamento)
	INNER JOIN Fornecedor F	   on (F.IDFornecedor = IDS.IDFornecedor)	
	INNER JOIN Categoria C     on (C.IDCategoria     = RIP.IDCategoria)
	LEFT JOIN  RegiaoCompra RC on (RC.idRegiaoCompra = IDS.idRegiaoCompra)
	INNER JOIN Subcategoria SC  on (SC.IDSubcategoria = IDS.IDSubcategoria)
	INNER JOIN Fineline FL		on (FL.IDFineLine = IDS.IDFineline)
	LEFT  JOIN AreaCD ACD       on (ACD.idAreaCD = IDS.idAreaCD)
	WHERE (RIP.cdSistema = @cdSistema)
	  AND (RIP.IDTipoRelacionamento = @IDTipoRelacionamento)
	  AND (RIP.IDItemDetalhe  = @IDItemDetalhe  OR @IDItemDetalhe  IS NULL)
	  AND (RIP.IDDepartamento = @IDDepartamento OR @IDDepartamento IS NULL)
	  AND (RIP.IDCategoria    = @IDCategoria    OR @IDCategoria    IS NULL)
	  AND (UPPER(IDP.dsItem) LIKE('%' + UPPER(@dsItem) + '%') OR @dsItem IS NULL)
	  AND (IDP.IDSubcategoria = @IDSubcategoria OR @IDSubcategoria IS NULL)
	  AND (IDP.IDFineline = @IDFineline OR @IDFineline IS NULL)
	  AND (IDP.idRegiaoCompra = @IDRegiaoCompra OR @IDRegiaoCompra IS NULL)
) AS Query  
ORDER BY IDRelacionamentoItemPrincipal, Tipo, dsItem
  
END

