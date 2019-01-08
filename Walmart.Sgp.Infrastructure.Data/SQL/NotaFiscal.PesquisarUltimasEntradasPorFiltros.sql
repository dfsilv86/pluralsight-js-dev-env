/*
DECLARE @idItemDetalhe INT, @idLoja INT, @dtSolicitacao DATETIME
SET @idItemDetalhe = 500072016;
SET @idLoja = 1618;
SET @dtSolicitacao = GETDATE()
--*/

IF (NOT EXISTS (SELECT 1 FROM RelacionamentoItemPrincipal (NOLOCK) WHERE IDItemDetalhe = @idItemDetalhe)) AND
   (NOT EXISTS (SELECT 1 FROM RelacionamentoItemSecundario (NOLOCK) WHERE IDItemDetalhe = @idItemDetalhe))
	BEGIN
		SELECT TOP 3
			 NF.nrNotaFiscal
			,NFI.vlCusto
			,NF.dtRecebimento
			,NF.dtEmissao
			,ID.cdItem
			,ID.dsItem
		 FROM NotaFiscal NF (NOLOCK)
		 JOIN NotaFiscalItem NFI (NOLOCK) ON NF.IDNotaFiscal = NFI.IDNotaFiscal 
		 JOIN ItemDetalhe ID (NOLOCK) ON NFI.IDItemDetalhe = ID.IDItemDetalhe		
		WHERE NFI.IdNotaFiscalItemStatus <> 3
		  AND ID.IDItemDetalhe = @idItemDetalhe
		  AND NF.IDLoja = @idLoja
		  AND NF.dtRecebimento <= @dtSolicitacao
		ORDER BY NF.dtRecebimento DESC
	END
ELSE IF EXISTS (SELECT 1 FROM ItemDetalhe WHERE IDItemDetalhe = @idItemDetalhe AND tpVinculado = 'S' AND blAtivo = 1)
	BEGIN
		SELECT TOP 3
			 NF.nrNotaFiscal
			,NFI.vlCusto
			,NF.dtRecebimento
			,NF.dtEmissao
			,IE.cdItem
			,IE.dsItem
		FROM NotaFiscal NF (nolock)
		JOIN NotaFiscalItem NFI (nolock) on NF.IDNotaFiscal = NFI.IDNotaFiscal 
		JOIN
		(
			SELECT 
				 id.IDItemDetalhe
				,id.cdItem
				,id.dsItem 
			 FROM RelacionamentoItemPrincipal rip (NOLOCK)
			 JOIN RelacionamentoItemSecundario ris (NOLOCK) ON rip.IDRelacionamentoItemPrincipal = ris.IDRelacionamentoItemPrincipal
			 JOIN ItemDetalhe id (NOLOCK) ON id.IDItemDetalhe = ris.IDItemDetalhe
			WHERE rip.IDItemDetalhe = @idItemDetalhe
			  AND id.blAtivo = 1
		) IE ON NFI.IDItemDetalhe = IE.IDItemDetalhe
		WHERE NF.IDLoja = @idLoja
		  AND NF.dtRecebimento <= @dtSolicitacao
		  AND NFI.IdNotaFiscalItemStatus <> 3
		ORDER BY NF.dtRecebimento DESC
	END
ELSE
	BEGIN
		SELECT TOP 3
			 NF.nrNotaFiscal
			,NFI.vlCusto
			,NF.dtRecebimento
			,NF.dtEmissao
			,IE.cdItem
			,IE.dsItem
		FROM NotaFiscal NF (nolock)
		JOIN NotaFiscalItem NFI (nolock) on NF.IDNotaFiscal = NFI.IDNotaFiscal 
		JOIN
		(
			SELECT 
				 id.IDItemDetalhe
				,id.cdItem
				,id.dsItem 
			FROM RelacionamentoItemSecundario ris (NOLOCK)
			join RelacionamentoItemPrincipal rip (NOLOCK) ON ris.IDRelacionamentoItemPrincipal = rip.IDRelacionamentoItemPrincipal
			join ItemDetalhe id (NOLOCK) on id.IDItemDetalhe = rip.IDItemDetalhe
			where ris.IDItemDetalhe = @idItemDetalhe
			group by id.IDItemDetalhe, id.cdItem, id.dsItem 
		) IE ON NFI.IDItemDetalhe = IE.IDItemDetalhe		
		WHERE NF.dtRecebimento <= @dtSolicitacao
		AND NF.IDLoja = @idLoja
		AND NFI.IdNotaFiscalItemStatus <> 3
		ORDER BY NF.dtRecebimento DESC
	END