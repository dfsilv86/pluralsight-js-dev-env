SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoteiroPedido](
	[idRoteiroPedido] [int] IDENTITY(1,1) NOT NULL,
	[idRoteiro] [int] NOT NULL,
	[idSugestaoPedido] [bigint] NOT NULL,
	[blAutorizado] [bit] NOT NULL,
	[idUsuarioAutorizacao] [int] NULL,
	[dhAutorizacao] [datetime] NULL,
 CONSTRAINT [PK_RoteiroPedido] PRIMARY KEY CLUSTERED 
(
	[idRoteiroPedido] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoteiroPedido]  WITH CHECK ADD  CONSTRAINT [RoteiroPedido_idRoteiro_Roteiro] FOREIGN KEY([idRoteiro])
REFERENCES [dbo].[Roteiro] ([idRoteiro])
GO

ALTER TABLE [dbo].[RoteiroPedido] CHECK CONSTRAINT [RoteiroPedido_idRoteiro_Roteiro]
GO

ALTER TABLE [dbo].[RoteiroPedido]  WITH CHECK ADD  CONSTRAINT [RoteiroPedido_idSugestaoPedido_SugestaoPedido] FOREIGN KEY([idSugestaoPedido])
REFERENCES [dbo].[SugestaoPedido] ([IDSugestaoPedido])
GO

ALTER TABLE [dbo].[RoteiroPedido] CHECK CONSTRAINT [RoteiroPedido_idSugestaoPedido_SugestaoPedido]
GO

ALTER TABLE [dbo].[RoteiroPedido] ADD  DEFAULT ((0)) FOR [blAutorizado]
GO

ALTER TABLE [dbo].[RoteiroPedido]  WITH CHECK ADD  CONSTRAINT [RoteiroPedido_idUsuarioAutorizacao_CWIUser] FOREIGN KEY([idUsuarioAutorizacao])
REFERENCES [dbo].[CWIUser] ([Id])
GO

ALTER TABLE [dbo].[RoteiroPedido] CHECK CONSTRAINT [RoteiroPedido_idUsuarioAutorizacao_CWIUser]
GO

Create index RoteiroPedido_idUsuarioAutorizacao_CWIUser on [dbo].[RoteiroPedido] (idUsuarioAutorizacao)
go
