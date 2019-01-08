/*
DECLARE @idItemDetalhe INT;
SET @idItemDetalhe = 16;
*/

SELECT ID.IdItemDetalhe
     , NULL AS SplitOn1
	 , RC.IdRegiaoCompra
     , RC.dsRegiaoCompra
     , NULL AS SplitOn2
     , AC.IdAreaCD
     , AC.dsAreaCD
  FROM ItemDetalhe ID WITH (NOLOCK)
	   LEFT JOIN RegiaoCompra RC WITH(NOLOCK)
			   ON RC.IdRegiaoCompra = ID.idRegiaoCompra
	   LEFT JOIN AreaCD AC WITH(NOLOCK)
			   ON AC.IdAreaCD = ID.idAreaCD
 WHERE ID.IDItemDetalhe = @idItemDetalhe