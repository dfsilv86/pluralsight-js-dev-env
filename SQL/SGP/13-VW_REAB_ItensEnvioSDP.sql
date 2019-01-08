/*
=======================================================================================================================
View...................: VW_REAB_ItensEnvioSDP
Autor..................: 
Data de criação........: 
Objetivo...............: Listar registros para o envio de pedidos OMS para DSD
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: Evandro Henrique Dapper (CWI)
Data Alteração.........: 11/04/2016
Descrição da alteração.: Verificar o tipo de abastecimento pela coluna vlTipoReabastecimento 
						 criada na tabela SugestaoPedido 
=======================================================================================================================
*/

ALTER VIEW [dbo].[VW_REAB_ItensEnvioSDP]
AS
	SELECT   SP.IDSugestaoPedido
			,LJ.cdLoja
			,DP.cdDepartamento
			,ID.cdItem
			,ID.vlTipoReabastecimento
			,CASE 
				WHEN DP.cdDepartamento = 94
					THEN DATEADD(DAY, 1, SP.dtPedido)
				ELSE SP.dtPedido
				END dtPedido
			,SP.qtdPackCompraOriginal
			,SP.qtdPackCompra
			,ID.IDItemDetalhe
			,LJ.IDLoja
	  FROM SugestaoPedido SP WITH (NOLOCK)
	  JOIN ItemDetalhe ID WITH (NOLOCK)
		ON SP.IDItemDetalhePedido = ID.IDItemDetalhe
	   AND ID.vlTipoReabastecimento IN (22, 42, 43, 3)
	  JOIN Loja LJ WITH (NOLOCK)
		ON LJ.IDLoja = SP.IDLoja
	   AND LJ.blEmitePedido = 1
	  JOIN Departamento DP WITH (NOLOCK)
		ON DP.IDDepartamento = ID.IDDepartamento
	  JOIN FornecedorParametro FP WITH (NOLOCK)
		ON FP.IdFornecedorParametro = SP.IdFornecedorParametro
	   AND FP.tpStoreApprovalRequired IN ('Y', 'R')
	  JOIN AutorizaPedido AP WITH (NOLOCK)
		ON AP.dtPedido = SP.dtPedido
	  JOIN LojaCDParametro LCP
		ON LCP.IDLoja = LJ.IDLoja
	   AND LCP.blAtivo = 1
	  JOIN CD
		ON CD.IDCD = LCP.IDCD
	   AND CD.blAtivo = 1
	   AND CD.blConvertido = 1
	 CROSS APPLY dbo.fnBuscaGradeFechada(LJ.IDBandeira, ID.IDDepartamento, SP.IDLoja) GS
	 WHERE SP.dtPedido = CONVERT(DATE, GETDATE(), 103)
	   AND SP.qtdPackCompra > 0
	   AND SP.tpStatusEnvio IS NULL	   
	   AND AP.IdDepartamento = DP.IdDepartamento
	   AND AP.IdLoja = SP.IdLoja
GO
