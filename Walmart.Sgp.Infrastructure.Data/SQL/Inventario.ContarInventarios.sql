/*
DECLARE @idLoja INT, @idDepartamento INT, @inicioDhInventario DATETIME, @fimDhInventario DATETIME, @stInventarios INT
SET @idLoja = 1;
SET @idDepartamento = 80;
SET @inicioDhInventario = '2016-01-01';
SET @fimDhInventario = '2016-06-01';
SET @fimDhInventario = NULL
--*/

SELECT 
	COUNT(1) 
FROM
	Inventario I WITH (NOLOCK)	
WHERE
		I.IDLoja = @idLoja
    AND I.IDDepartamento = @idDepartamento
	AND I.dhInventario >= @inicioDhInventario AND I.dhInventario < @fimDhInventario 
	AND I.stInventario IN @stInventarios
