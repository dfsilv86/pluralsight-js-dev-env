/****** Object:  UserDefinedFunction [dbo].[fnAcaoRefCruzada]    Script Date: 05/10/2016 14:55:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[fnAcaoRefCruzada] (@IDRelacionamentoItemPrime INT, @TpManipulado CHAR, @TpVinculado CHAR, @TpReceituario CHAR, @TpCaixaFornecedor CHAR, @VlTipoReabastecimento INT)
RETURNS @retorno TABLE 
(
    Acao nvarchar(4000)
)
AS 
BEGIN
	DECLARE @VincularItemEntradaEmSGP VARCHAR(50) = 'Vincular item de entrada em Relacionamento SGP',
			@VerificarAdicaoItemEstocadoARefCruzada VARCHAR(80) = 'Verificar necessidade de adição do item (ESTOCADO) à referência cruzada OIF',
			@VerificarRemocaoItemDSDRefCruzada VARCHAR(80) = 'Verificar necessidade de remoção do item (DSD) da referência cruzada OIF',
			@VerificarRemocaoItemXDOCRefCruzada VARCHAR(80) = 'Verificar necessidade de remoção do item (XDOC) da referência cruzada OIF',
			
			@True INT = 1,
			@False INT = 0,
			
			@OIF INT,
			@SGP INT,
			@Estocado INT,
			@DSD INT,
			@XDOC INT;
	
	SET	@OIF = CASE WHEN (@IDRelacionamentoItemPrime IS NOT NULL) THEN @True ELSE @False END;
	SET @SGP = CASE WHEN (@TpManipulado IS NULL AND @TpVinculado IS NULL AND @TpReceituario IS NULL AND @TpCaixaFornecedor = 'F') THEN @False ELSE @True END;
	
	SET @Estocado = CASE WHEN @VlTipoReabastecimento IN (20, 22, 40, 42, 43) THEN @True ELSE @False END;
	SET @DSD = CASE WHEN (@VlTipoReabastecimento IN (7, 37)) THEN @True ELSE @False END;
	SET @XDOC = CASE WHEN (@VlTipoReabastecimento IN (3, 33)) THEN @True ELSE @False END;

    IF(@OIF = @True AND @SGP = @False AND @Estocado = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
	END
		
	IF(@OIF = @False AND @SGP = @True AND @Estocado = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VerificarAdicaoItemEstocadoARefCruzada);
	END	      
			    
    IF(@OIF = @False AND @SGP = @False AND @Estocado = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
		INSERT INTO @retorno VALUES (@VerificarAdicaoItemEstocadoARefCruzada);
	END	      
 
	IF(@OIF = @True AND @SGP = @True AND @DSD = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VerificarRemocaoItemDSDRefCruzada);
	END	  	
			
	IF(@OIF = @True AND @SGP = @False AND @DSD = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
		INSERT INTO @retorno VALUES (@VerificarRemocaoItemDSDRefCruzada);
	END	
			
	IF(@OIF = @False AND @SGP = @False AND @DSD = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
	END
	
	IF(@OIF = @True AND @SGP = @True  AND @XDOC = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VerificarRemocaoItemXDOCRefCruzada);
	END
		
	IF(@OIF = @True AND @SGP = @False AND @XDOC = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
		INSERT INTO @retorno VALUES (@VerificarRemocaoItemXDOCRefCruzada);
	END	
	
	IF(@OIF = @False AND @SGP = @False AND @XDOC = @True)
	BEGIN
		INSERT INTO @retorno VALUES (@VincularItemEntradaEmSGP);
	END
	
	IF((SELECT COUNT(1) FROM @retorno) = 0)
	BEGIN
		INSERT INTO @retorno VALUES (NULL);
	END

	RETURN
END