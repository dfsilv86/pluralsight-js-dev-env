/*
DECLARE @cdItem BIGINT, @cdLoja BIGINT, @cdCD BIGINT, @cdSistema BIGINT;
SET @cdItem = 9400427;
SET @cdLoja = 1006;
SET @cdCD = 7471;
SET @cdSistema = 1;
--*/

DECLARE @blConvertido BIT, @idCD INT, @idLoja INT, @IDItemDetalhe INT, @IsPrimeWithSecondary BIT, @IsStaple BIT;

SELECT @blConvertido = blConvertido, @idCD = IDCD FROM CD WITH (NOLOCK) WHERE CDCD=@cdCD AND cdSistema = @cdSistema;

SELECT @idLoja = IDLoja FROM Loja WITH (NOLOCK) WHERE cdLoja = @cdLoja AND cdSistema = @cdSistema;

SELECT @IDItemDetalhe = IDItemDetalhe FROM ItemDetalhe ID WITH (NOLOCK) WHERE ID.cdItem = @cdItem;

SELECT @IsStaple = CASE WHEN dbo.fnObterTipoReabastecimento(@IDItemDetalhe, @idCD, @idLoja) IN (20, 22, 40, 42, 43, 81) THEN 1 ELSE 0 END;

SELECT TOP 1 @IsPrimeWithSecondary = 1
  FROM RelacionamentoItemPrime RIP1
       INNER JOIN RelacionamentoItemPrime RIP2
	           ON RIP2.cdCrossRef = RIP1.cdCrossRef
			  AND RIP2.Sequencial <> 1
			  AND dbo.fnObterTipoReabastecimento(RIP2.IDItemDetalhe, @idCD, @idLoja) IN (20, 22, 40, 42, 43, 81)
 WHERE RIP1.IDItemDetalhe = @IDItemDetalhe
   AND RIP1.Sequencial = 1;
   
-- Retorna null caso nao seja vinculado de saida, ou retorna flag se é valido quando atende todos os requisitos
SELECT CASE WHEN @blConvertido = 1 AND @IsStaple = 1 AND @IsPrimeWithSecondary = 1 THEN 1 ELSE 0 END AS IsValid, @blConvertido, @IsStaple, @IsPrimeWithSecondary
  FROM ItemDetalhe ID WITH (NOLOCK)
 WHERE ID.IDItemDetalhe = @IDItemDetalhe
   AND ID.cdSistema = @cdSistema
   AND ID.tpVinculado = 'S'
   AND EXISTS (SELECT TOP 1 1 FROM RelacionamentoitemPrincipal RIP1 WHERE RIP1.IDItemDetalhe = @IDItemDetalhe)  
 