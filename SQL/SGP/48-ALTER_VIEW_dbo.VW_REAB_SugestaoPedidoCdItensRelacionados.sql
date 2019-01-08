
CREATE VIEW [dbo].[VW_REAB_SugestaoPedidoCdItensRelacionados]
AS
WITH Itens (IDRelacionamentoItemSecundario, IdItemDetalheS, dsItemS, cdItemS, IdItemDetalheP, dsItemP, cdItemP,tpVinculado, tpManipulado, tpReceituario, TotalParticipacaoReceita, TotalRendimentoReceita, PesoUnitatio, TotalParticipacaoReceitaOriginal, TotalRendimentoReceitaOriginal, idCd, cdCd)
AS
(	--Pai / Derivado
	SELECT RS.IDRelacionamentoItemSecundario IDRelacionamentoItemSecundario,
		   IDP.IdItemDetalhe IdItemDetalheP,
		   IDP.dsItem dsItemP,
		   IDP.cdItem cdItemP,
		   IDS.IdItemDetalhe IdItemDetalheS,
		   IDS.dsItem dsItemS,
		   IDS.cdItem cdItemS,
		   IDS.tpVinculado,
		   IDS.tpManipulado,
		   IDS.tpReceituario, 		   
		   CAST(1 AS FLOAT) AS TotalParticipacaoReceita,
		   CAST(RS.pcRendimentoDerivado / 100 AS FLOAT) TotalRendimentoReceita,
		   CAST (1 AS DECIMAL(18, 3))  PesoUnitatio,
		   CAST(1 AS FLOAT) AS TotalParticipacaoReceitaOriginal,
		   CAST(RS.pcRendimentoDerivado / 100 AS FLOAT) TotalRendimentoReceitaOriginal,
		   IEP.idCD,
		   CD.cdCD
	  FROM RelacionamentoItemPrincipal RP WITH(NOLOCK) INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
												  ON RP.IDRelacionamentoItemPrincipal = RS.IDRelacionamentoItemPrincipal
												  AND RP.cdSistema = 1
												  AND RP.IDTipoRelacionamento = 3
										  INNER JOIN ItemDetalhe IDP WITH(NOLOCK)
												  ON IDP.IdItemDetalhe = RP.IdItemDetalhe
												 AND IDP.cdSistema = 1
												 AND IDP.blAtivo = 1
												 AND IDP.tpManipulado = 'P'
												 AND IDP.blItemTransferencia = 0
										  INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
												  ON IDS.IdItemDetalhe = RS.IdItemDetalhe
												 AND IDS.cdSistema = 1
												 AND IDS.blAtivo = 1
												 AND IDS.tpManipulado = 'D'
												 AND IDS.blItemTransferencia = 0
										  INNER JOIN Departamento DP WITH(NOLOCK)
												  ON IDP.IDDepartamento = DP.IDDepartamento
												 AND DP.blPerecivel = 'S'
												 AND DP.blAtivo = 1
										  INNER JOIN Departamento DS WITH(NOLOCK)
												  ON IDS.IDDepartamento = DS.IDDepartamento
												 AND DS.blPerecivel = 'S'
												 AND DS.blAtivo = 1
										  INNER JOIN WLMSLP_STAGE..SugestaoPedidoCD IEP WITH(NOLOCK)
												  ON IEP.idItemDetalheSugestao = RP.IdItemDetalhe
										  INNER JOIN CD cd WITH(NOLOCK)
												ON cd.IDCD = IEP.idCD

	UNION ALL

	--Insumo / Transformado
	SELECT RS.IDRelacionamentoItemSecundario IDRelacionamentoItemSecundario,
		   IDS.IdItemDetalhe IdItemDetalheP,
		   IDS.dsItem dsItemP,
		   IDS.cdItem cdItemP,
		   IDP.IdItemDetalhe IdItemDetalheS,
		   IDP.dsItem dsItemS,	   
		   IDP.cdItem cdItemS,
		   IDP.tpVinculado,
		   IDP.tpManipulado,  
		   IDP.tpReceituario,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) TotalParticipacaoReceita,
		   (RP.pcRendimentoReceita / 100) AS TotalRendimentoReceita,
		   CASE WHEN RP.psUnitario = 0 THEN 1 ELSE RP.psUnitario END PesoUnitatio,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) TotalParticipacaoReceitaOriginal,
		   (RP.pcRendimentoReceita / 100) AS TotalRendimentoReceitaOriginal,
		   IEP.idCD,
		   CD.cdCD
	  FROM RelacionamentoItemPrincipal RP WITH(NOLOCK) INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
												  ON RP.IDRelacionamentoItemPrincipal = RS.IDRelacionamentoItemPrincipal
												  AND RP.cdSistema = 1
												  AND RP.IDTipoRelacionamento = 2
										  INNER JOIN ItemDetalhe IDP WITH(NOLOCK)
												  ON IDP.IdItemDetalhe = RP.IdItemDetalhe
												 AND IDP.cdSistema = 1
												 AND IDP.blAtivo = 1
												 AND IDP.tpReceituario = 'T'
												 AND IDP.blItemTransferencia = 0
										  INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
												  ON IDS.IdItemDetalhe = RS.IdItemDetalhe
												 AND IDS.cdSistema = 1
												 AND IDS.blAtivo = 1
												 AND IDS.tpReceituario = 'I'
												 AND IDS.blItemTransferencia = 0
										  INNER JOIN Departamento DP WITH(NOLOCK)
												  ON IDP.IDDepartamento = DP.IDDepartamento
												 --AND DP.blPerecivel = 'S'
												 AND DP.blAtivo = 1
										  INNER JOIN Departamento DS WITH(NOLOCK)
												  ON IDS.IDDepartamento = DS.IDDepartamento
												 --AND DS.blPerecivel = 'S'
												 AND DS.blAtivo = 1
										  INNER JOIN WLMSLP_STAGE..SugestaoPedidoCD IEP WITH(NOLOCK)
												  ON IEP.idItemDetalheSugestao = RS.IdItemDetalhe
											INNER JOIN CD cd WITH(NOLOCK)
												ON cd.IDCD = IEP.idCD	 

	UNION ALL

	--Transformado / Transformado Recursivo
	SELECT RS.IDRelacionamentoItemSecundario IDRelacionamentoItemSecundario,
		   I.IdItemDetalheS IdItemDetalheP,
		   I.dsItemS dsItemP,
		   I.cdItemS cdItemP,
		   IDP.IdItemDetalhe IdItemDetalheS,
		   IDP.dsItem dsItemS,	   	   
		   IDP.cdItem cdItemS,
		   IDP.tpVinculado,
		   IDP.tpManipulado,  
		   IDP.tpReceituario,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) * I.TotalParticipacaoReceita TotalParticipacaoReceita,
		   (RP.pcRendimentoReceita / 100) * I.TotalRendimentoReceita AS TotalRendimentoReceita,
		   CASE WHEN RP.psUnitario = 0 THEN 1 ELSE RP.psUnitario END PesoUnitatio,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) TotalParticipacaoReceitaOriginal,
		   (RP.pcRendimentoReceita / 100) TotalRendimentoReceitaOriginal,
		   I.idCd,
		   I.cdCD
	  FROM RelacionamentoItemPrincipal RP WITH(NOLOCK) INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
												  ON RP.IDRelacionamentoItemPrincipal = RS.IDRelacionamentoItemPrincipal
												  AND RP.cdSistema = 1
												  AND RP.IDTipoRelacionamento = 2
										  INNER JOIN ItemDetalhe IDP WITH(NOLOCK)
												  ON IDP.IdItemDetalhe = RP.IdItemDetalhe
												 AND IDP.cdSistema = 1
												 AND IDP.blAtivo = 1
												 AND IDP.tpReceituario = 'T'
												 AND IDP.blItemTransferencia = 0
										  INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
												  ON IDS.IdItemDetalhe = RS.IdItemDetalhe
												 AND IDS.cdSistema = 1
												 AND IDS.blAtivo = 1
												 AND IDS.tpReceituario = 'T'
												 AND IDS.blItemTransferencia = 0
										  INNER JOIN Departamento DP WITH(NOLOCK)
												  ON IDP.IDDepartamento = DP.IDDepartamento
												 --AND DP.blPerecivel = 'S'
												 AND DP.blAtivo = 1
										  INNER JOIN Departamento DS WITH(NOLOCK)
												  ON IDS.IDDepartamento = DS.IDDepartamento
												 --AND DS.blPerecivel = 'S'
												 AND DS.blAtivo = 1
										  INNER JOIN Itens I 
												  ON I.IdItemDetalheP =  RS.IdItemDetalhe

	 UNION ALL 

	--Insumo / Transformado Recursivo
	SELECT RS.IDRelacionamentoItemSecundario IDRelacionamentoItemSecundario,
		   I.IdItemDetalheS IdItemDetalheP,
		   I.dsItemS dsItemP,
		   I.cdItemS cdItemP,
		   IDP.IdItemDetalhe IdItemDetalheS,
		   IDP.dsItem dsItemS,	   	   
		   IDP.cdItem cdItemS,
		   IDP.tpVinculado,
		   IDP.tpManipulado,  
		   IDP.tpReceituario,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) TotalParticipacaoReceita,
		   (RP.pcRendimentoReceita / 100) AS TotalRendimentoReceita,
		   CASE WHEN RP.psUnitario = 0 THEN 1 ELSE RP.psUnitario END PesoUnitatio,
		   dbo.fnObterParticipacaoReceita(IDS.IdItemDetalhe, IDP.IdItemDetalhe) TotalParticipacaoReceitaOriginal,
		   (RP.pcRendimentoReceita / 100) AS TotalRendimentoReceitaOriginal,
		   I.idCd,
		   I.cdCD
	  FROM RelacionamentoItemPrincipal RP WITH(NOLOCK) INNER JOIN RelacionamentoItemSecundario RS WITH(NOLOCK)
												  ON RP.IDRelacionamentoItemPrincipal = RS.IDRelacionamentoItemPrincipal
												 AND RP.cdSistema = 1
												 AND RP.IDTipoRelacionamento = 2
										  INNER JOIN ItemDetalhe IDP WITH(NOLOCK)
												  ON IDP.IdItemDetalhe = RP.IdItemDetalhe
												 AND IDP.cdSistema = 1
												 AND IDP.blAtivo = 1
												 AND IDP.tpReceituario = 'T'
												 AND IDP.blItemTransferencia = 0
										  INNER JOIN ItemDetalhe IDS WITH(NOLOCK)
												  ON IDS.IdItemDetalhe = RS.IdItemDetalhe
												 AND IDS.cdSistema = 1
												 AND IDS.blAtivo = 1
												 AND IDS.tpReceituario = 'I'
												 AND IDS.blItemTransferencia = 0
										  INNER JOIN Departamento DP WITH(NOLOCK)
												  ON IDP.IDDepartamento = DP.IDDepartamento
											   --AND DP.blPerecivel = 'S'
												 AND DP.blAtivo = 1
										  INNER JOIN Departamento DS WITH(NOLOCK)
												  ON IDS.IDDepartamento = DS.IDDepartamento
											   --AND DS.blPerecivel = 'S'
												 AND DS.blAtivo = 1
										  INNER JOIN Itens I 
												  ON I.IdItemDetalheP = RS.IdItemDetalhe
)
SELECT DISTINCT I.*
  FROM Itens I WITH(NOLOCK)

GO


