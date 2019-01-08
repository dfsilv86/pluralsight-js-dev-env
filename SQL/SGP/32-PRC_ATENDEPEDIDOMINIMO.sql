SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
=======================================================================================================================
Procedure..............: PR_AtendePedidoMinimo
Autor..................: Evandro Henrique Dapper (CWI)
Data de criação........: 28/04/2016 (Projeto PESS)
Objetivo...............: Essa rotina tem como objetivo consolidar os pedidos dos itens tipos 7, 37 e 33 das 
                         sugestões calculadas pelo SGP (SugestaoPedido.cdOrigemCalculo = “S”)
Parâmetros.............: idLoja = id da loja
						 idFornecedorParametro = id do fornecedor parâmetro
						 DataPedido = data do pedido
						 blAdicionaPack = Se não adiciona pack coloca o pedido mínimo como não atendido
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: 
Data Alteração.........: 
Descrição da alteração.: 
=======================================================================================================================
*/

ALTER PROCEDURE [dbo].[PRC_ATENDEPEDIDOMINIMO] @idLoja INT
	,@idFornecedorParametro BIGINT
	,@DataPedido DATE
	,@blAdicionaPack BIT = 1
AS
BEGIN
	DECLARE @vlPedidoMinimo DECIMAL(15, 2)
		,@vlPedido DECIMAL(15, 2)
		,@tpPedidoMinimo CHAR(1)
		,@blAtendePedidoMinimo BIT
		,@maxShelfLifeAtendido BIT
		,@idSugestaoPedido BIGINT
		,@vlShelfLife NUMERIC(5)
		,@vlEstoque DECIMAL(15, 3)
		,@vlForecastMedio DECIMAL(15, 3)
		,@vlFatorConversao FLOAT
		,@qtVendorPackage INT
		,@vlModulo DECIMAL(15, 3)
		,@qtPackCompra INT
		,@vlCustoItem DECIMAL(15, 3)
		,@qtDiasEstoque INT
		,@tpCaixaFornecedor CHAR(1)
		,@vlPesoLiquido DECIMAL(15, 4)
		,@cdConvertido BIT

	IF @blAdicionaPack = 0
		UPDATE SugestaoPedido
		SET blAtendePedidoMinimo = 0
		WHERE dtPedido = @DataPedido
			AND IdLoja = @idLoja
			AND IDFornecedorParametro = @idFornecedorParametro
			AND vlTipoReabastecimento IN (7, 33, 37)
			AND qtdPackCompra > 0
			AND vlForecastMedio > 0
			AND cdOrigemCalculo = 'S'
	ELSE
	BEGIN
		SELECT @vlPedidoMinimo = FP.vlValorMinimo
			,@tpPedidoMinimo = FP.tpPedidoMinimo
		FROM FornecedorParametro FP WITH (NOLOCK)
		WHERE FP.IDFornecedorParametro = @idFornecedorParametro

		IF @tpPedidoMinimo = 'C'
			SELECT @vlPedido = SUM(SP.qtdPackCompra * SP.vlFatorConversao * SP.qtVendorPackage)
			FROM SugestaoPedido SP WITH (NOLOCK)
			WHERE SP.dtPedido = @DataPedido
				AND SP.IdLoja = @idLoja
				AND SP.IDFornecedorParametro = @idFornecedorParametro
				AND SP.vlTipoReabastecimento IN (7, 33, 37)
				AND SP.qtdPackCompra > 0
				AND SP.vlForecastMedio > 0
				AND SP.cdOrigemCalculo = 'S'
		ELSE
			SELECT @vlPedido = SUM(SP.qtdPackCompra * SP.vlFatorConversao * SP.qtVendorPackage * CASE ISNULL(ES.vlCustoContabilAtual, 0)
						WHEN 0
							THEN ISNULL(ES.vlCustoCadastroAtual, 0)
						ELSE ISNULL(ES.vlCustoContabilAtual, 0)
						END)
			FROM SugestaoPedido SP WITH (NOLOCK)
			LEFT JOIN Estoque ES WITH (NOLOCK)
				ON ES.IDLoja = SP.IdLoja
					AND ES.IDItemDetalhe = SP.IDItemDetalheSugestao
					AND ES.dtRecebimento = (
						SELECT MAX(E.dtRecebimento)
						FROM Estoque E WITH (NOLOCK)
						WHERE E.IDItemDetalhe = ES.IDItemDetalhe
							AND E.IDLoja = ES.IDLoja
						)
			WHERE SP.dtPedido = @DataPedido
				AND SP.IdLoja = @idLoja
				AND SP.IDFornecedorParametro = @idFornecedorParametro
				AND SP.vlTipoReabastecimento IN (7, 33, 37)
				AND SP.qtdPackCompra > 0
				AND SP.vlForecastMedio > 0
				AND SP.cdOrigemCalculo = 'S'

		--print '01'
		--print '    @vlPedido= ' + convert(VARCHAR, @vlPedido)
		--print '    @vlPedidoMinimo= ' + convert(VARCHAR, @vlPedidoMinimo)
		--print '  '

		IF @vlPedido >= @vlPedidoMinimo
			UPDATE SP
			SET SP.blAtendePedidoMinimo = 1
			FROM SugestaoPedido SP
			JOIN CD
				ON CD.idCD = SP.idCD
			WHERE SP.dtPedido = @DataPedido
				AND SP.IdLoja = @idLoja
				AND SP.IDFornecedorParametro = @idFornecedorParametro
				AND (
					SP.vlTipoReabastecimento IN (7, 33, 37)
					OR (
						CD.blConvertido = 1
						AND SP.vlTipoReabastecimento = 97
						)
					)
				AND SP.qtdPackCompra > 0
				AND SP.vlForecastMedio > 0
				AND SP.cdOrigemCalculo = 'S'
		ELSE
		BEGIN
			-- Habilita para a iteração todos os itens passivos de adição no pack. (itens em aberto)
			UPDATE SP
			SET SP.blAtendePedidoMinimo = 1
			FROM SugestaoPedido SP WITH (NOLOCK)
			JOIN CD WITH (NOLOCK)
				ON CD.idCD = SP.idCD
			JOIN Loja L WITH (NOLOCK)
				ON L.IDLoja = SP.IdLoja
			JOIN ItemDetalhe ID WITH (NOLOCK)
				ON ID.IDItemDetalhe = SP.IDItemDetalhePedido
			CROSS APPLY dbo.fnBuscaGrade(L.IDBandeira, ID.IDDepartamento, SP.IDLoja) GS
			WHERE SP.dtPedido = @DataPedido
				AND SP.IdLoja = @idLoja
				AND SP.IDFornecedorParametro = @idFornecedorParametro
				AND (
					SP.vlTipoReabastecimento IN (7, 33, 37)
					OR (
						CD.blConvertido = 1
						AND SP.vlTipoReabastecimento = 97
						)
					)
				AND SP.qtdPackCompra > 0
				AND SP.vlForecastMedio > 0
				AND SP.cdOrigemCalculo = 'S'
				AND SP.blAtendePedidoMinimo = 0

			DECLARE Pedidos CURSOR STATIC FORWARD_ONLY READ_ONLY
			FOR
			SELECT SP.IDSugestaoPedido
				,SP.vlshelflife
				,SP.vlestoque
				,SP.vlforecastmedio
				,SP.vlfatorconversao
				,SP.qtvendorpackage
				,SP.vlmodulo
				,SP.qtdpackcompra
				,CASE ISNULL(ES.vlCustoContabilAtual, 0)
					WHEN 0
						THEN ISNULL(ES.vlCustoCadastroAtual, 0)
					ELSE ISNULL(ES.vlCustoContabilAtual, 0)
					END vlCustoItem
				,CD.blConvertido
				,SP.vlPesoLiquido
				,SP.tpCaixaFornecedor
			FROM SugestaoPedido SP WITH (NOLOCK)
			LEFT JOIN Estoque ES WITH (NOLOCK)
				ON ES.IDLoja = SP.IdLoja
					AND ES.IDItemDetalhe = SP.IDItemDetalheSugestao
					AND ES.dtRecebimento = (
						SELECT MAX(E.dtRecebimento)
						FROM Estoque E WITH (NOLOCK)
						WHERE E.IDItemDetalhe = ES.IDItemDetalhe
							AND E.IDLoja = ES.IDLoja
						)
			JOIN CD
				ON CD.idCD = SP.idCD
			WHERE SP.dtPedido = @DataPedido
				AND SP.IdLoja = @idLoja
				AND SP.IDFornecedorParametro = @idFornecedorParametro
				AND (
					SP.vlTipoReabastecimento IN (7, 33, 37)
					OR (
						CD.blConvertido = 1
						AND SP.vlTipoReabastecimento = 97
						)
					)
				AND SP.qtdPackCompra > 0
				AND SP.vlForecastMedio > 0
				AND SP.cdOrigemCalculo = 'S'
				AND SP.blAtendePedidoMinimo = 1

			SET @blAtendePedidoMinimo = 0
			SET @maxShelfLifeAtendido = 0

			WHILE @blAtendePedidoMinimo = 0
				AND @maxShelfLifeAtendido = 0
			BEGIN
				--print '02'
				--print '    @idLoja = ' + convert(VARCHAR, @idLoja)
				--print '    @idFornecedorParametro = ' + convert(VARCHAR, @idFornecedorParametro)
				--print '    Cursor'
				--print '  '

				OPEN Pedidos

				FETCH Pedidos
				INTO @idSugestaoPedido
					,@vlShelfLife
					,@vlEstoque
					,@vlForecastMedio
					,@vlFatorConversao
					,@qtVendorPackage
					,@vlModulo
					,@qtPackCompra
					,@vlCustoItem
					,@cdConvertido
					,@vlPesoLiquido
					,@tpCaixaFornecedor

				IF @@FETCH_STATUS != 0
					SET @maxShelfLifeAtendido = 1
				ELSE
					WHILE @@FETCH_STATUS = 0
						AND @blAtendePedidoMinimo = 0
					BEGIN
						--print '03'
						--print '    @idSugestaoPedido = ' + convert(VARCHAR, @idSugestaoPedido)
						--print '    @qtVendorPackage=' + convert(VARCHAR, @qtVendorPackage)
						--print '    @qtPackCompra=' + convert(VARCHAR, @qtPackCompra)
						--print '    @vlPedido=' + convert(VARCHAR, @vlPedido)
						--print '    @cdConvertido=' + convert(VARCHAR, @cdConvertido)
						--print '    @tpCaixaFornecedor=' + @tpCaixaFornecedor
						--print '  '

						IF @cdConvertido = 0
						BEGIN
							IF @qtVendorPackage = 1
							BEGIN
								SET @qtPackCompra += @vlModulo
								SET @qtDiasEstoque = CEILING((@vlEstoque + (@qtPackCompra * @vlFatorConversao)) / @vlForecastMedio)

								IF @qtDiasEstoque < @vlShelfLife
									SET @vlPedido = @vlPedido + (
											@vlModulo * CASE @tpPedidoMinimo
												WHEN 'C'
													THEN 1
												ELSE @vlCustoItem
												END
											)
							END
							ELSE
							BEGIN
								SET @qtPackCompra += 1
								SET @qtDiasEstoque = CEILING(@vlEstoque + (@qtPackCompra * @qtVendorPackage * @vlFatorConversao)) / @vlForecastMedio

								IF @qtDiasEstoque < @vlShelfLife
									SET @vlPedido = @vlPedido + CASE @tpPedidoMinimo
											WHEN 'C'
												THEN 1
											ELSE @vlCustoItem
											END
							END
						END
						ELSE
						BEGIN
							IF @tpCaixaFornecedor = 'V'
							BEGIN
								SET @qtPackCompra += @vlPesoLiquido
								SET @qtDiasEstoque = CEILING((@vlEstoque + (@qtPackCompra * @vlFatorConversao)) / @vlForecastMedio)

								IF @qtDiasEstoque < @vlShelfLife
									SET @vlPedido = @vlPedido + (
											@vlPesoLiquido * CASE @tpPedidoMinimo
												WHEN 'C'
													THEN 1
												ELSE @vlCustoItem
												END
											)
							END
							ELSE
							BEGIN
								SET @qtPackCompra += 1
								SET @qtDiasEstoque = CEILING(@vlEstoque + (@qtPackCompra * @qtVendorPackage * @vlFatorConversao)) / @vlForecastMedio

								IF @qtDiasEstoque < @vlShelfLife
									SET @vlPedido = @vlPedido + CASE @tpPedidoMinimo
											WHEN 'C'
												THEN 1
											ELSE @vlCustoItem
											END
							END
						END

						--print '04'
						--print '    @qtPackCompra=' + convert(VARCHAR, @qtPackCompra)
						--print '    @qtDiasEstoque=' + convert(VARCHAR, @qtDiasEstoque)
						--print '    @vlShelfLife=' + convert(VARCHAR, @vlShelfLife)
						--print '    @vlPedido=' + convert(VARCHAR, @vlPedido)
						--print '    @vlPedidoMinimo=' + convert(VARCHAR, @vlPedidoMinimo)
						--print '  '

						IF @vlPedido >= @vlPedidoMinimo
							SET @blAtendePedidoMinimo = 1

						IF @qtDiasEstoque < @vlShelfLife
						BEGIN
							--print '05'
							--print '    @qtDiasEstoque < @vlShelfLife'
							--print '  '

							UPDATE SugestaoPedido
							SET vlQtdDiasEstoque = @qtDiasEstoque
								,qtdPackCompra = @qtPackCompra
							WHERE IDSugestaoPedido = @idSugestaoPedido
						END

						FETCH Pedidos
						INTO @idSugestaoPedido
							,@vlShelfLife
							,@vlEstoque
							,@vlForecastMedio
							,@vlFatorConversao
							,@qtVendorPackage
							,@vlModulo
							,@qtPackCompra
							,@vlCustoItem
							,@cdConvertido
							,@vlPesoLiquido
							,@tpCaixaFornecedor
					END

				CLOSE Pedidos

				--print '06'
				--print '@blAtendePedidoMinimo=' + convert(VARCHAR, @blAtendePedidoMinimo)
				--print '@maxShelfLifeAtendido=' + convert(VARCHAR, @maxShelfLifeAtendido)

				UPDATE SugestaoPedido
				SET blAtendePedidoMinimo = @blAtendePedidoMinimo
				WHERE dtPedido = @DataPedido
					AND IdLoja = @idLoja
					AND IDFornecedorParametro = @idFornecedorParametro
					AND vlTipoReabastecimento IN (7, 33, 37)
					AND qtdPackCompra > 0
					AND vlForecastMedio > 0
					AND cdOrigemCalculo = 'S'
			END

			DEALLOCATE Pedidos
		END
	END
END
