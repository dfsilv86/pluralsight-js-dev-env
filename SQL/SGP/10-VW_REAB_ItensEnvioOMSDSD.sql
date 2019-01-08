/*
=======================================================================================================================
View...................: VW_REAB_ItensEnvioOMSDSD
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
						 criada na tabela SugestaoPedido (Projeto PESS)
=======================================================================================================================
*/

create VIEW [dbo].[VW_REAB_ItensEnvioOMSDSD]
AS

SELECT	'00011' REQ_BATCH_SOURCE_CD,																-- Campo 01: REQ_BATCH_SOURCE_CD; 		Size:	5
		'007' order_office_id, 																		-- Campo 02: order_office_id; 			Size:	3
		'01' base_div_nbr, 																			-- Campo 03: base_div_nbr; 				Size:	2
		RIGHT(REPLICATE('0', 6) + RTrim(F.cdFornecedor), 6) vendor_nbr,								-- Campo 04: vendor_nbr; 				Size:	6
		STR(ID.cdDepartamentoVendor, 2) vendor_dept_nbr, 											-- Campo 05: vendor_dept_nbr; 			Size:	2
		STR(ID.cdSequenciaVendor, 1) vendor_seq_nbr, 												-- Campo 06: vendor_seq_nbr; 			Size:	1
		RIGHT(REPLICATE('0', 9) + RTrim(ID.cdItem), 9) item_nbr,									-- Campo 07: item_nbr; 					Size:	9		
		'03' channel_mthd_cd,																		-- Campo 08: channel_mthd_cd; 			Size:	2		 
		SPACE(5) from_store_nbr,																	-- Campo 09: from_store_nbr; 			Size:	5
		SPACE(2) from_country_code,																	-- Campo 10: from_country_code; 		Size:	2
		RIGHT(REPLICATE('0', 5) + RTrim(L.cdLoja), 5) to_store_nbr, 								-- Campo 11: to_store_nbr; 				Size:	5		
		'BR' to_country_code, 																		-- Campo 12: to_country_code; 			Size:	2
		RIGHT(REPLICATE('0', 9) + RTrim(SP.qtdPackCompra * SP.qtVendorPackage), 9) order_each_qty,	-- Campo 13: order_each_qty; 			Size:	9		 	
		'VN' source_type_code, 																		-- Campo 14: source_type_code; 			Size:	2
		'000000000' po_group_id, 																	-- Campo 15: po_group_id; 				Size:	9
		LEFT('SGP REPLEN' + SPACE(20), 20) event_name,												-- Campo 16: event_name; 				Size:	20
		LEFT(DATEADD(D, SP.vlLeadTime + 2, SP.dtPedido), 10) mabd_date,								-- Campo 17: mabd_date; 				Size:	10
		'00.00.00' mabd_time, 																		-- Campo 18: mabd_time; 				Size:	8
		LEFT(SPACE(10) + CONVERT(VARCHAR(10), SP.dtPedido, 120), 10) dnsb_date,						-- Campo 19: dnsb_date; 				Size:	10
		LEFT(DATEADD(D, SP.vlLeadTime + 1, SP.dtPedido), 10) dnsa_date,								-- Campo 20: dnsa_date; 				Size:	10
		SPACE(10) sched_ship_date,																	-- Campo 21: sched_ship_date; 			Size:	10
		SPACE(8) sched_ship_time, 																	-- Campo 22: sched_ship_time; 			Size:	8
		SPACE(10) sched_arrival_date,																-- Campo 23: sched_arrival_date; 		Size:	10
		SPACE(8) sched_arrive_time, 																-- Campo 24: sched_arrive_time; 		Size:	8
		'96' msg_code, 																				-- Campo 25: msg_code; 					Size:	2
		SPACE(2) demand_reason_code, 																-- Campo 26: demand_reason_code; 		Size:	2
		'N' firm_alloc_ind, 																		-- Campo 27: firm_alloc_ind; 			Size:	1
		'N' substitution_ind, 																		-- Campo 28: substitution_ind; 			Size:	1
		'N' manual_create_ind, 																		-- Campo 29: manual_create_ind; 		Size:	1
		LEFT('SG0' + SPACE(8), 8) create_userid, 													-- Campo 30: create_userid; 			Size:	8
		LEFT('SG0' + SPACE(8), 8) create_system_id, 												-- Campo 31: create_system_id; 			Size:	8
		'N' dynamic_distr_ind, 																		-- Campo 32: dynamic_distr_ind; 		Size:	1
		'N' ltl_ind, 																				-- Campo 33: ltl_ind; 					Size:	1
		REPLICATE('0', 2 - LEN('0')) + RTrim('0') mdse_div_nbr,										-- Campo 34: mdse_div_nbr; 				Size:	2
		'N' tab_event_ind, 																			-- Campo 35: tab_event_ind; 			Size:	1
		'N' SEASONAL_IND, 																			-- Campo 36: SEASONAL_IND; 				Size:	1
		'N' SAMPLE_PO_IND, 																			-- Campo 37: SAMPLE_PO_IND; 			Size:	1
		REPLICATE('0', 4 - LEN('1')) + RTrim('1') priority_nbr,										-- Campo 38: priority_nbr; 				Size:	4
		SPACE(10) external_system_order_id,															-- Campo 39: external_system_order_id;	Size:	10
		SPACE(13) override_vnpk_cost,																-- Campo 40: override_vnpk_cost; 		Size:	13
		SPACE(185) Filler,																			-- Campo 41: Filler; 					Size:	185
		SP.IDSugestaoPedido
FROM SugestaoPedido SP WITH (NOLOCK) INNER JOIN ItemDetalhe ID WITH (NOLOCK) 
											 ON ID.IdItemDetalhe = SP.IdItemDetalhePedido
											 AND ID.blAtivo = 1
									 INNER JOIN Fornecedor F WITH (NOLOCK) 
											 ON F.IdFornecedor = ID.IDFornecedor
											 AND F.blAtivo = 1
									 INNER JOIN Loja L WITH (NOLOCK) 
											 ON SP.IdLoja = L.IdLoja
											AND L.blEmitePedido = 1
											AND L.blEnviaPedidoOMS = 1
											AND L.blAtivo = 1
									 INNER JOIN FornecedorParametro FP WITH (NOLOCK) 
											 ON FP.IdFornecedorParametro = SP.IdFornecedorParametro
											 AND FP.blAtivo = 1
											 AND FP.tpStoreApprovalRequired IN ('Y', 'R')
									 INNER JOIN AutorizaPedido AP WITH (NOLOCK) 
											 ON AP.dtPedido = SP.dtPedido
											AND AP.IdLoja = SP.IdLoja
											AND AP.IdDepartamento = ID.IDDepartamento											
									  LEFT JOIN RoteiroPedido RP 
									         ON RP.idSugestaoPedido = SP.IDSugestaoPedido	
									CROSS APPLY dbo.fnBuscaGradeFechada(L.IDBandeira, ID.IDDepartamento, SP.IDLoja) GS
WHERE SP.dtPedido = CONVERT(DATE, GETDATE(), 103)
  AND SP.blAtendePedidoMinimo = 1
  AND SP.qtdPackCompra > 0
  AND SP.tpStatusEnvio IS NULL
  AND SP.vlTipoReabastecimento IN (7, 37, 97)
  AND ISNULL( RP.blAutorizado, 1 ) = 1

GO
