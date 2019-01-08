/*
DECLARE @idSugestaoPedido INT;
SET @idSugestaoPedido = 41150886;
*/

SELECT 
	A.IdAutorizaPedido,
	A.dtPedido,
	A.dtAutorizacao,
	NULL AS SplitOn1,
	L.cdLoja,
	L.nmLoja,
	NULL AS SplitOn2,
	D.cdDepartamento,
	D.dsDepartamento,
	NULL AS SplitOn3,
	U.Username,
	U.FullName
FROM
	AutorizaPedido A WITH(NOLOCK)
		INNER JOIN Loja L WITH(NOLOCK)
			ON A.IdLoja = L.IDLoja
		INNER JOIN Departamento D WITH(NOLOCK)
			ON A.IDDepartamento = D.IDDepartamento
		INNER JOIN CWIUser U WITH(NOLOCK)
			ON A.IdUser = U.Id	
		INNER JOIN SugestaoPedido S WITH (NOLOCK)
			ON A.IdLoja = S.IdLoja AND A.dtPedido = S.dtPedido
WHERE
	S.IDSugestaoPedido = @idSugestaoPedido