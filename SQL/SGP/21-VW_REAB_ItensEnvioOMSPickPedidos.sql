/*
=======================================================================================================================
View...................: VW_REAB_ItensEnvioOMSPickPedidos
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 14/04/2016
Objetivo...............: Listar registros para o envio de pedidos OMS para Pick Pedidos
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: 
Data Alteração.........: 
Descrição da alteração.: 

=======================================================================================================================
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create VIEW [dbo].[VW_REAB_ItensEnvioOMSPickPedidos]
AS
	SELECT 	 '01' [OUTB_DIV]											-- Campo 01: OUTB-DIV; 			Size:	2
			,SPACE(1) FILLER1 											-- Campo 02: FILLER; 			Size:	1
			,RIGHT(REPLICATE('0', 5) + RTrim(L.cdLoja), 5) [OUTB_STORE]	-- Campo 03: Código Loja;		Size:	5
			,SPACE(1) FILLER2 											-- Campo 04: FILLER; 			Size:	1
			,RIGHT(REPLICATE('0', 9) + RTrim(ID.cdItem), 9) [OUTB_ITEM]	-- Campo 05: OUTB-ITEM; 		Size:	9
			,SPACE(1) FILLER3 											-- Campo 06: FILLER; 			Size:	1
			,'4' [OUTB_MSG_CODE]										-- Campo 07: OUTB-MSG-COD; 		Size:	1
			,SPACE(1) FILLER4 											-- Campo 08: FILLER; 			Size:	1
			,RIGHT(REPLICATE('0', 5) + RTrim(CASE 
												WHEN SP.tpCaixaFornecedor = 'V'
													THEN SP.qtdPackCompra
												ELSE SP.qtdPackCompra * SP.qtVendorPackage
												END), 5) [OUTB_QTY]		-- Campo 09: OUTB-QTY; 			Size:	5
			,SPACE(1) FILLER5 											-- Campo 10: FILLER; 			Size:	1
			,RIGHT(REPLICATE('0', 5) + RTrim(CD.cdCD), 5) [OUTB_DC_NBR]	-- Campo 11: OUTB-DC-NBR; 		Size:	5
			,SPACE(1) FILLER6 											-- Campo 12: FILLER; 			Size:	1
			,'E' [OUTB_UNIT_TYPE]										-- Campo 12: OUTB-UNIT-TYPE; 	Size:	1
			,SPACE(46) FILLER7											-- Campo 13: Filler; 			Size:	46
			,SP.IDSugestaoPedido
	  FROM  SugestaoPedido SP WITH (NOLOCK)
	  JOIN ItemDetalhe ID WITH (NOLOCK)
		ON ID.IdItemDetalhe = SP.IdItemDetalhePedido
	  JOIN Loja L WITH (NOLOCK)
		ON SP.IdLoja = L.IdLoja
	   AND L.blEmitePedido = 1
	   AND L.blEnviaPedidoOMS = 1
	  JOIN AutorizaPedido AP WITH (NOLOCK)
		ON AP.dtPedido = SP.dtPedido
	   AND AP.IdLoja = SP.IdLoja
	   AND AP.IdDepartamento = ID.IDDepartamento
	  JOIN RelacaoItemLojaCD RILC WITH (NOLOCK)
		ON RILC.IdItemEntrada = ID.IdItemDetalhe
	   AND RILC.blAtivo = 1
	  JOIN LojaCDParametro LCP WITH (NOLOCK)
		ON LCP.IdLojaCDParametro = RILC.IDLojaCDParametro
	   AND LCP.IdLoja = L.IDLoja
	  JOIN CD WITH (NOLOCK)
		ON CD.IdCd = LCP.IdCD
	   AND CD.blConvertido = 1
	 WHERE SP.dtPedido = CONVERT(DATE, GETDATE(), 103)
	   AND SP.blReturnSheet = 0
	   AND SP.qtdPackCompra > 0
	   AND SP.vlTipoReabastecimento IN (20, 22, 40, 42, 43, 81)

GO


