/*
DECLARE @id INT
set @id = 1
--*/

SELECT 
	 RT.IDRelacionamentoTransferencia
	,RT.IDItemDetalheOrigem
	,RT.IDItemDetalheDestino
	,RT.IDLoja
	,RT.dtCriacao
	,RT.IDUsuario
	,RT.blAtivo
	,RT.dtInativo
	,NULL AS SplitOn1
	,IDO.cdItem
	,IDO.dsItem
	,IDO.tpUnidadeMedida 
	,IDO.vlFatorConversao
	,NULL AS SplitOn2
	,DO.cdDepartamento
	,DO.dsDepartamento
	,NULL AS SplitOn3
	,IDD.cdItem
	,IDD.dsItem
	,IDD.tpUnidadeMedida 
	,IDD.vlFatorConversao
	,NULL AS SplitOn4
	,DD.cdDepartamento
	,DD.dsDepartamento
	,NULL AS SplitOn5
	,L.cdLoja
	,L.nmLoja
	,NULL AS SplitOn6
	,U.Id
	,U.Username
FROM RelacionamentoTransferencia RT (NOLOCK)
JOIN ItemDetalhe IDO (NOLOCK) ON RT.IDItemDetalheOrigem = IDO.IDItemDetalhe
JOIN Departamento DO (NOLOCK) ON IDO.IDDepartamento = DO.IDDepartamento
JOIN ItemDetalhe IDD (NOLOCK) ON RT.IDItemDetalheDestino = IDD.IDItemDetalhe
JOIN Departamento DD (NOLOCK) ON IDD.IDDepartamento = DD.IDDepartamento
JOIN Loja L (NOLOCK) ON RT.IDLoja = L.IDLoja
JOIN Bandeira B (NOLOCK) ON L.IDBandeira = B.IDBandeira
JOIN CWIUser U (NOLOCK) ON RT.IDUsuario = U.Id
WHERE RT.IDRelacionamentoTransferencia = @id