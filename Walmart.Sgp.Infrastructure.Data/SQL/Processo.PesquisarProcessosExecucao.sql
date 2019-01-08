/*
DECLARE @logDataInicio AS DATETIME,
        @logDataFim AS DATETIME,
        @idProcesso AS INT,
        @cdSistema AS INT,
        @idBandeira AS INT,
        @idLoja AS INT,
        @idItemDetalhe AS INT

SET @cdSistema = 1
SET @idBandeira = 1
SET @logDataInicio = '2016-01-20'
SET @logDataFim = '2016-01-24 23:59:59';
*/

SELECT LE.Data
     , LE.IdItemDetalhe
     , LE.IdLoja
     , LE.Mensagem
     , NULL AS SplitOn1
	 , ID.cdItem 
	 , ID.dsItem 
	 , NULL AS SplitOn2
     , LJ.cdLoja
	 , LJ.nmLoja
     , NULL AS SplitOn3
     , LTE.Descricao     
     , LTP.Descricao AS DescricaoProcesso
  FROM LogExcecao LE WITH(NOLOCK)
  JOIN ItemDetalhe ID WITH(NOLOCK) 
    ON LE.IdItemDetalhe = ID.IDItemDetalhe
  JOIN Loja LJ WITH(NOLOCK) 
    ON LE.IdLoja = LJ.IDLoja
  JOIN LogTipoExcecao LTE WITH(NOLOCK) 
    ON LE.IdTipoLogExcecao = LTE.IdTipoLogExcecao
  JOIN LogExecucao LEX WITH(NOLOCK) 
    ON LE.IdLogExecucao = LEX.IdLogExecucao
  JOIN LogTipoProcesso LTP WITH(NOLOCK) 
    ON LEX.IdProcesso = LTP.IdProcesso
 WHERE LE.Data >= @logDataInicio
   AND LE.Data < @logDataFim
   AND LTP.IdProcesso = ISNULL(@idProcesso, LTP.IdProcesso)
   AND LJ.cdSistema = @cdSistema
   AND LJ.IDBandeira = @idBandeira
   AND LE.IdLoja = ISNULL(@idLoja, LE.IdLoja)
   AND LE.IdItemDetalhe = ISNULL(@idItemDetalhe, LE.IdItemDetalhe)