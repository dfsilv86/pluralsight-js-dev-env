SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VW_ConsultaAlteracaoCusto]
AS
SELECT nfi.IDNotaFiscalItem
	,lja.cdLoja
	,nf.dtRecebimento
	,nf.dtEmissao
	,nf.nrNotaFiscal
	,d.cdDepartamento
	,CONVERT(VARCHAR, ide.cdItem) + ' - ' + ide.dsItem AS cddsItem
	,rgga.dhLiberacao
	,CASE 
		WHEN nfi.vlCusto <= 0
			THEN 0
		ELSE nfi.vlCusto
		END AS vlCusto
	,CASE 
		WHEN ISNULL(TB.VlCustoCompraAtual, 0) = 0
			THEN ISNULL(TB.VlCustoCadastroAtual, 0)
		ELSE TB.VlCustoCompraAtual
		END AS vlCustoCompraAtual
	,nfi.VariacaoUltimoCusto
	,nfi.qtItem
	,ISNULL(rgga.qtItemCorrigida, nfi.qtItem) AS qtItemCorrigida
	,nfi.vlMercadoria
	,ide.QtVendorPackage
	,ide.DsTamanhoItem
	,CASE 
		WHEN ide.QtVendorPackage > 0
			THEN (ide.VlCustoUnitario / ISNULL(ide.QtVendorPackage, 1))
		ELSE 0
		END AS vlCustoUnitarioReal
	,ide.VlCustoUnitario
	,nfi.dhCriacao
	,nfi.dtLiberacao
	,usr.FullName AS usrNomeAlteracao
	,nfi.IdNotaFiscalItemStatus
	,lja.IDBandeira
	,nf.IDFornecedor
	,nf.IDLoja
	,nfi.IDItemDetalhe
	,ide.IDDepartamento
	,nf.dtCadastroConcentrador
	,nf.dtAtualizacaoConcentrador
FROM NotaFiscal nf WITH (NOLOCK)
JOIN NotaFiscalItem nfi WITH (NOLOCK)
	ON nf.IDNotaFiscal = nfi.IDNotaFiscal
JOIN Loja lja WITH (NOLOCK)
	ON nf.idloja = lja.idloja
JOIN ItemDetalhe ide WITH (NOLOCK)
	ON nfi.IDItemDetalhe = ide.IDItemDetalhe
JOIN Departamento D WITH (NOLOCK)
	ON D.IDDepartamento = IDE.IDDepartamento
LEFT JOIN RegistroCorrecaoCusto rgga WITH (NOLOCK)
	ON rgga.IDNotaFiscalItem = nfi.IDNotaFiscalItem
		AND rgga.IDRegistroCorrecaoCusto = (
			SELECT MAX(IDRegistroCorrecaoCusto)
			FROM RegistroCorrecaoCusto RC WITH (NOLOCK)
			WHERE RC.IDNotaFiscalItem = nfi.IDNotaFiscalItem
			)
LEFT JOIN cwiUser usr WITH (NOLOCK)
	ON usr.Id = rgga.IDUsuarioLiberacao
LEFT JOIN Estoque TB WITH (NOLOCK)
	ON TB.IDItemDEtalhe = nfi.IDItemDetalhe
		AND tb.IDLoja = nf.IDLoja
		AND TB.dtRecebimento = (
			SELECT MAX(dtRecebimento)
			FROM Estoque WITH (NOLOCK)
			WHERE IDLoja = TB.IDLoja
				AND IDItemDetalhe = TB.IDItemdetalhe
				AND dtRecebimento <= nf.dtRecebimento);
GO


