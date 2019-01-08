/*
=======================================================================================================================
Procedure..............: PR_AtualizaItemEstoqueCd
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 19/04/2016
Objetivo...............: Atualizar os dados da STAGE para a base SGP (PESS_DEV) 
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........:
Data Alteração.........:
Descrição da alteração.:
=======================================================================================================================
*/

CREATE PROCEDURE [dbo].[PR_AtualizaItemEstoqueCd]
AS
BEGIN
	TRUNCATE TABLE ItemEstoqueCD

	INSERT INTO ItemEstoqueCD (  idItemDetalhe
								,idCd
								,QtdOnHand
								,QtdOnOrder )
						SELECT   ID.idItemDetalhe
								,CD.idCd
								,QtdOnHand * CASE WHEN ID.tpCaixaFornecedor = 'V' THEN ID.vlPesoLiquido ELSE ID.qtVendorPackage END
								,QtdOnOrder * CASE WHEN ID.tpCaixaFornecedor = 'V' THEN ID.vlPesoLiquido ELSE ID.qtVendorPackage END
						  FROM  [WLMSLP_STAGE].[dbo].[ItemEstoqueCD] IE WITH (NOLOCK)
						  JOIN ItemDetalhe ID WITH (NOLOCK)
							ON ID.cdItem = IE.cdItem
						  JOIN CD WITH (NOLOCK)
							ON CD.cdCD = IE.cdCD								
END
