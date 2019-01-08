/*
DECLARE @cdSistema int, @idBandeira int, @cdLoja int, @data datetime;
SET @cdSistema = 1;
SET @idBandeira = 1;
--SET @cdLoja = 87;
SET @data = '2016-01-22';
--SELECT COUNT(1) FROM LOJA
*/

SELECT
	COUNT(1)
FROM
	Bandeira B WITH (NOLOCK)
		INNER JOIN Loja L 
			ON B.IDBandeira = L.IDBandeira		
WHERE
	B.cdSistema = @cdSistema AND
	B.IDBandeira = ISNULL(@idBandeira, B.IDBandeira) AND
	L.cdLoja = ISNULL(@cdLoja, L.cdLoja) 	