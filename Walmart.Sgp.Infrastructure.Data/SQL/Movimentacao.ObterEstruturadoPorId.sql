/*
DECLARE @idMovimentacao INT;

SET @idMovimentacao = 650273465;
--*/

SELECT 
	M.IDMovimentacao,
	M.qtdMovimentacao,
	NULL AS SplitOn1,
	I.IDItemDetalhe,
	I.cdOldNumber,
	I.dsItem,   
	NULL AS SplitOn2,
	IDP.IDDepartamento,
	IDP.cdDepartamento,
	IDP.dsDepartamento,
	NULL AS SplitOn3,
	IT.IDItemDetalhe,
	IT.cdOldNumber,
	IT.dsItem,   
	NULL AS SplitOn4,
	ITDP.IDDepartamento,
	ITDP.cdDepartamento,
	ITDP.dsDepartamento
FROM 
	Movimentacao M WITH (NOLOCK)
				INNER JOIN ItemDetalhe I WITH (NOLOCK)
                            ON M.IDItem = I.IDItemDetalhe
                   INNER JOIN Departamento IDP WITH (NOLOCK)
                            ON IDP.IDDepartamento = I.IDDepartamento
                   LEFT JOIN ItemDetalhe IT WITH (NOLOCK)
                            ON M.IdItemTransferencia = IT.IDItemDetalhe
                   LEFT JOIN Departamento ITDP WITH (NOLOCK)
                            ON ITDP.IDDepartamento = IT.IDDepartamento                                      
WHERE    
	M.IDMovimentacao = @idMovimentacao