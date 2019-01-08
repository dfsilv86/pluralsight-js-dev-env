/*
=======================================================================================================================
Function...............: fnValidarTipoReabastecimentoXCDConvertido
Autor..................: Rafael de Souza Bueno (CWI)
Data de criação........: 
Objetivo...............: Validar o tipo de reabastecimento do item para XDoc e CD convertido
Parâmetros.............: @idItemDetalhe = ID do item
						 @idCD = ID do CD
						 @idLoja = ID do loja
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: Evandro Henrique Dapper (CWI)
Data Alteração.........: 14/04/2016
Descrição da alteração.: Acrescentado o parâmetro do ID loja devido a alteração nos parâmetros da função
						 fnObterTipoReabastecimento (Projeto PESS)

=======================================================================================================================
*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fnValidarTipoReabastecimentoXCDConvertido](@idItemDetalhe BIGINT, @idCD INT, @idLoja INT)
RETURNS BIT
BEGIN
	
	DECLARE @tipoReabastecimento INT
	DECLARE @blConvertido BIT
	
	SELECT @tipoReabastecimento = [dbo].[fnObterTipoReabastecimento](@idItemDetalhe, @idCD, @idLoja)
	
	SELECT @blConvertido = cd.blConvertido
	FROM CD cd WITH (NOLOCK)
	WHERE cd.idCD = @idCD
	
	IF(@tipoReabastecimento IN(03, 33, 20, 22, 40, 42, 43, 94, 81) AND @blConvertido = 1)
	BEGIN
		RETURN 1
	END

	RETURN 0
END