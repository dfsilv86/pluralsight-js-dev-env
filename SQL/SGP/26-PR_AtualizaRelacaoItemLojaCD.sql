SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
=======================================================================================================================
Procedure..............: PR_AtualizaRelacaoItemLojaCD
Autor..................: 
Data de criação........: 
Objetivo...............: Atualizar tabelas RelacaoItemLojaCD e RelacaoItemCD com dados da STAGE
Parâmetros.............: 
Exemplo de uso.........: 
=======================================================================================================================
HISTÓRICO DE ALTERAÇÃO

Alterado por...........: Evandro Henrique Dapper
Data Alteração.........: 25/04/2016 (Projeto PESS)
Descrição da alteração.: Inclusão da atualização da tabela RelacaoItemCD com dados da STAGE
=======================================================================================================================
*/

ALTER PROCEDURE [dbo].[PR_AtualizaRelacaoItemLojaCD]
AS
BEGIN
	SELECT   idItem
			,idItemEntrada
			,idLoja
			,vlTipoReabastecimento
			,cdCrossRef
		INTO #Antigo
		FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		JOIN LojaCDParametro LCP WITH (NOLOCK)
		  ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
	   WHERE idItemEntrada IS NOT NULL

--print 'TRUNCATE TABLE RelacaoItemLojaCD'

	TRUNCATE TABLE RelacaoItemLojaCD

--print 'INSERT INTO RelacaoItemLojaCD'

	INSERT INTO RelacaoItemLojaCD (  idItem
									,idLojaCDParametro
									,cdSistema
									,blAtivo
									,cdUsuarioCriacao
									,cdUsuarioAtualizacao
									,dhCriacao
									,dhAtualizacao
									,idItemEntrada
									,vlTipoReabastecimento
									,cdCrossRef )
							SELECT   ID.IDItemDetalhe
									,LCP.IDLojaCDParametro
									,RILC.cdFormato
									,1
									,3
									,3
									,GETDATE()
									,GETDATE()
									,idItemEntrada
									,CASE 
										WHEN idItemEntrada IS NOT NULL
										 AND cdCrossRef IS NULL
										THEN dbo.fnObterTipoReabastecimento(idItemEntrada, LCP.IDCD, LCP.IDLoja)
										WHEN cdCrossRef IS NOT NULL
										THEN ANT.vlTipoReabastecimento
										WHEN id.tpVinculado IS NULL
										THEN dbo.fnObterTipoReabastecimento(id.IDItemDetalhe, LCP.IDCD, LCP.IDLoja)
										ELSE NULL
									 END
									,cdCrossRef
							  FROM WLMSLP_STAGE.dbo.RelacaoItemLojaCD RILC WITH (NOLOCK)
							  JOIN ItemDetalhe ID WITH (NOLOCK)
								ON ID.cdItem = RILC.cdItem
							   AND ID.blAtivo = 1
							   AND ID.tpStatus = 'A'
							   AND ID.cdSistema = RILC.cdFormato
							   AND (    ID.tpVinculado = 'S'
									 OR ID.tpManipulado = 'P'
									 OR ID.tpReceituario = 'I'
									 OR COALESCE(tpVinculado, tpManipulado, tpReceituario) IS NULL )
							  JOIN Loja L WITH (NOLOCK)
								ON L.cdLoja = RILC.cdLoja
							   AND L.cdSistema = RILC.cdFormato
							   AND L.blCalculaSugestao = 1
							  JOIN CD C WITH (NOLOCK)
								ON C.cdCD = RILC.cdCD
							   AND C.cdSistema = RILC.cdFormato
							  JOIN Trait T WITH (NOLOCK)
								ON T.IdLoja = L.IDLoja
							   AND T.IdItemDetalhe = ID.IDItemDetalhe
							   AND T.blAtivo = 1
							  JOIN LojaCDParametro LCP WITH (NOLOCK)
								ON LCP.IDLoja = L.IDLoja
							   AND LCP.IDCD = C.IDCD
							   AND LCP.blAtivo = 1
						 LEFT JOIN #Antigo ANT WITH (NOLOCK)
								ON ANT.IDItem = ID.IDItemDetalhe
							   AND ANT.IDLoja = LCP.IDLoja

