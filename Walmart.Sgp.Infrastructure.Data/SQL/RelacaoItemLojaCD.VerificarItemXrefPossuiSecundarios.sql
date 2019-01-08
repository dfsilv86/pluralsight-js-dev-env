/*
DECLARE @cdItem BIGINT, @cdLoja BIGINT, @cdCD BIGINT, @cdSistema BIGINT;
SET @cdItem = 500686027;
SET @cdLoja = 1006;
SET @cdCD = 7471;
SET @cdSistema = 1;
--*/
SELECT COUNT(1) FROM (
	-- A consulta retorna informações nos casos onde não é xref ou staple porque não há tomada de decisão fora da spec se deve avaliar a spec ou não.
	SELECT ITE.IDItemDetalhe  -- caso onde é item de entrada, prime, staple (cd convertido ou nao), e tem outro tpVinculado=E como secundario
	  FROM ItemDetalhe ITE WITH(NOLOCK)
		   INNER JOIN Loja L WITH(NOLOCK) 
		           ON l.cdLoja = @cdloja
				  AND l.cdSistema = @cdSistema
		   INNER JOIN CD WITH(NOLOCK) 
		           ON CD.cdCD = @cdCD
				  AND CD.cdSistema = @cdSistema
		   INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
				   ON RIS.IDItemDetalhe = ITE.IDItemDetalhe
		   INNER JOIN FornecedorParametro FPE WITH(NOLOCK)
				   ON FPE.IDFornecedorParametro = ITE.idFornecedorParametro
				  AND FPE.blAtivo = 1
		   INNER JOIN RelacionamentoItemPrime IP WITH(NOLOCK)
				   ON IP.idItemDetalhe = ITE.IDItemDetalhe
				  --AND ((IP.Sequencial = 1 AND CD.blConvertido = 1)  -- nao valida sequencial aqui, pois se nao for =1, deve barrar com a msg da outra spec
				  --     OR CD.blConvertido != 1)
		    LEFT JOIN RelacionamentoItemSecundario RIS2 WITH(NOLOCK)  
				   ON RIS2.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
		    LEFT JOIN RelacionamentoItemPrime RIP2 WITH(NOLOCK)
				   ON RIP2.IDItemDetalhe = RIS2.IDItemDetalhe
				  AND RIP2.cdCrossRef = IP.cdCrossRef
				  AND RIP2.Sequencial > 1
				  AND CD.blConvertido = 1
		    LEFT JOIN ItemDetalhe ID2 WITH(NOLOCK)
				   ON ID2.IDItemDetalhe = RIP2.IDItemDetalhe
				  AND dbo.fnObterTipoReabastecimento(ID2.IDItemDetalhe, cd.idcd, L.idloja) IN (20, 22, 40, 42, 43, 81)
	 WHERE ITE.cdItem = @cdItem
	   AND ITE.tpVinculado = 'E'
	   AND ITE.cdSistema = @cdSistema
	   AND dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, cd.IDCD, L.idloja) IN (20, 22, 40, 42, 43, 81)
	   AND ((CD.blConvertido = 1 AND ID2.IDItemDetalhe IS NOT NULL)
	        OR CD.blConvertido = 0)
	UNION ALL
	SELECT ITE.IDItemDetalhe  -- caso onde é item de entrada, prime, staple (cd convertido ou nao), e tem outro tpVinculado=S como secundario
	  FROM ItemDetalhe ITE WITH(NOLOCK)
		   INNER JOIN Loja L WITH(NOLOCK) 
		           ON l.cdLoja = @cdloja
				  AND l.cdSistema = @cdSistema
		   INNER JOIN CD WITH(NOLOCK) 
		           ON CD.cdCD = @cdCD
				  AND CD.cdSistema = @cdSistema
		   INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
				   ON RIS.IDItemDetalhe = ITE.IDItemDetalhe
		   INNER JOIN FornecedorParametro FPE WITH(NOLOCK)
				   ON FPE.IDFornecedorParametro = ITE.idFornecedorParametro
				  AND FPE.blAtivo = 1
		   INNER JOIN RelacionamentoItemPrime IP WITH(NOLOCK)
				   ON IP.idItemDetalhe = ITE.IDItemDetalhe
				  --AND ((IP.Sequencial = 1 AND CD.blConvertido = 1)  -- nao valida sequencial aqui, pois se nao for =1, deve barrar com a msg da outra spec
				  --     OR CD.blConvertido != 1)
		    LEFT JOIN RelacionamentoItemPrincipal RIS2 WITH(NOLOCK)  
				   ON RIS2.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
		    LEFT JOIN RelacionamentoItemPrime RIP2 WITH(NOLOCK)
				   ON RIP2.IDItemDetalhe = RIS2.IDItemDetalhe
				  AND RIP2.cdCrossRef = IP.cdCrossRef
				  AND RIP2.Sequencial > 1
				  AND CD.blConvertido = 1
		    LEFT JOIN ItemDetalhe ID2 WITH(NOLOCK)
				   ON ID2.IDItemDetalhe = RIP2.IDItemDetalhe
				  AND dbo.fnObterTipoReabastecimento(ID2.IDItemDetalhe, cd.idcd, L.idloja) IN (20, 22, 40, 42, 43, 81)
	 WHERE ITE.cdItem = @cdItem
	   AND ITE.tpVinculado = 'E'
	   AND ITE.cdSistema = @cdSistema
	   AND dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, cd.IDCD, L.idloja) IN (20, 22, 40, 42, 43, 81)
	   AND ((CD.blConvertido = 1 AND ID2.IDItemDetalhe IS NOT NULL)
	        OR CD.blConvertido = 0)
	UNION ALL
	SELECT TOP 1 ITE.IDItemDetalhe  -- caso onde é item de saida, não valida aqui; existe outra spec para lidar com tpVinculado=S;
	  FROM ItemDetalhe ITE WITH (NOLOCK)
	 WHERE ITE.cdItem = @cdItem
	   AND ITE.tpVinculado = 'S'
	   AND ITE.cdSistema = @cdSistema
	   AND EXISTS (SELECT TOP 1 1 FROM RelacionamentoitemPrincipal RIP1 WHERE RIP1.IDItemDetalhe = ITE.IDItemDetalhe)
    UNION ALL
	SELECT TOP 1 ITE.IDItemDetalhe -- caso onde não é staple, é vinculado de entrada, sendo ou não prime
      FROM ItemDetalhe ITE WITH (NOLOCK)
		   INNER JOIN Loja L WITH(NOLOCK)
		           ON l.cdLoja = @cdloja
				  AND l.cdSistema = @cdSistema
		   INNER JOIN CD WITH(NOLOCK)
		           ON CD.cdCD = @cdCD
				  AND CD.cdSistema = @cdSistema
	 WHERE ITE.cdItem = @cdItem
	   AND ITE.tpVinculado = 'E'
	   AND ITE.cdSistema = @cdSistema
	   AND dbo.fnObterTipoReabastecimento(ITE.IDItemDetalhe, cd.IDCD, L.idloja) NOT IN (20, 22, 40, 42, 43, 81)
	   --AND CD.blConvertido = 0
	   --AND NOT EXISTS(SELECT TOP 1 1 FROM RelacionamentoItemPrime RIP WHERE RIP.IDItemDetalhe = ITE.IDItemDetalhe AND RIP.Sequencial = 1) -- esta linha foi comentada pois agora ha validacao especifica do caso onde nao eh staple mas eh prime
	UNION ALL
	SELECT TOP 1 ID.IDItemDetalhe -- caso onde nao existe na tabela RelacionamentoItemPrime
	  FROM ItemDetalhe ID
	 WHERE NOT EXISTS(SELECT TOP 1 1 FROM RelacionamentoItemPrime RIP WHERE RIP.IDItemDetalhe = ID.IDItemDetalhe)
	   AND ID.cdItem = @cdItem
	   AND ID.cdSistema = @cdSistema
	   AND ID.tpVinculado = 'E'
) Qtds