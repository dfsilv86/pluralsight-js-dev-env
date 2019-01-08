/*
DECLARE @idNotaFiscal AS INT
     
SET @idNotaFiscal = 14509039
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
     , NF.blDivergente
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
	 , NULL AS SplitOn3
	 , B.dsBandeira
	 , B.sgBandeira
  FROM NotaFiscal NF WITH (NOLOCK)
  JOIN Loja L WITH (NOLOCK) ON NF.IDLoja = L.IDLoja 
  JOIN Fornecedor F WITH (NOLOCK) ON NF.IDFornecedor = F.IDFornecedor 
  JOIN Bandeira B WITH (NOLOCK) ON NF.IDBandeira = B.IDBandeira  
 WHERE NF.IDNotaFiscal = @idNotaFiscal