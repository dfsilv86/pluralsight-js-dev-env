/*
DECLARE @idRevisaoCusto INT
SET @idRevisaoCusto = NULL
--*/

select
	 R.IDRevisaoCusto
	,R.IDLoja
	,R.IDItemDetalhe
	,R.IDStatusRevisaoCusto
	,R.IDMotivoRevisaoCusto
	,R.IDUsuarioSolicitante
	,R.dtSolicitacao
	,R.vlCustoSolicitado
	,R.dtCriacao
	,R.dsMotivo
	,R.IDUsuarioRevisor
	,R.dsRevisor
	,R.dtRevisado
	,R.vlCustoRevisado
	,R.dtCustoRevisado
	,NULL AS SplitOn1
	,L.cdLoja
	,L.nmLoja
	,L.cdSistema
	,L.IDBandeira	
	,NULL AS SplitOn2
	,ID.IDItemDetalhe
	,ID.cdItem
	,ID.dsItem
	,ID.tpVinculado
	,ID.tpReceituario
	,ID.tpManipulado
	,NULL AS SplitOn3
	,d.cdDepartamento
	,d.dsDepartamento
	,NULL AS SplitOn4
	,M.IDMotivo
	,M.dsMotivo
	,NULL AS SplitOn5
	,U.Id
	,U.Username
	,NULL AS SplitOn6
	,S.IDStatusRevisaoCusto
	,S.dsStatus
from RevisaoCusto r (nolock)
join Loja l (nolock) on r.IDLoja = l.IDLoja
join ItemDetalhe id (nolock) on r.IDItemDetalhe = id.IDItemDetalhe
join Departamento d (nolock) on id.IDDepartamento = d.IDDepartamento
join StatusRevisaoCusto s (nolock) on r.IDStatusRevisaoCusto = s.IDStatusRevisaoCusto
join MotivoRevisaoCusto m (nolock) on r.IDMotivoRevisaoCusto = m.IDMotivo
join CWIUser u (nolock) on r.IDUsuarioSolicitante = u.Id
where r.IDRevisaoCusto = @idRevisaoCusto