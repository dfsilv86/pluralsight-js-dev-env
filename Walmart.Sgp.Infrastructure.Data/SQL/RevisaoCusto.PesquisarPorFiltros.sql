/*
DECLARE @idBandeira INT, @idLoja INT, @idDepartamento INT, @cdItem INT, @dsItem VARCHAR, @idStatus INT
SET @idBandeira = 1
SET @idLoja = NULL
SET @idDepartamento = NULL
SET @cdItem = NULL
SET @dsItem = NULL
SET @idStatus = NULL
--*/

select
	 R.IDRevisaoCusto
	,R.dtSolicitacao
	,R.vlCustoSolicitado
	,R.dtCriacao
	,NULL AS SplitOn1
	,L.cdLoja
	,L.nmLoja
	,NULL AS SplitOn2
	,ID.cdItem
	,ID.dsItem
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
where L.IDBandeira = @idBandeira
  and (@idLoja IS NULL OR L.IDLoja = @idLoja)
  and (@idDepartamento IS NULL OR ID.IDDepartamento = @idDepartamento)
  and (@cdItem IS NULL OR ID.cdItem = @cdItem)
  and (@dsItem IS NULL OR ID.dsItem LIKE '%' + @dsItem + '%')
  and (@idStatus IS NULL OR S.IDStatusRevisaoCusto = @idStatus)