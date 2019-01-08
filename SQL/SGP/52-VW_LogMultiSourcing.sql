/*
=======================================================================================================================
View...................: VW_LogMultiSourcing
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 14/04/2016 (Projeto PESS)
Objetivo...............: Mostrar log de alterações da tabela Multisourcing
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


CREATE  VIEW [dbo].[VW_LogMultiSourcing]
AS

SELECT	ITE.cdItem CodItemEntrada, 
		ITE.dsItem DescItemEntrada, 
		CONVERT(VARCHAR, FR.cdfornecedor) + CONVERT(VARCHAR, DP.cddepartamento) + CONVERT(VARCHAR, ITE.cdSequenciaVendor) cd9DVendor,
		CASE WHEN dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, LM.idCD, NULL) IN (20, 22, 40, 42, 43) THEN 'W'
	        ELSE CASE WHEN dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, LM.idCD, NULL) IN (3, 7, 37, 33) THEN 'D'
			ELSE 'L' END
		END CanalVendor,
		FR.nmFornecedor	NomeVendor,
		ITS.cdItem CodItemSaida,
		ITS.dsItem DescItemSaida,
		CD.cdCD CD,
		CONVERT(INT,LM.PercAnterior) PercentualCadastrado,
		LM.Observacao
  FROM	LogMultiSourcing LM WITH (NOLOCK)
  JOIN	ItemDetalhe ITE WITH (NOLOCK) ON ITE.IDItemDetalhe = LM.IdItemDetalheEntrada
  JOIN	Fornecedor FR WITH (NOLOCK) ON FR.IDFornecedor = ITE.IDFornecedor
  JOIN	Departamento DP WITH (NOLOCK) ON DP.IDDepartamento = ITE.IDDepartamento
  JOIN	ItemDetalhe ITS WITH (NOLOCK) ON ITS.IDItemDetalhe = LM.IdItemDetalheSaida
  JOIN	CD WITH (NOLOCK) ON CD.IDCD = LM.IdCd
 WHERE  LM.TpOperacao = 'E' -- Exclusão
   	AND	LM.IdUsuario = 3 -- Root
   	--AND CONVERT(CHAR(10), LM.DATA, 111) = '2016/03/07'
GO


