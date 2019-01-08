/*
DECLARE @cdItemEntrada INT, @cdItemSaida INT, @cdSistema INT;
SET @cdItemEntrada = 9400427; --500071739;
SET @cdItemSaida = 500071739; --9400427;
SET @cdSistema = 1;
--*/

SELECT TOP 1 1
  FROM (
		SELECT 1 Itens
		  FROM RelacionamentoItemPrincipal AS RIP WITH(NOLOCK)
		       INNER JOIN ItemDetalhe ID_S WITH(NOLOCK)
			           ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
		       INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
			           ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
		       INNER JOIN ItemDetalhe ID_E WITH(NOLOCK)
			           ON RIS.IDItemDetalhe = ID_E.IDItemDetalhe
		 WHERE ID_S.cdItem = @cdItemSaida
		   AND ID_S.cdSistema = @cdSistema
		   AND ID_E.cdItem = @cdItemEntrada
		   AND ID_E.cdSistema = @cdSistema
		UNION	
		SELECT 1 Itens
		  FROM RelacionamentoItemPrincipal RIP WITH(NOLOCK)
		       INNER JOIN ItemDetalhe ID_S WITH(NOLOCK)
			           ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
		       INNER JOIN RelacionamentoItemPrime ITP WITH(NOLOCK)
			           ON ITP.idItemDetalhe = RIP.IDItemDetalhe 
					  AND ITP.Sequencial = 1
		 WHERE ID_S.cdItem = @cdItemSaida
		   AND ID_S.cdSistema = @cdSistema 
		UNION
		-- verifica a relação como se fosse inversa pois no caso em que se tenta vincular o item de saída no de entrada (o contrário do normal), 
		-- não deve emitir a msg de que não possui vinculo; vai ser barrado em outra spec (a do ItemXrefPrime)
		SELECT 1 Itens  
		  FROM RelacionamentoItemPrincipal AS RIP WITH(NOLOCK)
		       INNER JOIN ItemDetalhe ID_S WITH(NOLOCK)
			           ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
		       INNER JOIN RelacionamentoItemSecundario RIS WITH(NOLOCK)
			           ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
		       INNER JOIN ItemDetalhe ID_E WITH(NOLOCK)
			           ON RIS.IDItemDetalhe = ID_E.IDItemDetalhe
		 WHERE ID_E.cdItem = @cdItemSaida
		   AND ID_E.cdSistema = @cdSistema
		   AND ID_S.cdItem = @cdItemEntrada
		   AND ID_S.cdSistema = @cdSistema
		-- tambem aceita o caso onde ambos os cditens sao iguais se o cditem for um vinculado de saida
		-- mesmo motivo
        UNION
		SELECT 1 Itens  
		  FROM RelacionamentoItemPrincipal AS RIP WITH(NOLOCK)
		       INNER JOIN ItemDetalhe ID_S WITH(NOLOCK)
			           ON RIP.IDItemDetalhe = ID_S.IDItemDetalhe AND RIP.cdSistema = @cdSistema AND RIP.IDTipoRelacionamento = 1
		 WHERE ID_S.cdItem = @cdItemSaida
		   AND ID_S.cdItem = @cdItemEntrada
		   AND ID_S.cdSistema = @cdSistema
		   AND ID_S.tpVinculado = 'S'
       ) AS ITENS