

ALTER FUNCTION [dbo].[fnPossuiVendasPeriodo](@IdLoja INT, @IdItem INT, @DataInicial DATETIME, @DataFinal DATETIME)  
RETURNS BIT  
AS  
BEGIN  

	DECLARE @RETORNO BIT = 0	


	IF @DataInicial IS NULL 
	BEGIN
		SELECT  @DataInicial = CONVERT(date, getdate() - 8)
	END;
	
	IF @DataFinal IS NULL
	BEGIN
		SELECT @DataFinal = CONVERT(date, getdate() - 1)
	END;
	 
		if EXISTS (	
			SELECT IDMovimentacao
			FROM Movimentacao WITH (NOLOCK)
			WHERE IDLoja = @IdLoja
			AND IdItemOperacao = @IdItem
			AND dtMovimentado BETWEEN @DataInicial AND @DataFinal
			AND IDTipoMovimentacao IN (5,6,7,8)
			)
			begin
			set @RETORNO = 1
			end

  RETURN @RETORNO  

END











GO


