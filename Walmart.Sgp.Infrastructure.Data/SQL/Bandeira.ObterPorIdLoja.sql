/*
DECLARE @idLoja INT
SET @idLoja = 2
--*/

SELECT B.*
  FROM Bandeira B WITH (NOLOCK)
       INNER JOIN Loja L WITH (NOLOCK)
	           ON L.IDBandeira = B.IDBandeira
 WHERE L.IDLoja = @idLoja