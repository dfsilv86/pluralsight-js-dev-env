
ALTER VIEW [dbo].[VW_REAB_SaldosPedidos]
AS

	WITH Lojas AS
	(
		SELECT IdLoja, 'S' tpRegiao 
		  FROM Loja WITH(NOLOCK)
		 WHERE blCalculaSugestao = 1	
		   AND dsEstado NOT IN ('PE','AL','CE','RN','PI','MA','PB','BA','SE')
		   AND DataConversao IS NOT NULL

		UNION ALL

		SELECT IdLoja, 'N' tpRegiao 
		  FROM Loja L WITH(NOLOCK)
		 WHERE blCalculaSugestao = 1	
		   AND L.dsEstado IN ('PE','AL','CE','RN','PI','MA','PB','BA','SE')
		   AND L.DataConversao IS NOT NULL
	), 
	InventariosPorDepartamento AS
	(
		SELECT L.IDLoja,
				I.IDDepartamento,
				MAX(I.dhInventario) as dhInventario
		FROM Inventario I WITH(NOLOCK)
		INNER JOIN Lojas L WITH(NOLOCK)
			ON L.IDLoja = I.IDLoja
		GROUP BY L.IDLoja, I.IDDepartamento

	), 
	ItensEntrada AS
	(		

		SELECT ID_ITEM_ENTRADA as IdItemDetalhe, IdLoja
		FROM WLMSLP_STAGE..SugestaoPedidoItemEntrada
		WHERE vlTipoReabastecimento = 3
	),
	SaldoRecebido AS
	(
		SELECT NFI.IdItemDetalhe, NF.IdLoja, SUM(NFI.qtItem) as qtSaldoRecebido, L.tpRegiao
		FROM NotaFiscalItem NFI WITH(NOLOCK)
		INNER JOIN NotaFiscal NF WITH(NOLOCK)
			ON NF.IDNotaFiscal = NFI.IDNotaFiscal
			AND NF.dtEmissao = GETDATE() -1 
			AND NF.dtRecebimento = GETDATE() -1
			AND NF.IDTipoNota = 8
		INNER JOIN Lojas L WITH(NOLOCK)
			ON L.IDLoja = NF.IDLoja
 		 GROUP BY NFI.IdItemDetalhe, NF.IdLoja, L.tpRegiao
	), 
	SaldoAntigo AS 
	(
		SELECT NFI.IdItemDetalhe, NF.IdLoja, SUM(NFI.qtItem) as qtSaldoAntigo
		FROM ItensEntrada IE WITH(NOLOCK) 
		INNER JOIN ItemDetalhe ID WITH(NOLOCK)
			ON ID.IdItemDetalhe = IE.IdItemDetalhe 
		INNER JOIN InventariosPorDepartamento IPD WITH(NOLOCK)
			ON ID.IdDepartamento = IPD.IdDepartamento 
		INNER JOIN NotaFiscalItem NFI WITH(NOLOCK)
			ON NFI.IdItemDetalhe = ID.IdItemDetalhe
		INNER JOIN NotaFiscal NF WITH(NOLOCK)
			ON NF.IdNotaFiscal = NFI.IdNotaFiscal 
			AND NF.IdLoja = IPD.IdLoja
			AND NF.dtRecebimento IS NULL
			AND NF.dtEmissao BETWEEN IPD.dhInventario AND GETDATE() -2
			AND NF.IDTipoNota = 8
	  GROUP BY NFI.IdItemDetalhe, NF.IdLoja
	)
	SELECT DISTINCT IE.IdItemDetalhe, IE.IdLoja, SA.qtSaldoRecebido , SAN.qtSaldoAntigo, SA.tpRegiao
	  FROM ItensEntrada IE WITH(NOLOCK) LEFT JOIN SaldoRecebido SA WITH(NOLOCK)
								  ON IE.IdItemDetalhe = SA.IdItemDetalhe 
								 AND IE.IdLoja = SA.IdLoja
						   LEFT JOIN SaldoAntigo SAN WITH(NOLOCK)
								  ON IE.IdItemDetalhe = SAN.IdItemDetalhe 
								 AND IE.IdLoja = SAN.IdLoja




GO


