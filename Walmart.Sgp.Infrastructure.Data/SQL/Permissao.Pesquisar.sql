/*
DECLARE @IdUsuario INT, @IdLoja INT, @IdBandeira INT
SET @IdUsuario = 2337
SET @IdLoja = NULL
SET @IdBandeira = 1
--*/

SELECT 
     P.*, 
     B.*, 
     L.*
FROM
    Permissao P WITH (NOLOCK) 
                INNER JOIN PermissaoBandeira B WITH (NOLOCK) 
                    ON P.IDPermissao = B.IDPermissao
                INNER JOIN PermissaoLoja L WITH (NOLOCK) 
                    ON P.IDPermissao = L.IDPermissao
                INNER JOIN CWIUser U WITH (NOLOCK)
                    ON P.IdUsuario = U.Id
WHERE
    (@IdUsuario IS NULL OR P.IdUsuario = @IdUsuario)
AND (@IdBandeira IS NULL OR B.IdBandeira = @IdBandeira)
AND (@IdLoja IS NULL OR L.IdLoja = @IdLoja)
ORDER BY 
    U.Username        