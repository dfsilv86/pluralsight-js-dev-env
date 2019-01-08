/*DECLARE @cdItemSaida BIGINT, @cdLoja BIGINT, @cdSistema BIGINT;
SET @cdItemSaida = 500686032;
SET @cdLoja = 2801;
SET @cdSistema = 1;
--*/

SELECT 
	RILC.IDRelacaoItemLojaCD  
	, RILC.IDItem          
	, RILC.IDLojaCDParametro 
	, RILC.blAtivo 
	, RILC.cdSistema 
	, RILC.dhCriacao            
	, RILC.dhAtualizacao          
	, RILC.cdUsuarioCriacao 
	, RILC.cdUsuarioAtualizacao 
	, RILC.idItemEntrada       
	, RILC.vlTipoReabastecimento
	, RILC.cdCrossRef
FROM RelacaoItemLojaCD RILC WITH(NOLOCK)
INNER JOIN ItemDetalhe ID WITH(NOLOCK)
	ON ID.IDItemDetalhe = RILC.IDItem
INNER JOIN LojaCDParametro LCP WITH(NOLOCK)
	ON LCP.IDLojaCDParametro = RILC.IDLojaCDParametro
INNER JOIN Loja L WITH(NOLOCK)
	ON L.IDLoja = LCP.IDLoja
WHERE RILC.cdSistema = @cdSistema AND RILC.blAtivo = 1
	AND ID.cdItem = @cdItemSaida
	AND L.cdLoja = @cdLoja