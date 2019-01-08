/*
DECLARE @cdItem BIGINT, @cdLoja BIGINT, @cdCD BIGINT, @cdSistema BIGINT;
SET @cdItem = 500686027;
SET @cdLoja = 1006;
SET @cdCD = 7471;
SET @cdSistema = 1;
--*/

DECLARE @blConvertido BIT, @idCD INT, @idLoja INT;

SELECT @blConvertido = blConvertido, @idCD = IDCD FROM CD WITH (NOLOCK) WHERE CDCD=@cdCD AND cdSistema = @cdSistema;

SELECT @idLoja = IDLoja FROM Loja WITH (NOLOCK) WHERE cdLoja = @cdLoja AND cdSistema = @cdSistema;

SELECT dbo.fnObterTipoReabastecimento(RIS.IDItemDetalhe, @idCD, @idLoja) vlTipoReabastecimento--, CASE WHEN dbo.fnObterTipoReabastecimento(RIS.IDItemDetalhe, @idCD, @idLoja) IN (20, 22, 40, 42, 43, 81) THEN 1 ELSE 0 END AS IsStaple
  FROM RelacionamentoItemSecundario RIS WITH (NOLOCK)
       INNER JOIN RelacionamentoItemPrime RIPR WITH (NOLOCK)
	           ON RIPR.IDItemDetalhe = RIS.IDItemDetalhe
			  AND RIPR.Sequencial = 1
	   INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	           ON ID.IDItemDetalhe = RIS.IDItemDetalhe
 WHERE ID.tpVinculado = 'E'
   AND ID.cdItem = @cdItem
   AND ID.cdSistema = @cdsistema
   AND RIPR.IDItemDetalhe IS NOT NULL