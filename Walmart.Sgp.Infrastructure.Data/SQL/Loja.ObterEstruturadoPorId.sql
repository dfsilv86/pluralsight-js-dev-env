/*
DECLARE @IDLoja INT;
SET @IDLoja = 22;
--*/

SELECT L.*, NULL AS SplitOn1, B.*, NULL AS SplitOn2, F.*, NULL AS SplitOn3, D.*, NULL AS SplitOn4, R.*
  FROM Loja L WITH (NOLOCK)
  LEFT JOIN Bandeira B WITH (NOLOCK)
         ON B.IDBandeira = L.IDBandeira
  LEFT JOIN Formato F WITH (NOLOCK)
         ON F.IDFormato = B.IDFormato
  LEFT JOIN Distrito D WITH (NOLOCK)
         ON D.IDDistrito = L.IDDistrito
  LEFT JOIN Regiao R WITH (NOLOCK)
        ON R.IDRegiao = D.IDRegiao
 WHERE L.IDLoja = @IDLoja;