using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface para serviço de cadastro de item detalhe.
    /// </summary>
    public interface IItemDetalheService : IDomainService<ItemDetalhe>
    {
        /// <summary>
        /// Obtém um item pelo seu id, com informações básicas da estrutura mercadológica e fornecedor.
        /// </summary>
        /// <param name="id">O id do item.</param>
        /// <returns>O ItemDetalhe com informações básicas da estrutura mercadológica e fornecedor.</returns>
        ItemDetalhe ObterEstruturadoPorId(int id);

        /// <summary>
        /// Obtém o <see cref="Walmart.Sgp.Domain.Gerenciamento.FornecedorParametro">FornecedorParametro</see>
        /// relacionado ao item.
        /// </summary>
        /// <param name="idItem">O id do item.</param>
        /// <returns>O <see cref="Walmart.Sgp.Domain.Gerenciamento.FornecedorParametro">FornecedorParametro</see>
        /// relacionado ao item ou <c>null</c> caso não existam associações.
        /// </returns>
        FornecedorParametro ObterFornecedorParametro(int idItem);

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O detalhe do item.</returns>
        ItemDetalhe ObterPorItemESistema(long cdItem, byte cdSistema);

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <returns>O detalhe do item.</returns>
        ItemDetalhe ObterPorOldNumberESistemaEDepartamento(long cdItem, byte cdSistema, int cdDepartamento);

        /// <summary>
        /// Obtém um item pelo seu código plu e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdPLU">O código plu do item.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O detalhe do item.</returns>
        ItemDetalhe ObterPorPluESistema(long cdPLU, byte cdSistema);

        /// <summary>
        /// Obtém os itens de entrada com base no item de saida.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdItemSaida">Codigo do item de saida</param>
        /// <param name="idFornecedorParametro">codigo do fornecedor do item de entrada</param>
        /// <returns>O detalhe do item.</returns>
        IEnumerable<ItemDetalhe> ObterItemEntradaPorItemSaida(byte cdSistema, long cdItemSaida, int? idFornecedorParametro);

        /// <summary>
        /// Obtém um item vinculado de saida pelo seu código, código da estrutura mercadológica e códgio do fornecedor.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="idFornecedorParametro">O código do fornecedor.</param>
        /// <param name="idRegiaoCompra">ID da regiao.</param>
        /// <param name="tpStatus">O Status do item.</param>
        /// <param name="blPerecivel">Filtrar somente pereciveis.</param>
        /// <returns>O detalhe do item.</returns>
        ItemDetalhe ObterItemSaidaPorFornecedorItemEntrada(long cdItem, byte cdSistema, int? idFornecedorParametro, int? idRegiaoCompra, string tpStatus, string blPerecivel);

        /// <summary>
        /// Pesquisa detalhe de itens saida pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Filtro do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        IEnumerable<ItemDetalhe> ObterListaItemSaidaPorFornecedorItemEntrada(ItemDetalheFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém as colunas CdItem, DsItem, TpStatus e vlTipoReabastecimento de um item pelo seu código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retornar o item detalhe.</returns>
        ItemDetalhe ObterPorCodigoESistemaComProjecao(long cdItem, int cdSistema);

        /// <summary>
        /// Pesquisa detalhe de itens pelos filtros informados, código do usuário e tipos de reabastecimento.
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
        IEnumerable<ItemDetalhe> PesquisarPorFiltroTipoReabastecimento(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, int? idRegiaoCompra, Paging paging);

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
        /// <param name="tpVinculado">O vinculo do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        IEnumerable<ItemDetalhe> PesquisarPorFiltro(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, string tpVinculado, Paging paging);

        /// <summary>
        /// Pesquisa Itens de Saida e retorna eles com seus CDs. Filtrando somente CDs convertidos.
        /// </summary>
        /// <param name="cdItem">O codigo do item de saída (cdItem).</param>
        /// <param name="cdDepartamento">O codigo do departamento do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema do item de saida.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="filtroMS">Incluir itens sem multisourcing possivel.</param>
        /// <param name="filtroCadastro">Incluir itens que possuem cadastro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de saida com seu devido CD.</returns>
        IEnumerable<ItemDetalheCD> PesquisarItensSaidaComCDConvertido(long? cdItem, int? cdDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro, Paging paging);

        /// <summary>
        /// Altera o tipo do vinculado do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoVinculado">O novo valor para tipo de vinculado.</param>
        void AlterarVinculado(int idItemDetalhe, TipoVinculado tipoVinculado);

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de entrada com informações de CD e multisourcing.</returns>
        IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema, Paging paging);

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Os itens de entrada com informação de CD e multisourcing.</returns>
        IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema);

        /// <summary>
        /// Verifica se o item detalhe pertence ao fornecedor.
        /// </summary>
        /// <param name="cdItem">O código do item detalhe.</param>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true caso o item detalhe pertença ao fornecedor, do contrário retorna false.</returns>
        bool ItemDetalhePertenceFornecedor(long cdItem, long cdV9D, int cdSistema);

        /// <summary>
        /// Verifica se o item detalhe de entrada pertence ao item detalhe de saída.
        /// </summary>
        /// <param name="cdItemEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdItemSaida">O código do item detalhe de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna IDRelacionamentoItemSecundario caso item detalhe entrada pertença ao item detalhe saída, do contrário retorna default(long).</returns>
        long ItemDetalheEntradaPertenceItemDetalheSaida(long cdItemEntrada, long cdItemSaida, int cdSistema);

        /// <summary>
        /// Altera o tipo do manipulado do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoManipulado">O novo valor para tipo de manipulado.</param>
        void AlterarManipulado(int idItemDetalhe, TipoManipulado tipoManipulado);

        /// <summary>
        /// Altera o tipo do receituário do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoReceituario">O novo valor para tipo de receituário.</param>
        void AlterarReceituario(int idItemDetalhe, TipoReceituario tipoReceituario);

        /// <summary>
        /// Altera as informações de unidade de medida e fator conversão, informados na tela de custos do item.
        /// </summary>
        /// <param name="itemDetalhe">O ItemDetalhe.</param>
        /// <remarks>Modifica apenas os campos tpUnidadeMedida e vlFatorConversao.</remarks>
        void AlterarDadosCustos(ItemDetalhe itemDetalhe);

        /// <summary>
        /// Verifica se o item detalhe saída pertence ao CD.
        /// </summary>
        /// <param name="cdItemSaida">O código do item detalhe saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true caso o item detalhe saída pertença ao CD, caso contrário retorna false.</returns>
        bool ItemDetalheSaidaPertenceCD(long cdItemSaida, int cdCD, int cdSistema);

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
        IEnumerable<ItemDetalhe> ObterItensDetalheReturnSheet(int relacionamentoSGP, int cdDepartamento, int? cdItemDetalhe, long? cdV9D, int? idRegiaoCompra, int cdSistema, int idReturnSheet, Paging paging);

        /// <summary>
        /// Pesquisa detalhe de itens cruzando com as bandeiras às quais os itens estão associados através de Traits, e filtrando pela permissão do usuário.
        /// </summary>
        /// <param name="filtro">The filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfazem o filtro.</returns>
        IEnumerable<ResultadoConsultaItem> PesquisarAbertoPorBandeira(ItemFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém as informações de estoque por loja.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As informações de estoque por loja.</returns>
        IEnumerable<ResultadoConsultaItemPorLoja> ObterInformacoesEstoquePorLoja(int cdItem, int idBandeira, int? idLoja, Paging paging);

        /// <summary>
        /// Obtém as informações cadastrais do item.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>As informações cadastrais do item.</returns>
        Tuple<ItemDetalhe, Bandeira> ObterInformacoesCadastrais(int cdItem, int idBandeira);

        /// <summary>
        /// Altera as informações cadastrais e agenda o reprocessamento de custo caso o item receba nota.
        /// </summary>
        /// <param name="itemDetalhe">O item detalhe.</param>
        void AlterarInformacoesCadastrais(ItemDetalhe itemDetalhe);

        /// <summary>
        /// Obtém os custos do item na bandeira e loja (opcional) informados.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os custos do item.</returns>
        IEnumerable<ItemCusto> ObterItemCustos(int cdItem, int idBandeira, int? idLoja, Paging paging);

        /// <summary>
        /// Obtém o valor do tipo de reabastecimento.
        /// </summary>
        /// <param name="idItemDetalheEntrada">O id do item detalhe de entrada.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>Retorna o valor do tipo de reabastecimento.</returns>
        ValorTipoReabastecimento ObterValorTipoReabastecimento(long idItemDetalheEntrada, int idCD);

        /// <summary>
        /// Obtém as Lojas do item.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Lojas do item detalhe.</returns>
        IEnumerable<Loja> ObterTraitsPorItem(int idItemDetalhe, int cdSistema, Paging paging);

        /// <summary>
        /// Verifica se o item detalhe de entrada está vinculado a uma compra casada.
        /// </summary>
        /// <param name="cdItemDetalheEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true se o item estiver vinculado a compra casada, do contrário retorna false.</returns>
        bool EstaVinculadoCompraCasada(long cdItemDetalheEntrada, long cdSistema);
    }
}
