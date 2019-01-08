CREATE NONCLUSTERED INDEX IX_RelacaoItemLojaCD_cdItem_cdCD
ON [dbo].[RelacaoItemLojaCD] ([cdItem],[cdCD])
INCLUDE ([cdCd])
GO

