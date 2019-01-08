SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoteiroLoja](
	[idRoteiroLoja] [int] IDENTITY(1,1) NOT NULL,
	[idRoteiro] [int] NOT NULL,
	[idLoja] [int] NOT NULL,
	[blAtivo] [bit] NULL,
 CONSTRAINT [PK_RoteiroLoja] PRIMARY KEY CLUSTERED 
(
	[idRoteiroLoja] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoteiroLoja]  WITH CHECK ADD  CONSTRAINT [RoteiroLoja_idRoteiro_Roteiro] FOREIGN KEY([idRoteiro])
REFERENCES [dbo].[Roteiro] ([idRoteiro])
GO

ALTER TABLE [dbo].[RoteiroLoja] CHECK CONSTRAINT [RoteiroLoja_idRoteiro_Roteiro]
GO

ALTER TABLE [dbo].[RoteiroLoja] ADD  DEFAULT ((1)) FOR [blAtivo]
GO

ALTER TABLE [dbo].[RoteiroLoja]  WITH CHECK ADD  CONSTRAINT [RoteiroLoja_idloja_Loja] FOREIGN KEY([idloja])
REFERENCES [dbo].[Loja] ([idloja])
GO

ALTER TABLE [dbo].[RoteiroLoja] CHECK CONSTRAINT [RoteiroLoja_idloja_Loja]
GO

