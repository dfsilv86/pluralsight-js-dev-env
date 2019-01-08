/*
DECLARE @idLoja INT, @idItemDetalhe INT;
SET @idLoja = 2;
SET @idItemDetalhe = 296;
--*/

SELECT TOP 1 ISNULL(
                    CASE dbo.fnObterCustoContabilAtual(@idLoja, @idItemDetalhe) 
                         WHEN 0 THEN dbo.fn_RetornavlCustoCadastroAtual(@idLoja, @idItemDetalhe) 
                         ELSE dbo.fnObterCustoContabilAtual(@idLoja, @idItemDetalhe) 
                    END, 0)