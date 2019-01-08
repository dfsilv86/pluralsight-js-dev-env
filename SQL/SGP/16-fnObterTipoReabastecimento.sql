/****** Object:  UserDefinedFunction [dbo].[fnObterTipoReabastecimento]    Script Date: 04/30/2016 10:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
=======================================================================================================================
Autor..................: Rafael de Souza Bueno (CWI)
Data de criação........: 
Objetivo...............: Obter o tipo de reabastecimento do item
Parâmetros.............: @idItemDetalhe = ID do item
						 @idCD = ID do CD
						 @idLoja = ID do loja
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: Evandro Henrique Dapper (CWI)
Data Alteração.........: 13/04/2016
Descrição da alteração.: Acrescentada a verificação do CD convertido e do canal (Projeto PESS)

Alterado por...........: Evandro Henrique Dapper (CWI)
Data Alteração.........: 14/04/2016
Descrição da alteração.: Acrescentado o parâmetro do ID loja e a verificação do canal na tabela 
						 fornecedorlojaparametro (Projeto PESS)

Alterado por...........: Evandro Henrique Dapper (CWI)
Data Alteração.........: 30/04/2016
Descrição da alteração.: Retirada a verificação do canal na tabela fornecedorlojaparametro (Projeto PESS)
Observação.............: O parâmetro @idLoja foi mantido para evitar alterações no SGP e PWC

=======================================================================================================================
*/

create FUNCTION [dbo].[fnObterTipoReabastecimento](@idItemDetalhe BIGINT, @idCD INT, @idLoja INT)
RETURNS SMALLINT
BEGIN
	DECLARE @tpReabastecimento SMALLINT, @tpReabastecimentoForn SMALLINT, @IdFornecedorParametro INT
	DECLARE @blConvertido BIT
	DECLARE @Canal CHAR(1), @cdTipo CHAR(1)

	SELECT @tpReabastecimento = ID.vlTipoReabastecimento, 
	       @Canal = FP.cdTipo,
	       @IdFornecedorParametro = ID.IDFornecedorParametro
	  FROM ItemDetalhe ID WITH (NOLOCK)
 LEFT JOIN FornecedorParametro FP WITH (NOLOCK) ON FP.IDFornecedorParametro = ID.IDFornecedorParametro
	 WHERE ID.IDItemDetalhe = @IdItemDetalhe	
	
	SELECT @blConvertido = blConvertido
	  FROM CD WITH (NOLOCK)
	  WHERE CD.idCD = @idCD;

	IF @blConvertido = 1 AND @IdFornecedorParametro IS NOT NULL
		BEGIN
	  
			SELECT @cdTipo = FP.cdTipo
			  FROM FornecedorParametro FP WITH (NOLOCK)
			 WHERE FP.IDFornecedorParametro = @IdFornecedorParametro
			
			SET @Canal = ISNULL(@cdTipo, @Canal)
			
			IF @Canal = 'L' 	  
				BEGIN
					SELECT @tpReabastecimentoForn = RIF.TipoReabastecimento
					  FROM ReabastecimentoItemFornecedorCD RIF WITH (NOLOCK)
					 WHERE RIF.IDCD = @idCD
					   AND RIF.IDFornecedorParametro = @idFornecedorParametro
					   AND RIF.IdItemDetalhe = @idItemDetalhe
					
					IF @tpReabastecimentoForn IS NULL
						SELECT @tpReabastecimentoForn = rf.TipoReabastecimento
						  FROM ReabastecimentoFornecedorCD rf WITH (NOLOCK)
						 WHERE rf.IDCD = @idCD
						   AND rf.IDFornecedorParametro = @idFornecedorParametro
						   
					IF @tpReabastecimentoForn IS NOT NULL
						SET @tpReabastecimento = @tpReabastecimentoForn
				END
		END
	
	RETURN @tpReabastecimento
END