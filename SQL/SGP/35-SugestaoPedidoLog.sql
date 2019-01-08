SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SugestaoPedidoLog](
	[IdAuditRecord] [int] IDENTITY(1,1) NOT NULL,
	[IdAuditUser] [int] NOT NULL,
	[DhAuditStamp] [datetime] NOT NULL,
	[CdAuditKind] [int] NOT NULL,
	[IDSugestaoPedido] [int] NOT NULL,
	[qtdPackCompra] [int] NOT NULL,
	[vlEstoque] [decimal](11, 3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAuditRecord] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO


