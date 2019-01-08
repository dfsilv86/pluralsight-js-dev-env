ALTER TABLE [dbo].[FornecedorParametro]  
add [idDepartamento] int null;

ALTER TABLE [dbo].[FornecedorParametro]  WITH NOCHECK ADD  CONSTRAINT [FK_FornecedorParametros_idDepartamento_Departamento] FOREIGN KEY([idDepartamento])
REFERENCES [dbo].[Departamento] ([idDepartamento])
GO

ALTER TABLE [dbo].[FornecedorParametro] CHECK CONSTRAINT [FK_FornecedorParametros_idDepartamento_Departamento]
GO
