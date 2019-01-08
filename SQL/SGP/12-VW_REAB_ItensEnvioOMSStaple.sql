/*
=======================================================================================================================
View...................: VW_REAB_ItensEnvioOMSStaple
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 12/04/2016
Objetivo...............: Listar registros para o envio de pedidos OMS para STAPLE (Projeto PESS)
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........:
Data Alteração.........:
Descrição da alteração.:
=======================================================================================================================
*/

alter VIEW [dbo].[VW_REAB_ItensEnvioOMSStaple]
AS
WITH Registros
AS
(
	SELECT   F.cdFornecedor vendor_nbr
			,ID.cdDepartamentoVendor vendor_dept_nbr
			,ID.cdSequenciaVendor vendor_seq_nbr
			,ID.cdItem item_nbr
			,REPLICATE('0', 5 ) from_store_nbr
			,REPLICATE('0', 5 - LEN(cd.cdCD)) + RTrim(cd.cdCD) to_store_nbr
			,convert(INT, SP.qtdPackCompra * CASE WHEN ID.tpCaixaFornecedor = 'F' THEN SP.qtVendorPackage ELSE ID.vlPesoLiquido END) order_each_qty
			,'DC' source_type_code
			,'' mabd_date
			,'' mabd_time
			,'' dnsb_date
			,'' dnsa_date
			,CAST(SP.dtPedido AS VARCHAR) sched_ship_date
			,'00.00.00' sched_ship_time
			,CAST(DATEADD(D, SP.vlLeadTime, SP.dtPedido) AS VARCHAR) sched_arrival_date
			,'00.00.00' sched_arrive_time
			,SP.IDSugestaoPedidoCD
			,'N' tpMultisourcing
			,SP.idCD
			,ID.IDItemDetalhe
			,SP.qtdPackCompra
			,SP.vlLeadTime
	  FROM SugestaoPedidoCD SP WITH (NOLOCK)
	  JOIN CD ON CD.idCD = SP.idCD
	  JOIN ItemDetalhe ID WITH (NOLOCK)
	    ON ID.IdItemDetalhe = SP.IdItemDetalhePedido
	  JOIN Fornecedor F WITH (NOLOCK)
	    ON F.IdFornecedor = ID.IDFornecedor
	  JOIN RelacaoItemLojaCD RILC WITH (NOLOCK)
		ON RILC.IdItemEntrada = ID.IdItemDetalhe
	   AND RILC.blAtivo = 1
	 WHERE SP.dtEnvioPedido = CONVERT(DATE, GETDATE(), 103)
	   AND SP.qtdPackCompra > 0
	   AND SP.blFinalizado = 1
	   AND SP.vlTipoReabastecimento IN (20, 40, 22, 42, 43, 81)
	   
),
Totais
AS
(
	SELECT	vendor_nbr,
			vendor_dept_nbr,
			vendor_seq_nbr,
			item_nbr,
			'' from_store_nbr,			
			from_store_nbr to_store_nbr,			
			SUM(order_each_qty)  order_each_qty,
			'VN' source_type_code,
			sched_arrival_date mabd_date,
			sched_ship_time mabd_time,
			sched_ship_date dnsb_date,
			sched_arrival_date dnsa_date,
			'' sched_ship_date,
			'' sched_ship_time,
			'' sched_arrival_date,
			'' sched_arrive_time,
			NULL as IDSugestaoPedidoCD,
			NULL tpMultisourcing,
			NULL idCD,
			NULL IDItemDetalhe,
			NULL qtdPackCompra,
			NULL vlLeadTime
	FROM Registros
	GROUP BY	vendor_nbr,
				vendor_dept_nbr,
				vendor_seq_nbr,
				item_nbr,				
				from_store_nbr,							
				source_type_code,
				sched_ship_date,
				sched_ship_time,
				sched_ship_date,
				sched_arrival_date,
				sched_ship_date,
				sched_ship_time,
				sched_arrival_date,
				sched_arrive_time,
				tpMultisourcing,
				idCD,
				IDItemDetalhe,
				qtdPackCompra,
				vlLeadTime
),
FileLines
AS
(
	SELECT	*
	  FROM Registros
	  UNION ALL 
	 SELECT *
	   FROM Totais
)
SELECT						
		'00011' REQ_BATCH_SOURCE_CD,													-- Campo 01: REQ_BATCH_SOURCE_CD; 		Size:	5
		'007' order_office_id, 															-- Campo 02: order_office_id; 			Size:	3
		'01' base_div_nbr, 																-- Campo 03: base_div_nbr; 				Size:	2
		RIGHT(REPLICATE('0', 6) + RTrim(vendor_nbr), 6) vendor_nbr, 					-- Campo 04: vendor_nbr; 				Size:	6
		STR(vendor_dept_nbr, 2) vendor_dept_nbr, 										-- Campo 05: vendor_dept_nbr; 			Size:	2
		STR(vendor_seq_nbr, 1) vendor_seq_nbr, 											-- Campo 06: vendor_seq_nbr; 			Size:	1
		RIGHT(REPLICATE('0', 9) + RTrim(item_nbr), 9) item_nbr, 						-- Campo 07: item_nbr; 					Size:	9		
		'02' channel_mthd_cd, 															-- Campo 08: channel_mthd_cd; 			Size:	2
		RIGHT(REPLICATE('0', 5) + RTrim(from_store_nbr), 5) from_store_nbr,				-- Campo 09: from_store_nbr; 			Size:	5		
		'BR' from_country_code, 														-- Campo 10: from_country_code; 		Size:	2
		RIGHT(REPLICATE('0', 5) + RTrim(to_store_nbr), 5) to_store_nbr, 				-- Campo 11: to_store_nbr; 				Size:	5
		'BR' to_country_code, 															-- Campo 12: to_country_code; 			Size:	2
		RIGHT(REPLICATE('0', 9) + RTrim(order_each_qty), 9) order_each_qty,				-- Campo 13: order_each_qty; 			Size:	9
		RIGHT (REPLICATE(' ', 2) + source_type_code, 2) source_type_code,				-- Campo 14: source_type_code; 			Size:	2
		'         ' po_group_id,		 												-- Campo 15: po_group_id; 				Size:	9
		RIGHT (REPLICATE(' ', 20) + 'SGP REPLEN', 20) event_name,						-- Campo 16: event_name; 				Size:	20
		RIGHT (REPLICATE(' ', 10) + mabd_date, 10) mabd_date,							-- Campo 17: mabd_date; 				Size:	10
		RIGHT (REPLICATE(' ', 8) + mabd_time, 8) mabd_time, 							-- Campo 18: mabd_time; 				Size:	8
		RIGHT (REPLICATE(' ', 10) + dnsb_date, 10) dnsb_date,							-- Campo 19: dnsb_date; 				Size:	10
		RIGHT (REPLICATE(' ', 10) + dnsa_date, 10) dnsa_date,							-- Campo 20: dnsa_date; 				Size:	10
		RIGHT (REPLICATE(' ', 10) + sched_ship_date, 10) sched_ship_date,				-- Campo 21: sched_ship_date; 			Size:	10
		RIGHT (REPLICATE(' ', 8) + sched_ship_time, 8) sched_ship_time, 				-- Campo 22: sched_ship_time; 			Size:	8
		RIGHT (REPLICATE(' ', 10) + sched_arrival_date, 10) sched_arrival_date,			-- Campo 23: sched_arrival_date; 		Size:	10
		RIGHT (REPLICATE(' ', 8) + sched_arrive_time, 8) sched_arrive_time, 			-- Campo 24: sched_arrive_time; 		Size:	8
		'  ' msg_code,	 																-- Campo 25: msg_code; 					Size:	2
		'  ' demand_reason_code, 														-- Campo 26: demand_reason_code; 		Size:	2
		'N' firm_alloc_ind, 															-- Campo 27: firm_alloc_ind; 			Size:	1
		'N' substitution_ind, 															-- Campo 28: substitution_ind; 			Size:	1
		'N' manual_create_ind, 															-- Campo 29: manual_create_ind; 		Size:	1
		RIGHT (REPLICATE(' ', 8) + 'SG0', 8) create_userid, 							-- Campo 30: create_userid; 			Size:	8
		RIGHT (REPLICATE(' ', 8) + 'SG0', 8) create_system_id, 							-- Campo 31: create_system_id; 			Size:	8
		'N' dynamic_distr_ind, 															-- Campo 32: dynamic_distr_ind; 		Size:	1
		'N' ltl_ind, 																	-- Campo 33: ltl_ind; 					Size:	1
		REPLICATE('0', 2 - LEN('0')) + RTrim('0') mdse_div_nbr,							-- Campo 34: mdse_div_nbr; 				Size:	2
		'N' tab_event_ind, 																-- Campo 35: tab_event_ind; 			Size:	1
		'N' SEASONAL_IND, 																-- Campo 36: SEASONAL_IND; 				Size:	1
		'N' SAMPLE_PO_IND, 																-- Campo 37: SAMPLE_PO_IND; 			Size:	1
		REPLICATE('0', 4 - LEN('1')) + RTrim('1') priority_nbr,							-- Campo 38: priority_nbr; 				Size:	4
		'         ' external_system_order_id,											-- Campo 39: external_system_order_id;	Size:	10
		'            ' override_vnpk_cost,												-- Campo 40: override_vnpk_cost; 		Size:	13
		REPLICATE(' ', 185) Filler,														-- Campo 41: Filler; 					Size:	185
		IDSugestaoPedidoCD,
		tpMultisourcing,
		idCD,
		IDItemDetalhe,
		qtdPackCompra,
		vlLeadTime
 FROM FileLines

GO


