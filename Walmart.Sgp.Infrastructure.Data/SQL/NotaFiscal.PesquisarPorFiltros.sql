/*
DECLARE @cdSistema AS INT
      , @idBandeira AS INT
      , @cdLoja AS INT
      , @cdFornecedor AS INT
      , @nrNotaFiscal AS INT
      , @cdItem AS INT
      , @dtRecebimentoInicio AS DATETIME
      , @dtRecebimentoFim AS DATETIME
      , @dtCadastroConcentradorInicio AS DATETIME
      , @dtCadastroConcentradorFim AS DATETIME
      , @dtAtualizacaoConcentradorInicio AS DATETIME
      , @dtAtualizacaoConcentradorFim AS DATETIME
     
SET @cdSistema = 1
SET @idBandeira = 40

SET @dtRecebimentoInicio = '11/01/2015 02:00:00'
SET @dtRecebimentoFim = '12/12/2015 02:00:00'
*/
 
SELECT NF.IDNotaFiscal
     , NF.IDConcentrador
     , NF.IDLoja
     , NF.IDBandeira
     , NF.IDFornecedor
     , NF.nrNotaFiscal
     , NF.srNotaFiscal
     , NF.dtEmissao
     , NF.dtRecebimento
     , NF.dtCadastroLivro
     , NF.dtCadastroConcentrador
     , NF.dtAtualizacaoConcentrador
     , NF.dtInclusaoHistorico
     , NF.dtAlteracaoHistorico
     , (SELECT COUNT(1) FROM NotaFiscalItem NFI WHERE NFI.IDNotaFiscal = NF.IDNotaFiscal AND NFI.IdNotaFiscalItemStatus = 3) AS blDivergente
     , NF.dtLiberacao
     , NF.IDTipoNota
     , NF.Visivel
     , NF.tpOperacao
     , NF.cdCfop
     , NF.DhCriacao
     , NULL AS SplitOn1
	 , L.cdLoja
	 , L.nmLoja
     , NULL AS SplitOn2
	 , F.cdFornecedor
	 , F.nmFornecedor
  FROM NotaFiscal NF WITH (NOLOCK)
  JOIN Loja L WITH (NOLOCK) ON NF.IDLoja = L.IDLoja AND L.cdLoja = ISNULL(@cdLoja, L.cdLoja)
  JOIN Fornecedor F WITH (NOLOCK) ON NF.IDFornecedor = F.IDFornecedor AND F.cdFornecedor = ISNULL(@cdFornecedor, F.cdFornecedor)
WHERE NF.IDBandeira = ISNULL(@idBandeira, NF.IDBandeira)
   AND NF.nrNotaFiscal = ISNULL(@nrNotaFiscal, NF.nrNotaFiscal)  
   AND NF.dtRecebimento >= @dtRecebimentoInicio AND NF.dtRecebimento < @dtRecebimentoFim   
   AND NF.dtCadastroConcentrador >= ISNULL(@dtCadastroConcentradorInicio, NF.dtCadastroConcentrador) AND (@dtCadastroConcentradorFim IS NULL OR NF.dtCadastroConcentrador < @dtCadastroConcentradorFim)
   AND NF.dtAtualizacaoConcentrador >= ISNULL(@dtAtualizacaoConcentradorInicio, NF.dtAtualizacaoConcentrador) AND (@dtAtualizacaoConcentradorFim IS NULL OR NF.dtAtualizacaoConcentrador < @dtAtualizacaoConcentradorFim)
   AND EXISTS (SELECT 1 FROM NotaFiscalItem NFI WITH (NOLOCK)
                        JOIN ItemDetalhe ID WITH (NOLOCK) ON NFI.IDItemDetalhe = ID.IDItemDetalhe
                         AND ID.cdSistema = ISNULL(@cdSistema, ID.cdSistema)      
                         AND ID.cdItem = ISNULL(@cdItem, ID.cdItem)
					   WHERE NFI.IDNotaFiscal = NF.IDNotaFiscal)