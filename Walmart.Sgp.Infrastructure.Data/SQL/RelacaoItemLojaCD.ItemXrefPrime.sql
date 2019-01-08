/*
DECLARE @cdItem BIGINT, @cdLoja BIGINT, @cdCD BIGINT, @cdSistema BIGINT;
SET @cdItem = 9400427;
SET @cdLoja = 1006;
SET @cdCD = 7471;
SET @cdSistema = 1;
--*/

SELECT TOP 1 * 
  FROM (
		SELECT ITE.IDItemDetalhe
			 , ITE.cdItem
			 , ITE.dsItem
			 , ITE.tpStatus
			 , ITE.idFornecedorParametro
			 , FPE.cdStatusVendor
			 , IP.Sequencial
			 , IP.CdCrossRef
			 , (
				SELECT TOP 1 cdItem FROM (
					   SELECT I.cdItem
						 FROM RelacionamentoItemSecundario R WITH (NOLOCK)
							   LEFT JOIN RelacionamentoItemPrime P WITH (NOLOCK)
									  ON P.idItemDetalhe = R.IDItemDetalhe
									 AND P.cdCrossRef = IP.cdCrossRef
									 AND P.Sequencial = 1
							  INNER JOIN ItemDetalhe I WITH (NOLOCK)
									  ON I.IDItemDetalhe = P.IDItemDetalhe
									 AND dbo.fnObterTipoReabastecimento(I.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)
									 AND I.tpStatus = 'A'
						WHERE R.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
					   UNION ALL
					   SELECT I.cdItem
						 FROM RelacionamentoItemPrincipal R WITH (NOLOCK)
							   LEFT JOIN RelacionamentoItemPrime P WITH (NOLOCK)
									  ON P.idItemDetalhe = R.IDItemDetalhe
									 AND P.cdCrossRef = IP.cdCrossRef
									 AND P.Sequencial = 1
							  INNER JOIN ItemDetalhe I WITH (NOLOCK)
									  ON I.IDItemDetalhe = P.IDItemDetalhe
									 AND dbo.fnObterTipoReabastecimento(I.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)
									 AND I.tpStatus = 'A'
						WHERE R.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
					) Itens
			   ) AS cdItemPrime
		  FROM ItemDetalhe ITE WITH (NOLOCK)
			   INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
					   ON RIS.IDItemDetalhe = ITE.IDItemDetalhe
				LEFT JOIN FornecedorParametro FPE WITH (NOLOCK)
					   ON FPE.IDFornecedorParametro = ITE.idFornecedorParametro
					  AND FPE.blAtivo = 1
			   INNER JOIN RelacionamentoItemPrime IP WITH (NOLOCK)
					   ON IP.idItemDetalhe = ITE.IDItemDetalhe
					  AND IP.Sequencial > 1
			   INNER JOIN Loja L WITH (NOLOCK)
					   ON l.cdLoja = @cdloja 
					  AND l.cdSistema = @cdSistema
			   INNER JOIN CD WITH (NOLOCK)
					   ON CD.cdCD = @cdCD 
					  AND CD.cdSistema = @cdSistema
		 WHERE ITE.cdItem = @cdItem 
		   AND ITE.cdSistema = @cdSistema
		   AND ITE.tpVinculado = 'E'
		   AND dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)
		   AND CD.blConvertido = 1
		-- trecho removido pq agora existe outra spec para o caso de tpVinculado=S
		/*UNION ALL
		SELECT ITE.IDItemDetalhe
			 , ITE.cdItem
			 , ITE.dsItem
			 , ITE.tpStatus
			 , ITE.idFornecedorParametro
			 , FPE.cdStatusVendor
			 , IP.Sequencial
			 , IP.CdCrossRef
			 , (
				SELECT TOP 1 cdItem FROM (
					   SELECT I.cdItem
						 FROM RelacionamentoItemSecundario R WITH (NOLOCK)
							   LEFT JOIN RelacionamentoItemPrime P WITH (NOLOCK)
									  ON P.idItemDetalhe = R.IDItemDetalhe
									 AND P.cdCrossRef = IP.cdCrossRef
									 AND P.Sequencial = 1
							  INNER JOIN ItemDetalhe I WITH (NOLOCK)
									  ON I.IDItemDetalhe = P.IDItemDetalhe
									 AND dbo.fnObterTipoReabastecimento(I.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)
									 AND I.tpStatus = 'A'
						WHERE R.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
					   UNION ALL
					   SELECT I.cdItem
						 FROM RelacionamentoItemPrincipal R WITH (NOLOCK)
							   LEFT JOIN RelacionamentoItemPrime P WITH (NOLOCK)
									  ON P.idItemDetalhe = R.IDItemDetalhe
									 AND P.cdCrossRef = IP.cdCrossRef
									 AND P.Sequencial = 1
							  INNER JOIN ItemDetalhe I WITH (NOLOCK)
									  ON I.IDItemDetalhe = P.IDItemDetalhe
									 AND dbo.fnObterTipoReabastecimento(I.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)
									 AND I.tpStatus = 'A'
						WHERE R.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
					) Itens
			   ) AS cdItemPrime
		  FROM ItemDetalhe ITE WITH (NOLOCK)
			   INNER JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
					   ON RIP.IDItemDetalhe = ITE.IDItemDetalhe
				LEFT JOIN FornecedorParametro FPE WITH (NOLOCK)
					   ON FPE.IDFornecedorParametro = ITE.idFornecedorParametro
					  AND FPE.blAtivo = 1
			   INNER JOIN RelacionamentoItemPrime IP WITH (NOLOCK)
					   ON IP.idItemDetalhe = ITE.IDItemDetalhe
					  AND IP.Sequencial > 1  -- se for prime, é validado em outra spec
			   INNER JOIN Loja L WITH (NOLOCK)
					   ON l.cdLoja = @cdloja 
					  AND l.cdSistema = @cdSistema
			   INNER JOIN CD WITH (NOLOCK)
					   ON CD.cdCD = @cdCD 
					  AND CD.cdSistema = @cdSistema
					  AND CD.blConvertido = 1   -- se nao for convertido, retorna 0 registros, ja que vai ser validado em outra spec
		 WHERE ITE.cdItem = @cdItem 
		   AND ITE.cdSistema = @cdSistema
		   AND ITE.tpVinculado = 'S'
		   AND dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, cd.idcd, l.idloja) IN (20, 22, 40, 42, 43, 81)  -- se é S mas nao é Staple, retorna 0 registros, ja que vai ser validado em otura spec*/
       ) X 
 ORDER BY cdItemPrime DESC