/*
DECLARE @cultureCode VARCHAR(5);
SET @CultureCode= 'pt-BR';
--*/

-- TODO: cadastrar valores em en-US e deixar o globalization traduzir?

SELECT DV.dsValue AS tpStatus, DV.dsText AS [Text]
  FROM CWIDomainValue DV WITH (NOLOCK)
 WHERE DV.IDDomain = 5
   AND (@cultureCode IS NULL OR DV.dsCultureCode = @cultureCode)
 ORDER BY DV.SortOrder