--print 'DELETE FROM #Antigo'

	DELETE a
	  FROM #Antigo a
	 WHERE EXISTS ( SELECT 1
					  FROM RelacaoItemLojaCD r WITH (NOLOCK)
					  JOIN LojaCDParametro L WITH (NOLOCK)
						ON L.IDLojaCDParametro = r.IDLojaCDParametro
					 WHERE r.IDItem = a.IDItem
					   AND r.idItemEntrada = a.idItemEntrada
					   AND L.IDLoja = a.IDLoja )
					   
--print '-- Não é mais Item de Entrada'

	-- Não é mais Item de Entrada				
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
	     SELECT  RIL.IDRelacaoItemLojaCD
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Não é mais Item de Entrada' Observacao
		   FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		   JOIN ItemDetalhe ID WITH (NOLOCK)
			 ON RIL.IdItemEntrada = ID.IDItemDetalhe
			AND ID.tpVinculado <> 'E'
		   JOIN ItemDetalhe IDS WITH (NOLOCK)
			 ON IDS.IDItemDetalhe = RIL.IDItem
		   JOIN LojaCDParametro LCP WITH (NOLOCK)
			 ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		   JOIN Loja L WITH (NOLOCK)
			 ON L.IdLoja = LCP.IDLoja
		  WHERE RIL.cdCrossRef IS NULL

--print '-- Item de entrada não vinculados a trait'

	-- Item de entrada não vinculados a trait
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT   RIL.IDRelacaoItemLojaCD
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Item de Entrada não vinculado a Trait da Loja' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
	 LEFT JOIN Trait T WITH (NOLOCK)
			ON T.IdLoja = L.IdLoja
		   AND T.IdItemDetalhe = RIL.IdItemEntrada
		 WHERE T.IdTrait IS NULL

--print '-- Item de entrada não vinculados a trait'
	-- Item de entrada não vinculados a trait
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT   -1
				,IDE.cdItem cdItemEntrada
				,IDE.dsItem dsItemEntrada
				,ID.cdItem cdItemSaida
				,ID.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Item de Saida não vinculado a Trait da Loja' Observacao
		  FROM #Antigo RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IDItem = ID.IDItemDetalhe
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = RIL.IdLoja
		  JOIN ItemDetalhe IDE WITH (NOLOCK)
			ON IDE.IDItemDetalhe = RIL.idItemEntrada

--print '-- Item inativo ou deletado na origem (OIF), item desvinculado da relação de itens a serem sugeridos'
	-- Item inativo ou deletado na origem (OIF), item desvinculado da relação de itens a serem sugeridos
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT   -1
				,IDE.cdItem cdItemEntrada
				,IDE.dsItem dsItemEntrada
				,ID.cdItem cdItemSaida
				,ID.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Item inativo ou deletado na origem (OIF), item desvinculado da relação de itens a serem sugeridos' Observacao
		  FROM #Antigo ANT WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON ID.IDItemDetalhe = ANT.IDItem
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = ANT.IdLoja
		  JOIN ItemDetalhe IDE WITH (NOLOCK)
			ON IDE.IDItemDetalhe = ANT.idItemEntrada
		WHERE NOT EXISTS (SELECT 1 FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro 
		WHERE LCP.idLoja = ANT.idLoja
		AND RIL.idItem = ANT.idItem )
			
--print '-- Relacionamento entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos'
	-- Relacionamento entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT   RIL.IDRelacaoItemLojaCD
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Relac. entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
		  JOIN RelacionamentoItemSecundario RIS 
		    ON RIS.idItemDetalhe = RIL.idItemEntrada
		  JOIN RelacionamentoItemPrincipal RIP
		    ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
		 WHERE RIL.cdCrossRef IS NULL
		   AND RIL.IDItem <> RIP.IDItemDetalhe

--print '-- Relacionamento entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos'

	-- Relacionamento entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT DISTINCT RIL.IDRelacaoItemLojaCD
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Relacionamento entrada/saída do SGP desfeito, item desvinculado da relação de itens a serem sugeridos' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
		  JOIN RelacionamentoItemPrincipal RIP
		    ON RIP.IDItemDetalhe = RIL.IDItem
		  JOIN RelacionamentoItemSecundario RIS 
		    ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
	 LEFT JOIN ItemDetalhe IDE
	        ON IDE.IDItemDetalhe = RIS.IDItemDetalhe
	       AND dbo.fnObterTipoReabastecimento(RIS.iditemDetalhe, LCP.idCD, LCP.idLoja) in (20, 22, 40, 42, 43, 81)
		 WHERE RIL.cdCrossRef IS NOT NULL
		   AND IDE.IDItemDetalhe IS NULL
	
