using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para item detalheem memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryItemDetalheGateway : MemoryDataGateway<ItemDetalhe>, IItemDetalheGateway
    {
        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O detalhe do item.</returns>
        public ItemDetalhe ObterPorItemESistema(long cdItem, byte cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um item pelo seu código plu e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdPLU">O código plu do item.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>
        /// O detalhe do item.
        /// </returns>        
        public ItemDetalhe ObterPorPluESistema(long cdPLU, byte cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa detalhe de itens pelos filtros informados e pelo código do usuário.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdPLU">O código Price Look Up.</param>
        /// <param name="dsItem">A descrição do item.</param>
        /// <param name="tpStatus">O status do item.</param>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idUsuario">The identifier usuario.</param>
        /// <param name="tpVinculado">O tipo.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> PesquisarPorFiltro(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, string tpVinculado, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Altera as informações de unidade de medida e fator conversão, informados na tela de custos do item.
        /// </summary>
        /// <param name="itemDetalhe">O ItemDetalhe.</param>
        /// <remarks>Modifica apenas os campos tpUnidadeMedida e vlFatorConversao.</remarks>
        public void AlterarDadosCustos(ItemDetalhe itemDetalhe)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o <see cref="Walmart.Sgp.Domain.Gerenciamento.FornecedorParametro">FornecedorParametro</see>
        /// relacionado ao item.
        /// </summary>
        /// <param name="idItem">O id do item.</param>
        /// <returns>O <see cref="Walmart.Sgp.Domain.Gerenciamento.FornecedorParametro">FornecedorParametro</see>
        /// relacionado ao item ou <c>null</c> caso não existam associações.
        /// </returns>
        public FornecedorParametro ObterFornecedorParametro(int idItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa detalhe de itens pelos filtros informados, código do usuário e tipo de reabastecimento.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdPLU">O código Price Look Up.</param>
        /// <param name="dsItem">A descrição do item.</param>
        /// <param name="tpStatus">O status do item.</param>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idUsuario">The identifier usuario.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> PesquisarPorFiltroTipoReabastecimento(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, int? idRegiaoCompra, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades associadas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>O ItemDetalhe com informações de Fornecedor, FineLine, Subcategoria, Categoria, Departamento e Divisao.</returns>
        public ItemDetalhe ObterEstruturadoPorId(int idItemDetalhe)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Busca itens de acordo com o filtro especificado.
        /// </summary>
        /// <param name="filtro">The filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os registros que satisfazem o filtro.
        /// </returns>        
        public IEnumerable<ResultadoConsultaItem> PesquisarAbertoPorBandeira(ItemFiltro filtro, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtem as informações de estoque por loja.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// As informações de estoque por loja.
        /// </returns>        
        public IEnumerable<ResultadoConsultaItemPorLoja> ObterInformacoesEstoquePorLoja(int cdItem, int idBandeira, int? idLoja, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém as informações cadastrais do item.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>
        /// As informações cadastrais do item.
        /// </returns>        
        public Tuple<ItemDetalhe, Bandeira> ObterInformacoesCadastrais(int cdItem, int idBandeira)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="idFornecedorParametro">Codigo do Fornecedor.</param>
        /// <param name="idRegiaoCompra">O id da região compra.</param>
        /// <param name="tpStatus">O status do item</param>
        /// <param name="blPerecivel">Filtrar somente pereciveis.</param>
        /// <returns>O detalhe do item.</returns>
        public ItemDetalhe ObterItemSaidaPorFornecedorItemEntrada(long cdItem, byte cdSistema, int? idFornecedorParametro, int? idRegiaoCompra, string tpStatus, string blPerecivel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa detalhe de itens pelos filtros informados, código do usuário e tipos de reabastecimento.
        /// </summary>
        /// <param name="filtro">O código do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> ObterListaItemSaidaPorFornecedorItemEntrada(ItemDetalheFiltro filtro, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os itens de entrada com base no item de saida.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdItemSaida">Codigo do item de saida</param>
        /// <param name="idFornecedorParametro">codigo do fornecedor do item de entrada</param>
        /// <returns>O detalhe do item.</returns>
        public IEnumerable<ItemDetalhe> ObterItemEntradaPorItemSaida(byte cdSistema, long cdItemSaida, int? idFornecedorParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades RegiaoCompra e AreaCD
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>O ItemDetalhe com informações de RegiaoCompra e AreaCD.</returns>
        public ItemDetalhe ObterEstruturadoExtraPorId(int idItemDetalhe)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém as colunas CdItem, DsItem, TpStatus e vlTipoReabastecimento de um item pelo seu código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retornar o item detalhe.</returns>
        public ItemDetalhe ObterPorCodigoESistemaComProjecao(long cdItem, int cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisar ItemDetalheCD
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdDepartamento">O cd do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="filtroMS">Incluir itens sem multisourcing possivel.</param>
        /// <param name="filtroCadastro">Incluir itens que possuem cadastro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de saida com informação de CD e multisourcing.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItemCD(long? cdItem, int? cdDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="paging">Parametro Paging.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna a quantidade de itens de entrada para um determinado item de saída e seu CD.
        /// </summary>
        /// <param name="cdItem">O código (cdItem) do item de saída.</param>
        /// <param name="cdCD">O código (cdCD) do CD a ser pesquisado.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A quantidade de itens de entrada para um determinado item de venda.</returns>
        public int ObterQuantidadeItensEntrada(long cdItem, long cdCD, long cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o item detalhe pertence ao fornecedor.
        /// </summary>
        /// <param name="cdItem">O código do item detalhe.</param>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true caso o item detalhe pertença ao fornecedor, do contrário retorna false.</returns>
        public bool ItemDetalhePertenceFornecedor(long cdItem, long cdV9D, int cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o item detalhe de entrada pertence ao item detalhe de saída.
        /// </summary>
        /// <param name="cdItemEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdItemSaida">O código do item detalhe de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna IDRelacionamentoItemSecundario caso item detalhe entrada pertença ao item detalhe saída, do contrário retorna default(long).</returns>
        public long ItemDetalheEntradaPertenceItemDetalheSaida(long cdItemEntrada, long cdItemSaida, int cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o item detalhe saída pertence ao CD.
        /// </summary>
        /// <param name="cdItemSaida">O código do item detalhe saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true caso o item detalhe saída pertença ao CD, caso contrário retorna false.</returns>
        public bool ItemDetalheSaidaPertenceCD(long cdItemSaida, int cdCD, int cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa dos itens de controle de estoque válidos para a inclusão em uma return sheet.
        /// </summary>
        /// <param name="relacionamentoSGP">O valor indicando se possui relacionamento SGP (0 - Não, 1 - Sim, 2 - Todos).</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdItemDetalhe">O código do item.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de controle de estoque válidos para inclusão em uma return sheet.</returns>
        public IEnumerable<ItemDetalhe> ObterItensDetalheReturnSheet(int relacionamentoSGP, int cdDepartamento, int? cdItemDetalhe, long? cdV9D, int? idRegiaoCompra, int cdSistema, int idReturnSheet, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os custos do item na bandeira e loja (opcional) informados.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os custos do item.</returns>
        public IEnumerable<ItemCusto> ObterItemCustos(int cdItem, int idBandeira, int? idLoja, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o valor do tipo de reabastecimento.
        /// </summary>
        /// <param name="idItemDetalheEntrada">O id do item detalhe de entrada.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>Retorna o valor do tipo de reabastecimento.</returns>
        public ValorTipoReabastecimento ObterValorTipoReabastecimento(long idItemDetalheEntrada, int idCD)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <returns>O detalhe do item.</returns>
        public ItemDetalhe ObterPorOldNumberESistemaEDepartamento(long cdItem, byte cdSistema, int cdDepartamento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém as Lojas do item.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Lojas do item detalhe.</returns>
        public IEnumerable<Loja> ObterTraitsPorItem(int idItemDetalhe, int cdSistema, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o item detalhe de entrada está vinculado a uma compra casada.
        /// </summary>
        /// <param name="cdItemDetalheEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true se o item estiver vinculado a compra casada, do contrário retorna false.</returns>
        public bool EstaVinculadoCompraCasada(long cdItemDetalheEntrada, long cdSistema)
        {
            throw new NotImplementedException();
        }
    }
}