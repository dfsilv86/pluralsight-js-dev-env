

ALTER PROCEDURE [dbo].[PR_AtualizaRelacaoItemLojaCD]
AS
BEGIN

	SELECT IDItem, idItemEntrada, IDLoja, vlTipoReabastecimento, cdCrossRef
	INTO #Antigo
	FROM RelacaoItemLojaCD RIL
	INNER JOIN LojaCDParametro LCP ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
	WHERE idItemEntrada IS NOT NULL
	
	TRUNCATE TABLE RelacaoItemLojaCD

	INSERT INTO RelacaoItemLojaCD(IDItem, IDLojaCDParametro, cdSistema, blAtivo, cdUsuarioCriacao, cdUsuarioAtualizacao, dhCriacao, dhAtualizacao, idItemEntrada, vlTipoReabastecimento, cdCrossRef)
	SELECT ID.IDItemDetalhe, LCP.IDLojaCDParametro, RILC.cdFormato, 1, 3, 3, GETDATE(), GETDATE(), idItemEntrada, CASE WHEN idItemEntrada IS NOT NULL AND cdCrossRef IS NULL THEN dbo.fnObterTipoReabastecimento(idItemEntrada, LCP.IDCD, LCP.IDLoja)
																														WHEN cdCrossRef IS NOT NULL THEN ANT.vlTipoReabastecimento
																														WHEN id.tpVinculado IS NULL THEN dbo.fnObterTipoReabastecimento(id.IDItemDetalhe, LCP.IDCD, LCP.IDLoja)
																														ELSE NULL END, cdCrossRef
	FROM WLMSLP_STAGE.dbo.RelacaoItemLojaCD RILC
	INNER JOIN ItemDetalhe ID 
		ON ID.cdItem = RILC.cdItem 
		AND ID.cdSistema = RILC.cdFormato
		AND (ID.tpVinculado = 'S' OR ID.tpManipulado = 'P' OR ID.tpReceituario = 'I')
	INNER JOIN Loja L
		ON L.cdLoja = RILC.cdLoja
		AND L.cdSistema = RILC.cdFormato
		AND L.blCalculaSugestao = 1
	INNER JOIN CD C
		ON C.cdCD = RILC.cdCD
		AND C.cdSistema = RILC.cdFormato
	INNER JOIN Trait T
		ON T.IdLoja = L.IDLoja
		AND T.IdItemDetalhe = ID.IDItemDetalhe
		AND T.blAtivo = 1
	INNER JOIN LojaCDParametro LCP
		ON LCP.IDLoja = L.IDLoja
		AND LCP.IDCD = C.IDCD
		AND LCP.blAtivo = 1
	LEFT JOIN #Antigo ANT
		ON ANT.IDItem = ID.IDItemDetalhe
		AND ANT.IDLoja = LCP.IDLoja

	DELETE a FROM #Antigo a
	WHERE EXISTS (SELECT 1 
					FROM RelacaoItemLojaCD r
					INNER JOIN LojaCDParametro L ON L.IDLojaCDParametro = r.IDLojaCDParametro
					WHERE r.IDItem = a.IDItem
					AND r.idItemEntrada = a.idItemEntrada
					AND L.IDLoja = a.IDLoja)

	INSERT INTO AtualizacaoRelacionamentoItem
	SELECT RIL.IDRelacaoItemLojaCD,
		   ID.cdItem cdItemEntrada,
		   ID.dsItem dsItemEntrada,
		   IDS.cdItem cdItemSaida,
		   IDS.dsItem dsItemSaida,
		   L.cdLoja,
		   ID.tpVinculado tpVinculadoEntrada,
		   'Não é mais Item de Entrada' Observacao	
		FROM RelacaoItemLojaCD RIL WITH (NOLOCK) 
		INNER JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe 
			AND ID.tpVinculado <> 'E'
		INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		INNER JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IDLoja
		WHERE RIL.cdCrossRef IS NULL

		-- Item de entrada não vinculados a trait
		INSERT INTO AtualizacaoRelacionamentoItem
		SELECT RIL.IDRelacaoItemLojaCD,
		   ID.cdItem cdItemEntrada,
		   ID.dsItem dsItemEntrada,
		   IDS.cdItem cdItemSaida,
		   IDS.dsItem dsItemSaida,
		   L.cdLoja,
		   ID.tpVinculado tpVinculadoEntrada,
		  'Item de Entrada não vinculado a Trait da Loja' AS Observacao			
		FROM RelacaoItemLojaCD RIL WITH (NOLOCK) 
		INNER JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		INNER JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja 															
		LEFT JOIN Trait T WITH (NOLOCK)
			ON T.IdLoja = L.IdLoja
			AND T.IdItemDetalhe = RIL.IdItemEntrada
		WHERE T.IdTrait IS NULL

		-- Item de entrada não vinculados a trait
		INSERT INTO AtualizacaoRelacionamentoItem
		SELECT -1,
		   IDE.cdItem cdItemEntrada,
		   IDE.dsItem dsItemEntrada,
		   ID.cdItem cdItemSaida,
		   ID.dsItem dsItemSaida,
		   L.cdLoja,
		   ID.tpVinculado tpVinculadoEntrada,
		  'Item de Saida não vinculado a Trait da Loja' AS Observacao			
		FROM #Antigo RIL WITH (NOLOCK) 
		INNER JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IDItem = ID.IDItemDetalhe 
		INNER JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = RIL.IdLoja
		INNER JOIN ItemDetalhe IDE WITH(NOLOCK)
			ON IDE.IDItemDetalhe= RIL.idItemEntrada
			
		UPDATE RelacaoItemLojaCD 
		   SET idItemEntrada = NULL, 
				vlTipoReabastecimento = NULL,
				cdCrossRef = null
	     WHERE IDRelacaoItemLojaCD IN (SELECT DISTINCT IdReabastecimentoItemLoja FROM AtualizacaoRelacionamentoItem)														
		
			
END