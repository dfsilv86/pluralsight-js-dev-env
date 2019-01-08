alter  TABLE [dbo].[FornecedorLojaParametro]
add	[blAtivo] [bit] NOT NULL DEFAULT 1,
	[dhCriacao] [datetime] NULL,
	[dhAtualizacao] [datetime] NULL,
	[cdUsuarioCriacao] [int] NULL,
	[cdUsuarioAtualizacao] [int] NULL

