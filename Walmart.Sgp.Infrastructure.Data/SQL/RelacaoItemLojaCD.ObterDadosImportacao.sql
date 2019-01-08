/*
DECLARE @cdLoja INT, @cdCD INT, @cdItemEntrada INT, @cdItemSaida INT;

SET @cdCD = 7424;
SET @cdLoja = 36;
SET @cdItemEntrada = 500247569;
SET @cdItemSaida = 500491087;
--*/

DECLARE @idCD INT
	,@cdSistema INT
	,@idLoja INT
	,@idItemDetalheSaida INT
	,@idLojaCDParametro INT
	,@idRelacaoItemLojaCD INT

SELECT @idCD = IDCD
	,@cdSistema = cdSistema
FROM CD WITH (NOLOCK)
WHERE cdCD = @cdCD

SELECT @idLoja = IDLoja
FROM Loja WITH (NOLOCK)
WHERE cdLoja = @cdLoja
	AND cdSistema = @cdSistema

SELECT @idItemDetalheSaida = IDItemDetalhe
FROM ItemDetalhe WITH (NOLOCK)
WHERE cdItem = @cdItemSaida
	AND cdSistema = @cdSistema

SELECT @idLojaCDParametro = IDLojaCDParametro
FROM LojaCDParametro WITH (NOLOCK)
WHERE IDLoja = @idLoja
	AND IDCD = @idCD

SELECT @idRelacaoItemLojaCD = RIL.IDRelacaoItemLojaCD
FROM RelacaoItemLojaCD RIL
WHERE RIL.IDLojaCDParametro = @idLojaCDParametro
	AND RIL.IDItem = @idItemDetalheSaida

SELECT @idRelacaoItemLojaCD AS IDRelacaoItemLojaCD
	,@idLojaCDParametro AS IDLojaCDParametro
	,@cdSistema AS cdSistema
	,dbo.fnObterTipoReabastecimento(IDE.IDItemDetalhe, @idCD, @idLoja) AS VlTipoReabastecimento
	,@idItemDetalheSaida AS IDItem
	,IDE.IDItemDetalhe AS IdItemEntrada
	,CASE 
		WHEN (
				RIPR.Sequencial = 1
				AND (
					dbo.fnObterTipoReabastecimento(IDE.IDItemDetalhe, @idCD, @idLoja) IN (
						20
						,22
						,40
						,42
						,43
						,81
						)
					)
				)
			THEN RIPR.CdCrossRef
		ELSE NULL
		END AS CdCrossRef
FROM ItemDetalhe IDE WITH (NOLOCK)
LEFT JOIN RelacionamentoItemPrime RIPR WITH (NOLOCK) ON RIPR.idItemDetalhe = IDE.IDItemDetalhe
WHERE IDE.cdItem = @cdItemEntrada
	AND IDE.cdSistema = @cdSistema