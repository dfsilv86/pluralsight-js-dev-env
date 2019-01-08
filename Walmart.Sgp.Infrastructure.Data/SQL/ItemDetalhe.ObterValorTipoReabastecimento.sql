/*
DECLARE @idItemDetalheEntrada INT, @idCD INT;
SET @idItemDetalheEntrada = 45500;
SET @idCD = 7471;
--*/

select dbo.fnObterTipoReabastecimento(@idItemDetalheEntrada, @idCD, 1)