--print '-- Item não pertence a uma XRef válida, item desvinculado da relação de itens a serem sugeridos'
	
	-- Item não pertence a uma XRef válida, item desvinculado da relação de itens a serem sugeridos
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT  RIL.IDRelacaoItemLojaCD
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Item não pertence a uma XRef válida, item desvinculado da relação de itens a serem sugeridos' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
	 LEFT JOIN RelacionamentoItemPrime IP
	        ON IP.CdCrossRef = RIL.cdCrossRef
	     WHERE RIL.cdCrossRef IS NOT NULL
	       AND IP.IdRelacionamentoItemPrime IS NULL
		   
--print '-- XRef do OIF atualizada'
	-- XRef do OIF atualizada
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT  -1
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'XRef do OIF atualizada' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
	      JOIN RelacionamentoItemPrime IP
	        ON IP.idItemDetalhe = RIL.IDItemEntrada        
	     WHERE RIL.cdCrossRef IS NOT NULL
	       AND IP.CdCrossRef <> RIL.cdCrossRef

--print '-- UPDATE XRef do OIF atualizada'
		   
	-- XRef do OIF atualizada
	UPDATE RIL
	   SET RIL.CdCrossRef = IP.CdCrossRef
	  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
	  JOIN ItemDetalhe ID WITH (NOLOCK)
		ON RIL.IdItemEntrada = ID.IDItemDetalhe
	  JOIN ItemDetalhe IDS WITH (NOLOCK)
		ON IDS.IDItemDetalhe = RIL.IDItem
	  JOIN LojaCDParametro LCP WITH (NOLOCK)
		ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
	  JOIN Loja L WITH (NOLOCK)
		ON L.IdLoja = LCP.IdLoja
	  JOIN RelacionamentoItemPrime IP
		ON IP.idItemDetalhe = RIL.IDItemEntrada        
	 WHERE RIL.cdCrossRef IS NOT NULL
	   AND IP.CdCrossRef <> RIL.cdCrossRef
			
--print '-- Item primário da XRef OIF atualizado'
	-- Item primário da XRef OIF atualizado
	INSERT INTO  AtualizacaoRelacionamentoItem(IdReabastecimentoItemLoja
											   ,cdItemEntrada
											   ,dsItemEntrada
											   ,cdItemSaida
											   ,dsItemSaida
											   ,cdLoja
											   ,tpVinculadoEntrada
											   ,Observacao)
		SELECT  -1
				,ID.cdItem cdItemEntrada
				,ID.dsItem dsItemEntrada
				,IDS.cdItem cdItemSaida
				,IDS.dsItem dsItemSaida
				,L.cdLoja
				,ID.tpVinculado tpVinculadoEntrada
				,'Item primário da XRef OIF atualizado' Observacao
		  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
		  JOIN ItemDetalhe ID WITH (NOLOCK)
			ON RIL.IdItemEntrada = ID.IDItemDetalhe
		  JOIN ItemDetalhe IDS WITH (NOLOCK)
			ON IDS.IDItemDetalhe = RIL.IDItem
		  JOIN LojaCDParametro LCP WITH (NOLOCK)
			ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
		  JOIN Loja L WITH (NOLOCK)
			ON L.IdLoja = LCP.IdLoja
	      JOIN RelacionamentoItemPrime IP
	        ON IP.idItemDetalhe = RIL.IDItemEntrada
	     WHERE RIL.cdCrossRef IS NOT NULL
	       AND IP.Sequencial <> 1

--print '-- Item primário da XRef OIF atualizado'
	-- Item primário da XRef OIF atualizado
	UPDATE RIL
	   SET RIL.idItemEntrada = IP.idItemDetalhe
	  FROM RelacaoItemLojaCD RIL WITH (NOLOCK)
	  JOIN ItemDetalhe ID WITH (NOLOCK)
		ON RIL.IdItemEntrada = ID.IDItemDetalhe
	  JOIN ItemDetalhe IDS WITH (NOLOCK)
		ON IDS.IDItemDetalhe = RIL.IDItem
	  JOIN LojaCDParametro LCP WITH (NOLOCK)
		ON LCP.IDLojaCDParametro = RIL.IDLojaCDParametro
	  JOIN Loja L WITH (NOLOCK)
		ON L.IdLoja = LCP.IdLoja
	  JOIN RelacionamentoItemPrime IP
		ON IP.CdCrossRef = RIL.cdCrossRef
	   AND IP.Sequencial = 1
	 WHERE RIL.cdCrossRef IS NOT NULL
	   AND IP.Sequencial <> 1
		   
