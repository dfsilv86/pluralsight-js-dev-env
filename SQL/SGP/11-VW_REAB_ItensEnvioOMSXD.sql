/*
=======================================================================================================================
View...................: VW_REAB_ItensEnvioOMSXD
Autor..................: 
Data de criação........: 
Objetivo...............: Listar registros para o envio de pedidos OMS para XDoc
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

create VIEW [dbo].[VW_REAB_ItensEnvioOMSXD]
AS
WITH Registros
AS
(
	SELECT ISNULL(MS.cdFornecedor, F.cdFornecedor) vendor_nbr
		,ISNULL(MS.cdDepartamentoVendor, ID.cdDepartamentoVendor) vendor_dept_nbr
		,ISNULL(MS.cdSequenciaVendor, ID.cdSequenciaVendor) vendor_seq_nbr
		,ISNULL(MS.cdItem, ID.cdItem) item_nbr
		,REPLICATE('0', 5 - LEN(cd.cdCD)) + RTrim(cd.cdCD) from_store_nbr
		,L.cdLoja to_store_nbr
		,convert(INT, SP.qtdPackCompra * SP.qtVendorPackage * isnull(MS.vlPercentual, 100) / 100) order_each_qty
		,'DC' source_type_code
		,'' mabd_date
		,'' mabd_time
		,'' dnsb_date
		,'' dnsa_date
		,CAST(SP.dtPedido AS VARCHAR) sched_ship_date
		,'00.00.00' sched_ship_time
		,CAST(DATEADD(D, SP.vlLeadTime, SP.dtPedido) AS VARCHAR) sched_arrival_date
		,'00.00.00' sched_arrive_time
		,SP.IDSugestaoPedido
		,ISNULL(MS.tpMultisourcing, 'N') tpMultisourcing
		,CD.idCD
		,ISNULL(MS.IDItemDetalhe, ID.IDItemDetalhe) IDItemDetalhe
		,SP.qtdPackCompra
		,SP.vlLeadTime
	FROM SugestaoPedido SP WITH (NOLOCK)
	JOIN ItemDetalhe ID WITH (NOLOCK)
		ON ID.IdItemDetalhe = SP.IdItemDetalhePedido
	LEFT JOIN (SELECT 	'S' tpMultisourcing
						,ITS.IDItemDetalhe ITS_IDItemDetalhe
						,MS.vlPercentual
						,ID.cdDepartamentoVendor
						,ID.cdSequenciaVendor
						,ID.cdItem
						,ID.IdItemDetalhe
						,F.cdFornecedor
						,ID.IDDepartamento
					FROM RelacionamentoItemSecundario ITS WITH (NOLOCK)
					JOIN RelacionamentoItemSecundario RS WITH (NOLOCK)
						ON RS.IDRelacionamentoItemPrincipal = ITS.IDRelacionamentoItemPrincipal
					JOIN Multisourcing MS WITH (NOLOCK)
						ON MS.IDRelacionamentoItemSecundario = RS.IDRelacionamentoItemSecundario
					JOIN ItemDetalhe ID WITH (NOLOCK)
						ON ID.IDItemDetalhe = RS.IDItemDetalhe
					JOIN Fornecedor F WITH (NOLOCK)
						ON F.IdFornecedor = ID.IDFornecedor ) MS
		    ON MS.ITS_IDItemDetalhe = ID.IDItemDetalhe
	JOIN Fornecedor F WITH (NOLOCK)
		ON F.IdFornecedor = ID.IDFornecedor
	JOIN Loja L WITH (NOLOCK)
		ON SP.IdLoja = L.IdLoja
			AND L.blEmitePedido = 1
			AND L.blEnviaPedidoOMS = 1
			AND L.blAtivo = 1
  JOIN FornecedorParametro FP WITH (NOLOCK) 
		 ON FP.IdFornecedorParametro = SP.IdFornecedorParametro
		 AND FP.tpStoreApprovalRequired IN ('Y', 'R')
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
	CROSS APPLY dbo.fnBuscaGradeFechada(L.IDBandeira, ID.IDDepartamento, SP.IDLoja) GS
	WHERE SP.dtPedido = CONVERT(DATE, GETDATE(), 103)
		AND SP.blAtendePedidoMinimo = 1
		AND SP.qtdPackCompra > 0
		AND SP.tpStatusEnvio IS NULL
		AND (
			SP.vlTipoReabastecimento IN (33)
			OR (
				CD.blConvertido = 1
				AND SP.vlTipoReabastecimento IN (03, 94)
				)
			)
),
Multisourcing
AS
(
	SELECT	vendor_nbr,
			vendor_dept_nbr,
			vendor_seq_nbr,
			item_nbr,
			from_store_nbr from_store_nbr,			
			'' to_store_nbr,			
			SUM(order_each_qty)  order_each_qty,
			'MS' source_type_code,
			sched_arrival_date mabd_date,
			sched_ship_time mabd_time,
			sched_ship_date dnsb_date,
			sched_arrival_date dnsa_date,
			'' sched_ship_date,
			'' sched_ship_time,
			'' sched_arrival_date,
			'' sched_arrive_time,
			NULL as IDSugestaoPedido	,
			tpMultisourcing,
			idCD,
			IDItemDetalhe,
			qtdPackCompra,
			vlLeadTime
	FROM Registros
	WHERE tpMultisourcing = 'S'
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
			NULL as IDSugestaoPedido	,
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
	  UNION ALL 
	 SELECT *
	   FROM Multisourcing
)
SELECT						
		'00011' REQ_BATCH_SOURCE_CD,													-- Campo 01: REQ_BATCH_SOURCE_CD; 		Size:	5
		'007' order_office_id, 															-- Campo 02: order_office_id; 			Size:	3
		'01' base_div_nbr, 																-- Campo 03: base_div_nbr; 				Size:	2
		RIGHT(REPLICATE('0', 6) + RTrim(vendor_nbr), 6) vendor_nbr, 					-- Campo 04: vendor_nbr; 				Size:	6
		STR(vendor_dept_nbr, 2) vendor_dept_nbr, 										-- Campo 05: vendor_dept_nbr; 			Size:	2
		STR(vendor_seq_nbr, 1) vendor_seq_nbr, 											-- Campo 06: vendor_seq_nbr; 			Size:	1
		RIGHT(REPLICATE('0', 9) + RTrim(item_nbr), 9) item_nbr, 						-- Campo 07: item_nbr; 					Size:	9		
		'01' channel_mthd_cd, 															-- Campo 08: channel_mthd_cd; 			Size:	2
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
		IDSugestaoPedido,
		tpMultisourcing,
		idCD,
		IDItemDetalhe,
		qtdPackCompra,
		vlLeadTime
 FROM FileLines
 --ORDER BY vendor_nbr, vendor_dept_nbr, vendor_seq_nbr, item_nbr, from_store_nbr DESC


GO


