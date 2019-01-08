
ALTER FUNCTION [dbo].[fnTotalNotaNoPeriodo]
(                                                               
           @IdItemDetalhe INT            
         , @IdLoja        INT            
         , @DataInicial   DATETIME       
		 , @DataFinal   DATETIME                                                       
)                                                                          
RETURNS NUMERIC(18,0)                                                                                  
AS                                                                          
BEGIN                                                      
                                                                              
    DECLARE  @Retorno                        NUMERIC(18,0) = 0

	SELECT @Retorno = COUNT(*)
	FROM NotaFiscal NF WITH(NOLOCK)
	INNER JOIN TipoNotaFiscal TNF WITH(NOLOCK)
		ON TNF.Id = NF.IDTipoNota
		AND TNF.sgTipo = 'DS'
	INNER JOIN NotaFiscalItem NFI WITH(NOLOCK)
		ON NFI.IDNotaFiscal = NF.IDNotaFiscal
	WHERE NF.dtRecebimento BETWEEN @DataInicial AND @DataFinal
		AND NF.IDLoja = @IdLoja
		AND NFI.IDItemDetalhe = @IdItemDetalhe
	GROUP BY NF.IDNotaFiscal                                           	
                                                                                                           
    RETURN @Retorno 
                                                                           
END   
  



