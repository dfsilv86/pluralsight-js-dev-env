/*
DECLARE @IDLoja INT, @IDDepartamento INT, @IDCategoria INT, @DataInventario DATETIME;
SET @IDLoja = 1;
SET @IDDepartamento = 1;
SET @DataInventario = GETDATE();
--*/
UPDATE InventarioCritica
   SET blAtivo = 0
 WHERE blAtivo = 1
   AND IDLoja = @IDLoja
   AND (@IDDepartamento IS NULL OR IDDepartamento = @IDDepartamento)
   AND (@IDCategoria IS NULL OR IDCategoria = @IDCategoria)
   AND dhInventario = @DataInventario;