--print 'UPDATE  RelacaoItemLojaCD'

	UPDATE  RelacaoItemLojaCD
	   SET  idItemEntrada = NULL
		   ,vlTipoReabastecimento = NULL
		   ,cdCrossRef = NULL
	 WHERE IDRelacaoItemLojaCD IN ( SELECT DISTINCT IdReabastecimentoItemLoja
									  FROM AtualizacaoRelacionamentoItem )
						
--print 'TRUNCATE TABLE RelacaoItemCD'

	TRUNCATE TABLE RelacaoItemCD

--print 'INSERT INTO RelacaoItemCD'

	INSERT INTO RelacaoItemCD (  idItemEntrada
								,idItemSaida
								,idCD
								,vlTipoReabastecimento
								,vlEstoqueSeguranca )
						SELECT   id.IDItemDetalhe
								,ids.IDItemDetalhe
								,cd.IDCD
								,CASE 
									WHEN ISNULL(fcp.cdTipo, fp.cdTipo) = 'L'
										THEN COALESCE(rifc.TipoReabastecimento, rfc.TipoReabastecimento, id.vlTipoReabastecimento)
									ELSE id.vlTipoReabastecimento
									END
								,rifc.EstoqueSeguranca
						  FROM WLMSLP_STAGE..RelacaoItemLojaCD r
						  JOIN ItemDetalhe id
							ON id.cdItem = r.cdItem
					 LEFT JOIN RelacionamentoItemSecundario rs
							ON rs.IDItemDetalhe = id.IDItemDetalhe
					 LEFT JOIN RelacionamentoItemPrincipal rp
							ON rp.IDRelacionamentoItemPrincipal = rs.IDRelacionamentoItemPrincipal
						   AND rp.IDTipoRelacionamento = 1
					LEFT JOIN ItemDetalhe IDS 
							ON IDS.IDItemDetalhe = RP.IDItemDetalhe
							AND IDS.tpStatus = 'A'
							AND IDS.blAtivo = 1
							AND IDS.blItemTransferencia = 0
					JOIN CD cd
							ON cd.cdCD = r.cdCD
							AND cd.blConvertido = 1
					JOIN FornecedorParametro fp
							ON fp.IDFornecedorParametro = id.idFornecedorParametro
					 LEFT JOIN FornecedorCDParametro fcp
							ON fcp.IDFornecedorParametro = fp.IDFornecedorParametro
						   AND fcp.IDCD = cd.IDCD
					 LEFT JOIN ReabastecimentoFornecedorCD rfc
							ON rfc.idFornecedorParametro = fp.IDFornecedorParametro
						   AND rfc.idCD = cd.IDCD
					 LEFT JOIN ReabastecimentoItemFornecedorCD rifc
							ON rifc.idFornecedorParametro = fp.IDFornecedorParametro
						   AND rifc.idCD = cd.IDCD
						   AND rifc.idItemDetalhe = id.IDItemDetalhe
						 WHERE (( ISNULL(fcp.cdTipo, fp.cdTipo) = 'L'
								 AND COALESCE(rifc.TipoReabastecimento, rfc.TipoReabastecimento, id.vlTipoReabastecimento) IN (20, 22, 40, 42, 43)
								)
								OR (id.vlTipoReabastecimento IN (20, 22, 40, 42, 43)))
							AND ((id.tpVinculado = 'E' AND ids.IDItemDetalhe IS NOT NULL) OR id.tpVinculado IS NULL)
							AND (id.tpManipulado = 'P' OR id.tpManipulado IS NULL)
							AND (id.tpReceituario = 'I' OR id.tpReceituario IS NULL)	
							AND id.tpStatus = 'A'
							AND id.blAtivo = 1
							AND id.blItemTransferencia = 0
					  GROUP BY   cd.IDCD
							    ,id.IDItemDetalhe
								,ids.IDItemDetalhe
								,fcp.cdTipo
								,fp.cdTipo
								,rifc.TipoReabastecimento
								,rfc.TipoReabastecimento
								,id.vlTipoReabastecimento
								,rifc.EstoqueSeguranca

END
