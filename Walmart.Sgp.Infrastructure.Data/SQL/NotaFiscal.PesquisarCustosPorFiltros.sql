/*
DECLARE 
	@idBandeira AS INT, 
	@dtRecebimentoInicio AS DATETIME, 
	@dtRecebimentoFim AS DATETIME,
	@idNotaFiscalItemStatus AS INT,
	@idLoja AS INT,
	@idFornecedor AS INT,
	@nrNotaFiscal AS INT,
	@idDepartamento AS INT,
	@idItemDetalhe AS INT,
	@dtCadastroConcentradorInicio AS DATETIME,
	@dtCadastroConcentradorFim AS DATETIME,
	@dtAtualizacaoConcentradorInicio AS DATETIME,
	@dtAtualizacaoConcentradorFim AS DATETIME

SET @idBandeira = 1	
SET @dtRecebimentoInicio = '2016-03-27'
SET @dtRecebimentoFim = '2016-03-27'
*/

SELECT IDNotaFiscalItem
	 , cdLoja
	 , dtRecebimento
	 , dtEmissao
	 , nrNotaFiscal
	 , cdDepartamento
	 , cddsItem
	 , dhLiberacao
	 , vlCusto
	 , vlCustoCompraAtual
	 , VariacaoUltimoCusto
	 , qtItem
	 , qtItemCorrigida
	 , vlMercadoria
	 , QtVendorPackage
 	 , DsTamanhoItem
	 , vlCustoUnitarioReal
	 , VlCustoUnitario
	 , dhCriacao
	 , dtLiberacao
	 , usrNomeAlteracao
	 , IdNotaFiscalItemStatus
	 , IDBandeira           
	 , IDFornecedor
	 , IDLoja
	 , IDItemDetalhe
	 , IDDepartamento
	 , dtCadastroConcentrador
	 , dtAtualizacaoConcentrador
     , qtItem AS qtAjustada
  FROM VW_ConsultaAlteracaoCusto
 WHERE IDBandeira = @idBandeira
   AND dtRecebimento >= @dtRecebimentoInicio
   AND dtRecebimento < @dtRecebimentoFim
   AND IdNotaFiscalItemStatus = ISNULL(@idNotaFiscalItemStatus, IdNotaFiscalItemStatus)
   AND IDLoja = ISNULL(@idLoja, IDLoja)  
   AND IDFornecedor = ISNULL(@idFornecedor, IDFornecedor)  
   AND nrNotaFiscal = ISNULL(@nrNotaFiscal, nrNotaFiscal)  
   AND IDItemDetalhe = ISNULL(@idItemDetalhe, IDItemDetalhe)  
   AND IDDepartamento = ISNULL(@idDepartamento, IDDepartamento)
   AND dtCadastroConcentrador >= ISNULL(@dtCadastroConcentradorInicio, dtCadastroConcentrador) 
   AND dtCadastroConcentrador < ISNULL(@dtCadastroConcentradorFim, dtCadastroConcentrador + 1)
   AND dtAtualizacaoConcentrador >= ISNULL(@dtAtualizacaoConcentradorInicio, dtAtualizacaoConcentrador) 
   AND dtAtualizacaoConcentrador < ISNULL(@dtAtualizacaoConcentradorFim, dtAtualizacaoConcentrador + 1)