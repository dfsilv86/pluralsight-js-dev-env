/*
DECLARE @cdCD INT, @cdSistema BIGINT;
SET @cdCD = 7472;
SET @cdSistema = 1;
--*/

SELECT COUNT(*) FROM CD WITH (NOLOCK)
WHERE CD.cdCD = @cdCD AND CD.blAtivo = 1 AND CD.cdSistema = @cdSistema