/*
DECLARE @IDUsuario INT, @IDLoja INT;
SET @IDUsuario = 3;
SET @IDLoja = 1;
--*/

SELECT TOP 1 1
  FROM Permissao P WITH (NOLOCK)
       INNER JOIN PermissaoBandeira PB WITH (NOLOCK) 
	           ON PB.IDPermissao = P.IDPermissao
       INNER JOIN Loja L WITH (NOLOCK)
	           ON L.IDBandeira = PB.IDBandeira
 WHERE L.IDLoja = @IDLoja
   AND P.IDUsuario = @IDUsuario
UNION
SELECT TOP 1 1
  FROM Permissao P WITH (NOLOCK)
       INNER JOIN PermissaoLoja PL WITH (NOLOCK) 
	           ON PL.IDPermissao = P.IDPermissao
 WHERE PL.IDLoja = @IDLoja
   AND P.IDUsuario = @IDUsuario;