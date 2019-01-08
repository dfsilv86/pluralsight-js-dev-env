/*
DECLARE @tpUnidadeMedida char(1), @vlFatorConversao decimal, @idItemDetalhe bigint, @cdSistema smallint;
set @tpUnidadeMedida= null;
set @vlFatorConversao = 1;
set @idItemDetalhe = 16;
set @cdSistema = 1;

--select * from itemdetalhe where iditemdetalhe = 16
--*/

UPDATE ItemDetalhe
   SET tpUnidadeMedida = @tpUnidadeMedida
     , vlFatorConversao = @vlFatorConversao
 WHERE idItemDetalhe = @idItemDetalhe
   AND cdSistema = @cdSistema;