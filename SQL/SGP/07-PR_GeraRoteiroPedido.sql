/*
=======================================================================================================================
Procedure..............: PR_GeraRoteiroPedido
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 12/04/2016
Objetivo...............: Vincular pedidos da sugestão ao roteiro
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........:
Data Alteração.........:
Descrição da alteração.:
=======================================================================================================================
*/

CREATE PROCEDURE [dbo].[PR_GeraRoteiroPedido]
AS
BEGIN

	IF OBJECT_ID('Tempdb..#tmpRoteiroPedido') IS NOT NULL 
		DROP TABLE #tmpRoteiroPedido

	SELECT DISTINCT  RT.idRoteiro
					,SP.IDSugestaoPedido
			  INTO #tmpRoteiroPedido
			  FROM SugestaoPedido SP WITH (NOLOCK)
			  JOIN Loja LJ WITH (NOLOCK) 
				ON LJ.IDLoja = SP.IdLoja
			  JOIN FornecedorParametro FP WITH (NOLOCK)
				ON FP.IDFornecedorParametro = SP.IDFornecedorParametro
			   AND FP.blAtivo = 1
			   AND FP.tpStoreApprovalRequired IN ('Y', 'R')
			  JOIN Roteiro RT WITH (NOLOCK)
				ON RT.blAtivo = 1
			   AND RT.cdV9D = FP.cdV9D
			  JOIN RoteiroLoja RL WITH (NOLOCK)
				ON RL.idRoteiro = RT.idRoteiro
			   AND RL.blativo = 1
			   AND RL.idloja = SP.IdLoja
		 LEFT JOIN FornecedorLojaParametro FLP WITH (NOLOCK)
				ON FLP.idLoja = RL.idLoja
			   AND FLP.IDFornecedorParametro = SP.IDFornecedorParametro
			  JOIN ItemDetalhe IDE WITH (NOLOCK)
				ON IDE.IdItemDetalhe = SP.IDItemDetalhePedido
			   AND IDE.blAtivo = 1
			   AND IDE.IDFornecedorParametro = FP.IDFornecedorParametro						   
			   AND IDE.tpVinculado = 'E'
			   AND IDE.tpStatus = 'A'
			   AND IDE.cdSistema = 1
			   AND IDE.blItemTransferencia = 0
			  JOIN AutorizaPedido AP WITH (NOLOCK)
				ON AP.dtPedido = SP.dtPedido
			   AND AP.IdLoja = SP.IdLoja
			   AND AP.IdDepartamento = IDE.IDDepartamento
		CROSS APPLY dbo.fnBuscaGradeFechada(LJ.IDBandeira, IDE.IDDepartamento, SP.IDLoja) GS
			 WHERE SP.vlTipoReabastecimento IN (7, 37, 97)
			   AND SP.blReturnSheet = 0
			   AND SP.tpStatusEnvio IS NULL
			   AND SP.qtdPackCompra > 0
			   AND CHARINDEX(CONVERT(CHAR(1), DATEPART(WEEKDAY, SP.dtPedido)), ISNULL(FLP.cdReviewDate, FP.cdReviewDate), 1) > 0

	INSERT INTO RoteiroPedido ( idRoteiro
							   ,idSugestaoPedido )
						SELECT  idRoteiro
							   ,idSugestaoPedido 
						  FROM #tmpRoteiroPedido
						  
	UPDATE SugestaoPedido
	  SET tpStatusEnvio = 'E', dhEnvioSugestao = GETDATE(), qtdSugestaoRoteiroRA=qtdPackCompra
	  WHERE IDSugestaoPedido IN (SELECT IDSugestaoPedido FROM #tmpRoteiroPedido )
END
