SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Roteiro](
	[idRoteiro] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[vlCargaMinima] [int] NOT NULL,
	[blKgCx] [bit] NOT NULL,
	[idUsuarioCriacao] [int] NOT NULL,
	[dhCriacao] [datetime] NOT NULL,
	[idUsuarioAtualizacao] [int] NULL,
	[dhAtualizacao] [datetime] NULL,
	[blAtivo] [bit] NOT NULL,
	[cdV9D] [bigint] NOT NULL,
 CONSTRAINT [PK_Roteiro] PRIMARY KEY CLUSTERED 
(
	[idRoteiro] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Roteiro]  WITH CHECK ADD  CONSTRAINT [Roteiro_IdUsuarioAtualizacao_FK_CWIUser_Id] FOREIGN KEY([idUsuarioAtualizacao])
REFERENCES [dbo].[CWIUser] ([Id])
GO

ALTER TABLE [dbo].[Roteiro] CHECK CONSTRAINT [Roteiro_IdUsuarioAtualizacao_FK_CWIUser_Id]
GO

ALTER TABLE [dbo].[Roteiro]  WITH CHECK ADD  CONSTRAINT [Roteiro_IdUsuarioCriacao_FK_CWIUser_Id] FOREIGN KEY([idUsuarioCriacao])
REFERENCES [dbo].[CWIUser] ([Id])
GO

ALTER TABLE [dbo].[Roteiro] CHECK CONSTRAINT [Roteiro_IdUsuarioCriacao_FK_CWIUser_Id]
GO

ALTER TABLE [dbo].[Roteiro] ADD  DEFAULT ((1)) FOR [blAtivo]
GO

