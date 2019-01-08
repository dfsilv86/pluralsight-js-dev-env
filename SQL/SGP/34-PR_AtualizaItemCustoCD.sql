/*
=======================================================================================================================
Procedure..............: PR_AtualizaItemCustoCD
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 29/04/2016
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
create PROCEDURE [dbo].[PR_AtualizaItemCustoCD]
AS
BEGIN
	--TRUNCATE TABLE ItemCustoCD

	-- Insere registros novos
	INSERT INTO ItemCustoCD(idCD
						   ,idItemDetalhe
						   ,BuyPrice
						   ,dtRecebimento
						   ,dhcriacao)
					SELECT  CD.idCD
						   ,ID.IDItemDetalhe
						   ,IC.BuyPrice
						   ,IC.DtRecebimento
						   ,GETDATE()
					  FROM WLMSLP_STAGE.dbo.ItemCustoCD IC
					  JOIN ItemDetalhe ID 
						ON ID.cdItem = IC.cdItem
					  JOIN CD
					    ON CD.dsUF = IC.UF
					 WHERE NOT EXISTS ( SELECT 1 FROM ItemCustoCD ICC 
												WHERE ICC.idItemDetalhe = ID.IDItemDetalhe
												  AND ICC.idCD = CD.idCD
												  AND ICC.DtRecebimento = IC.DtRecebimento)

	-- Atualiza registros já existentes
	UPDATE  ICC
	   SET  BuyPrice = IC.BuyPrice
		   ,dhCriacao = GETDATE()
	  FROM 	ItemCustoCD ICC
	  JOIN  ItemDetalhe ID 
		ON  ID.IDItemDetalhe = ICC.idItemDetalhe
	  JOIN  CD
		ON  CD.idCD = ICC.idCD
	  JOIN  WLMSLP_STAGE.dbo.ItemCustoCD IC
	    ON  IC.cdItem = ID.cdItem
	   AND  IC.UF = CD.dsUF
	   AND  IC.DtRecebimento = ICC.DtRecebimento
	   AND  IC.BuyPrice <> ICC.BuyPrice
												  
END
