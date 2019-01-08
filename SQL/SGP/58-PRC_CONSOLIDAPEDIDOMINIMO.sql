SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[PRC_CONSOLIDAPEDIDOMINIMO] @DataPedido DATE, @Particao INT
AS 
BEGIN
--
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON 

	DECLARE	@IDLoja INT,
			@IDFornecedorParametro BIGINT,
			@IdDiv INT,
			@cdLojaInicial INT,
			@cdLojaFinal INT


	IF @DataPedido IS NULL 
	BEGIN 
		SELECT @DataPedido = CAST(GETDATE() AS DATE)
	END

	-- Defini Particoes para execucao em paralelo da consolidacao
	
	SELECT @IdDiv = COUNT(*) / 20
	  FROM Loja L
	 WHERE L.blCalculaSugestao = 1;

	WITH Lojas
	AS
	(
		SELECT L.cdLoja, 
			   ROW_NUMBER() OVER(ORDER BY L.cdLoja) AS Row
		  FROM Loja L
		 WHERE L.blCalculaSugestao = 1
	)
	SELECT @cdLojaInicial = MIN(L.cdLoja), @cdLojaFinal = MAX(L.cdLoja)
	  FROM Lojas L
	 WHERE L.Row BETWEEN  (@IdDiv * (@Particao - 1)) + 1 AND @IdDiv * @Particao 
	 
	 -- Fim Definicao Particao


	DECLARE VendorsPorLoja CURSOR FOR
	SELECT DISTINCT SP.IdLoja, SP.IDFornecedorParametro, SP.dtPedido
	  FROM SugestaoPedido SP INNER JOIN ItemDetalhe ID 
									 ON SP.IDItemDetalhePedido = ID.IDItemDetalhe
									AND ID.vlTipoReabastecimento IN (33,37,7)
							 INNER JOIN Loja L 
									 ON L.IdLoja = SP.IdLoja									
	 WHERE SP.dtPedido = @DataPedido
	   AND L.cdLoja BETWEEN @cdLojaInicial AND @cdLojaFinal 
	   ORDER BY SP.IdLoja, SP.IDFornecedorParametro

	OPEN VendorsPorLoja

	FETCH NEXT FROM VendorsPorLoja
	INTO @IDLoja, @IDFornecedorParametro, @DataPedido

	WHILE @@FETCH_STATUS = 0
	BEGIN
	--
		EXEC PRC_ATENDEPEDIDOMINIMO @IDLoja, @IDFornecedorParametro, @DataPedido, 1

		FETCH NEXT FROM VendorsPorLoja
		INTO @IDLoja, @IDFornecedorParametro, @DataPedido
	--
	END		 

	CLOSE VendorsPorLoja
	DEALLOCATE VendorsPorLoja
--
END
