/*
DECLARE @cdLoja INT, @cdSistema BIGINT;
SET @cdLoja = 1039;
SET @cdSistema = 1;
--*/

SELECT COUNT(*) FROM Loja AS L WITH (NOLOCK)
WHERE L.cdLoja = @cdLoja AND L.blAtivo = 1 AND L.cdSistema = @cdSistema