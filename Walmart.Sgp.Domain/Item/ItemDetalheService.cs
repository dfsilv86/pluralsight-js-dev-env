using System;
using System.Collections.Generic;
using System.Linq;

using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Serviço de cadastro de item detalhe.
    /// </summary>
    public class ItemDetalheService : EntityDomainServiceBase<ItemDetalhe, IItemDetalheGateway>, IItemDetalheService
    {
        #region Fields
        private readonly IItemDetalheHistGateway m_histGateway;
        private readonly IItemRelacionamentoGateway m_itemRelacionamentoGateway;
        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDetalheService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        /// <param name="histGateway">O gateway de histórico de item detalhe.</param>
        /// <param name="itemRelacionamentoGateway">O gateway de item relacionamento.</param>
        public ItemDetalheService(IItemDetalheGateway mainGateway, IItemDetalheHistGateway histGateway, IItemRelacionamentoGateway itemRelacionamentoGateway)
            : base(mainGateway)
        {
            m_histGateway = histGateway;
            m_itemRelacionamentoGateway = itemRelacionamentoGateway;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Obtém um item pelo seu id, com informações básicas da estrutura mercadológica e fornecedor.
        /// </summary>
        /// <param name="id">O id do item.</param>
        /// <returns>O ItemDetalhe com informações básicas da estrutura mercadológica e fornecedor.</returns>
        public ItemDetalhe ObterEstruturadoPorId(int id)
        {
            var itemDetalhe = this.MainGateway.ObterEstruturadoPorId(id);
            if (null != itemDetalhe)
            {
                itemDetalhe.FornecedorParametro = this.ObterFornecedorParametro(id);
            }

            return MesclarItemDetalhe(itemDetalhe, this.ObterEstruturadoExtraPorId(id));
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades RegiaoCompra e AreaCD
        /// </summary>
        /// <param name="id">O id de item detalhe.</param>
        /// <returns>O ItemDetalhe com informações de RegiaoCompra e AreaCD.</returns>
        public ItemDetalhe ObterEstruturadoExtraPorId(int id)
        {
            return this.MainGateway.ObterEstruturadoExtraPorId(id);
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
            return this.MainGateway.ObterFornecedorParametro(idItem);
        }

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O detalhe do item.</returns>
        public ItemDetalhe ObterPorItemESistema(long cdItem, byte cdSistema)
        {
            Assert(new { ItemCode = cdItem, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorItemESistema(cdItem, cdSistema);
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
            Assert(new { ItemCode = cdItem, MarketingStructure = cdSistema, IdDepartamento = cdDepartamento }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorOldNumberESistemaEDepartamento(cdItem, cdSistema, cdDepartamento);
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
            Assert(new { Plu = cdPLU, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorPluESistema(cdPLU, cdSistema);
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
            Assert(new { ItemCode = cdItemSaida, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterItemEntradaPorItemSaida(cdSistema, cdItemSaida, idFornecedorParametro);
        }

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
        public ItemDetalhe ObterItemSaidaPorFornecedorItemEntrada(long cdItem, byte cdSistema, int? idFornecedorParametro, int? idRegiaoCompra, string tpStatus, string blPerecivel)
        {
            Assert(new { ItemCode = cdItem, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterItemSaidaPorFornecedorItemEntrada(cdItem, cdSistema, idFornecedorParametro, idRegiaoCompra, tpStatus, blPerecivel);
        }

        /// <summary>
        /// Pesquisa detalhe de itens saida pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O código do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> ObterListaItemSaidaPorFornecedorItemEntrada(ItemDetalheFiltro filtro, Paging paging)
        {
            Assert(new { MarketingStructure = filtro.CdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterListaItemSaidaPorFornecedorItemEntrada(filtro, paging);
        }

        /// <summary>
        /// Obtém as colunas CdItem, DsItem e TpStatus de um item pelo seu código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retornar o item detalhe.</returns>
        public ItemDetalhe ObterPorCodigoESistemaComProjecao(long cdItem, int cdSistema)
        {
            return this.MainGateway.ObterPorCodigoESistemaComProjecao(cdItem, cdSistema);
        }

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
        public IEnumerable<ItemDetalhe> PesquisarPorFiltroTipoReabastecimento(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, int? idRegiaoCompra, Paging paging)
        {
            Assert(new { User = idUsuario, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorFiltroTipoReabastecimento(cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, idRegiaoCompra, paging);
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
        /// <param name="idUsuario">O id do usuario.</param>
        /// <param name="tpVinculado">O vinculo do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> PesquisarPorFiltro(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, string tpVinculado, Paging paging)
        {
            Assert(new { User = idUsuario, MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorFiltro(cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, tpVinculado, paging);
        }

        /// <summary>
        /// Altera o tipo do vinculado do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoVinculado">O novo valor para tipo de vinculado.</param>
        public void AlterarVinculado(int idItemDetalhe, TipoVinculado tipoVinculado)
        {
            MainGateway.Update("TpVinculado = @tipoVinculado", "IDItemDetalhe = @idItemDetalhe", new { idItemDetalhe, tipoVinculado });

            var entidade = ObterPorId(idItemDetalhe);
            entidade.TpVinculado = tipoVinculado;
            m_histGateway.Insert(ItemDetalheHist.Create(entidade));
        }

        /// <summary>
        /// Altera o tipo do manipulado do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoManipulado">O novo valor para tipo de manipulado.</param>
        public void AlterarManipulado(int idItemDetalhe, TipoManipulado tipoManipulado)
        {
            MainGateway.Update("TpManipulado = @tipoManipulado", "IDItemDetalhe = @idItemDetalhe", new { idItemDetalhe, tipoManipulado });

            var entidade = ObterPorId(idItemDetalhe);
            entidade.TpManipulado = tipoManipulado;
            m_histGateway.Insert(ItemDetalheHist.Create(entidade));
        }

        /// <summary>
        /// Altera o tipo do receituário do item detalhe informado.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="tipoReceituario">O novo valor para tipo de receituário.</param>
        public void AlterarReceituario(int idItemDetalhe, TipoReceituario tipoReceituario)
        {
            MainGateway.Update("TpReceituario = @tipoReceituario", "IDItemDetalhe = @idItemDetalhe", new { idItemDetalhe, tipoReceituario });

            var entidade = ObterPorId(idItemDetalhe);
            entidade.TpReceituario = tipoReceituario;
            m_histGateway.Insert(ItemDetalheHist.Create(entidade));
        }

        /// <summary>
        /// Altera as informações de unidade de medida e fator conversão, informados na tela de custos do item.
        /// </summary>
        /// <param name="itemDetalhe">O ItemDetalhe.</param>
        /// <remarks>Modifica apenas os campos tpUnidadeMedida e vlFatorConversao.</remarks>
        public void AlterarDadosCustos(ItemDetalhe itemDetalhe)
        {
            Assert(new { itemDetalhe, idItemDetalhe = itemDetalhe.IDItemDetalhe, itemDetalhe.VlFatorConversao, TpUnidadeMedida = itemDetalhe.TpUnidadeMedida.Value }, new AllMustBeInformedSpec());

            this.MainGateway.AlterarDadosCustos(itemDetalhe);

            //// Code-review: todo o código feito para implementar a linha acima (interface de método no gateway, implementação do método no gateway e arquivo .sql), 
            //// poderia ser substituído pelo comando abaixo.
            ////this.MainGateway.Update(
            ////    "tpUnidadeMedida = @tpUnidadeMedida, vlFatorConversao = @vlFatorConversao",
            ////    "idItemDetalhe = @idItemDetalhe AND cdSistema = @cdSistema",
            ////    itemDetalhe);
        }

        /// <summary>
        /// Pesquisa Itens de Saida e retorna eles com seus CDs. Filtrando somente CDs convertidos.
        /// </summary>
        /// <param name="cdItem">O codigo do item de saída (cdItem)</param>
        /// <param name="cdDepartamento">O codigo do departamento do item de saida</param>
        /// <param name="cdSistema">O codigo do sistema do item de saida</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="filtroMS">Incluir itens sem multisourcing possivel.</param>
        /// <param name="filtroCadastro">Incluir itens que possuem cadastro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de saida com seu devido CD e multisourcing.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItensSaidaComCDConvertido(long? cdItem, int? cdDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro, Paging paging)
        {
            Assert(new { Department = cdDepartamento, cdItem = cdItem }, new AtLeastOneMustBeInformedSpec());

            return this.MainGateway.PesquisarItemCD(cdItem, cdDepartamento, cdSistema, idCD, filtroMS, filtroCadastro, paging);
        }

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de entrada com informação de CD e multisourcing.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema, Paging paging)
        {
            Assert(new { cd = cdCD, cdItem = cdItem, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            Assert(new Tuple<long, long, long>(cdItem, cdCD, cdSistema), new ItemDeveSerMultisourcingPossivelSpec(this.MainGateway.ObterQuantidadeItensEntrada));

            return this.MainGateway.PesquisarItensEntradaPorSaidaCD(cdItem, cdCD, cdSistema, paging);
        }

        /// <summary>
        /// Pesquisar Itens de entrada por item de saida e CD.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Os itens de entrada com informação de CD e multisourcing.</returns>
        public IEnumerable<ItemDetalheCD> PesquisarItensEntradaPorSaidaCD(long cdItem, long cdCD, long cdSistema)
        {
            Assert(new { cd = cdCD, cdItem = cdItem, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarItensEntradaPorSaidaCD(cdItem, cdCD, cdSistema);
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
            Assert(new { cdItem = cdItem, cdV9D = cdV9D, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ItemDetalhePertenceFornecedor(cdItem, cdV9D, cdSistema);
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
            Assert(new { InputItemCode = cdItemEntrada, OutputItemCode = cdItemSaida, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ItemDetalheEntradaPertenceItemDetalheSaida(cdItemEntrada, cdItemSaida, cdSistema);
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
            Assert(new { OutputItemCode = cdItemSaida, cd = cdCD, cdSistema = cdSistema });

            return this.MainGateway.ItemDetalheSaidaPertenceCD(cdItemSaida, cdCD, cdSistema);
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
            var args = new
            {
                cdDepartamento,
                cdSistema,
                idReturnSheet
            };

            Assert(args, new AllMustBeInformedSpec());

            return this.MainGateway.ObterItensDetalheReturnSheet(relacionamentoSGP, cdDepartamento, cdItemDetalhe, cdV9D, idRegiaoCompra, cdSistema, idReturnSheet, paging);
        }

        /// <summary>
        /// Altera as informações cadastrais e agenda o reprocessamento de custo caso o item receba nota.
        /// </summary>
        /// <param name="itemDetalhe">O item detalhe.</param>
        public void AlterarInformacoesCadastrais(ItemDetalhe itemDetalhe)
        {
            var itemOriginal = MainGateway.FindById(itemDetalhe.IDItemDetalhe);
            itemOriginal.VlFatorConversao = itemDetalhe.VlFatorConversao;
            itemOriginal.TpUnidadeMedida = itemDetalhe.TpUnidadeMedida;
            itemOriginal.BlItemTransferencia = itemDetalhe.BlItemTransferencia;

            Assert(itemOriginal, new Specs.InformacoesDeEstoquePodemSerAtualizadasSpec());
            this.MainGateway.Update(
                "tpUnidadeMedida = @tpUnidadeMedida, vlFatorConversao = @vlFatorConversao, blItemTransferencia = @BlItemTransferencia",
                "idItemDetalhe = @idItemDetalhe",
                itemOriginal);

            m_histGateway.Insert(ItemDetalheHist.Create(itemOriginal));

            if (itemOriginal.RecebeNota)
            {
                AgendarReprocessamentoCust(itemOriginal);
            }
        }

        /// <summary>
        /// Busca itens de acordo com o filtro especificado.
        /// </summary>
        /// <param name="filtro">The filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfazem o filtro.</returns>
        public IEnumerable<ResultadoConsultaItem> PesquisarAbertoPorBandeira(ItemFiltro filtro, Paging paging)
        {
            return MainGateway.PesquisarAbertoPorBandeira(filtro, paging);
        }

        /// <summary>
        /// Obtem as informações de estoque por loja.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As informações de estoque por loja.</returns>
        public IEnumerable<ResultadoConsultaItemPorLoja> ObterInformacoesEstoquePorLoja(int cdItem, int idBandeira, int? idLoja, Paging paging)
        {
            return MainGateway.ObterInformacoesEstoquePorLoja(cdItem, idBandeira, idLoja, paging);
        }

        /// <summary>
        /// Obtém as informações cadastrais do item.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>As informações cadastrais do item.</returns>
        public Tuple<ItemDetalhe, Bandeira> ObterInformacoesCadastrais(int cdItem, int idBandeira)
        {
            return MainGateway.ObterInformacoesCadastrais(cdItem, idBandeira);
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
            return MainGateway.ObterItemCustos(cdItem, idBandeira, idLoja, paging);
        }

        /// <summary>
        /// Obtém o valor do tipo de reabastecimento.
        /// </summary>
        /// <param name="idItemDetalheEntrada">O id do item detalhe de entrada.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>Retorna o valor do tipo de reabastecimento.</returns>
        public ValorTipoReabastecimento ObterValorTipoReabastecimento(long idItemDetalheEntrada, int idCD)
        {
            return this.MainGateway.ObterValorTipoReabastecimento(idItemDetalheEntrada, idCD);
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
            return this.MainGateway.ObterTraitsPorItem(idItemDetalhe, cdSistema, paging);
        }

        /// <summary>
        /// Verifica se o item detalhe de entrada está vinculado a uma compra casada.
        /// </summary>
        /// <param name="cdItemDetalheEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true se o item estiver vinculado a compra casada, do contrário retorna false.</returns>
        public bool EstaVinculadoCompraCasada(long cdItemDetalheEntrada, long cdSistema)
        {
            Assert(new { InputItemCode = cdItemDetalheEntrada, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.EstaVinculadoCompraCasada(cdItemDetalheEntrada, cdSistema);
        }

        /// <summary>
        /// Mescla as informações basicas e adicionais de duas entidades ItemDetalhe
        /// </summary>
        /// <param name="itemDetalheEstruturado">Entidade ItemDetalhe resultante do método ObterEstruturadoPorId</param>
        /// <param name="itemDetalheExtra">Entidade ItemDetalhe resultante do método ObterEstruturadoExtraPorId</param>
        /// <returns>O objeto itemDetalheEstruturado com as informações do objeto itemDetalheExtra</returns>
        private static ItemDetalhe MesclarItemDetalhe(ItemDetalhe itemDetalheEstruturado, ItemDetalhe itemDetalheExtra)
        {
            if (itemDetalheEstruturado != null && itemDetalheExtra != null)
            {
                itemDetalheEstruturado.AreaCD = itemDetalheExtra.AreaCD;
                itemDetalheEstruturado.RegiaoCompra = itemDetalheExtra.RegiaoCompra;
            }

            return itemDetalheEstruturado;
        }

        /// <summary>
        /// Agenda o processamento de custo para o item.
        /// </summary>
        /// <param name="itemDetalhe">O item detalhe.</param>
        private void AgendarReprocessamentoCust(ItemDetalhe itemDetalhe)
        {
            var relacionamentosPrincipal = m_itemRelacionamentoGateway.ObterPrincipaisPorItem(itemDetalhe.IDItemDetalhe);

            foreach (var relacionamento in relacionamentosPrincipal)
            {
                relacionamento.StatusReprocessamentoCusto = StatusReprocessamentoCusto.Agendado;
                relacionamento.BlReprocessamentoManual = false; // reprocessamento não manual para o serviço poder reprocessar.
                relacionamento.DescErroReprocessamento = string.Empty;
                relacionamento.DtInicioReprocessamentoCusto = null;
                relacionamento.DtFinalReprocessamentoCusto = null;
                relacionamento.IdUsuarioAlteracao = RuntimeContext.Current.User.Id;

                m_itemRelacionamentoGateway.Update(
                    @"StatusReprocessamentoCusto = @StatusReprocessamentoCusto,
                    BlReprocessamentoManual = @BlReprocessamentoManual,
                    DescErroReprocessamento = @DescErroReprocessamento,
                    DtInicioReprocessamentoCusto = @DtInicioReprocessamentoCusto,
                    DtFinalReprocessamentoCusto = @DtFinalReprocessamentoCusto,
                    IdUsuarioAlteracao = @IdUsuarioAlteracao",
                    "IDRelacionamentoItemPrincipal = @IDRelacionamentoItemPrincipal",
                    relacionamento);
            }
        }
        #endregion
    }
}
