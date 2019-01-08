using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Data.Dtos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para item detalhe utilizando o Dapper.
    /// </summary>
    public class DapperItemDetalheGateway : EntityDapperDataGatewayBase<ItemDetalhe>, IItemDetalheGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperItemDetalheGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperItemDetalheGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ItemDetalhe", "IDItemDetalhe")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDFineline", "IDCategoria", "IDSubcategoria", "IDDepartamento", "IDFornecedor", "cdItem", "cdOldNumber", "vlCustoUnitario", "cdSistema", "cdUPC", "dsItem", "dsHostItem", "blAtivo", "dhHostCreate", "dhHostUpdate", "blPesadoCaixa", "blPesadoRetaguarda", "cdPLU", "dsTamanhoItem", "dsCor", "dhCriacao", "dhAtualizacao", "tpStatus", "dhAtualizacaoStatus", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "tpVinculado", "tpReceituario", "tpManipulado", "qtVendorPackage", "qtWarehousePackage", "vlFatorConversao", "tpUnidadeMedida", "vlTipoReabastecimento", "vlShelfLife", "blItemTransferencia", "vlModulo", "cdDepartamentoVendor", "cdSequenciaVendor" };
            }
        }

        /// <summary>
        /// Obtém um item pelo seu código e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O detalhe do item.</returns>
        public ItemDetalhe ObterPorItemESistema(long cdItem, byte cdSistema)
        {
            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterPorItemESistema,
                new { cdItem, cdSistema },
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
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
            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterPorOldNumberESistemaEDepartamento,
                new { cdItem, cdSistema, cdDepartamento },
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
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
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterPorPluESistema,
                new { cdPLU, cdSistema },
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
        }

        /// <summary>
        /// Obtém as colunas CdItem, DsItem e TpStatus de um item pelo seu código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retornar o item detalhe.</returns>
        public ItemDetalhe ObterPorCodigoESistemaComProjecao(long cdItem, int cdSistema)
        {
            return this.Find("IDItemDetalhe, CdItem, DsItem, TpStatus, vlTipoReabastecimento", "CdItem=@CdItem AND CdSistema=@CdSistema", new { CdItem = cdItem, CdSistema = cdSistema }).SingleOrDefault();
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
            var args = new { cdSistema, cdItemSaida, idFornecedorParametro };

            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterItemEntradaPorItemSaida,
                args,
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6");
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
            var args = new
            {
                cdItem,
                cdSistema,
                idFornecedorParametro,
                cdPLU = (long?)null,
                dsItem = (string)null,
                tpStatus = tpStatus,
                cdFineLine = (int?)null,
                cdSubcategoria = (int?)null,
                cdCategoria = (int?)null,
                cdDepartamento = (int?)null,
                IDRegiaoCompra = idRegiaoCompra,
                blPerecivel = blPerecivel
            };

            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterItemSaidaPorFornecedorItemEntrada,
                args,
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa detalhe de itens saida pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> ObterListaItemSaidaPorFornecedorItemEntrada(ItemDetalheFiltro filtro, Paging paging)
        {
            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterItemSaidaPorFornecedorItemEntrada,
                filtro,
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").AsPaging(paging, Sql.ItemDetalhe.ObterItemSaidaPorFornecedorItemEntrada_Paging, Sql.ItemDetalhe.ObterItemSaidaPorFornecedorItemEntrada_Count);
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades associadas.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>O ItemDetalhe com informações de Fornecedor, FineLine, Subcategoria, Categoria, Departamento e Divisao.</returns>
        public ItemDetalhe ObterEstruturadoPorId(int idItemDetalhe)
        {
            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.ObterEstruturadoPorId,
                new { idItemDetalhe },
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades RegiaoCompra e AreaCD
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>O ItemDetalhe com informações de RegiaoCompra e AreaCD.</returns>
        public ItemDetalhe ObterEstruturadoExtraPorId(int idItemDetalhe)
        {
            return this.Resource.Query<ItemDetalhe, RegiaoCompra, AreaCD, ItemDetalhe>(
                Sql.ItemDetalhe.ObterEstruturadoExtraPorId,
                new { idItemDetalhe },
                MapItemDetalheExtra,
                "SplitOn1,SplitOn2").SingleOrDefault();
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
        /// <param name="tpVinculado">Vinculo do item.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens.</returns>
        public IEnumerable<ItemDetalhe> PesquisarPorFiltro(long? cdItem, long? cdPLU, string dsItem, string tpStatus, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int? cdSistema, int idUsuario, string tpVinculado, Paging paging)
        {
            var args = new
            {
                cdItem,
                cdPLU,
                dsItem,
                tpStatus,
                cdFineLine,
                cdSubcategoria,
                cdCategoria,
                cdDepartamento,
                cdSistema,
                idUsuario,
                tpVinculado,
            };

            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.PesquisarPorFiltros,
                args,
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6")
            .AsPaging(paging, Sql.ItemDetalhe.PesquisarPorFiltros_Paging, Sql.ItemDetalhe.PesquisarPorFiltros_Count);
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
            var args = new
            {
                cdItem,
                cdPLU,
                dsItem,
                tpStatus,
                cdFineLine,
                cdSubcategoria,
                cdCategoria,
                cdDepartamento,
                cdSistema,
                idUsuario,
                idRegiaoCompra
            };

            return this.Resource.Query<ItemDetalhe, Fornecedor, FineLine, Subcategoria, Categoria, Departamento, Divisao, ItemDetalhe>(
                Sql.ItemDetalhe.PesquisarPorFiltrosComTipoReabastecimento,
                args,
                MapItemDetalhe,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6")
            .AsPaging(paging);
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
            return this.Resource.QueryOne<FornecedorParametro>(Sql.ItemDetalhe.ObterFornecedorParametro, new { idItem = idItem });
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
            var args = new
            {
                cdItem = cdItem,
                cddepartamento = cdDepartamento,
                cdSistema = cdSistema,
                idCD = idCD,
                filtroMS = filtroMS,
                filtroCadastro = filtroCadastro
            };

            return this.Resource.Query<ItemDetalheCD>(Sql.ItemDetalhe.PesquisarItemCD, args)
                .AsPaging(paging);
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
            var args = new
            {
                cdItem = cdItem,
                cdCD = cdCD,
                cdSistema = cdSistema,
            };

            return this.Resource.Query<ItemDetalheCD>(Sql.ItemDetalhe.PesquisarItensEntradaPorSaidaCD, args)
                .AsPaging(paging);
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
            var args = new
            {
                cdItem = cdItem,
                cdCD = cdCD,
                cdSistema = cdSistema,
            };

            return this.Resource.Query<ItemDetalheCD>(Sql.ItemDetalhe.PesquisarItensEntradaPorSaidaCD, args).ToList();
        }

        /// <summary>
        /// Retorna a quantidade de itens de entrada para um determinado item de saída e seu CD.
        /// </summary>
        /// <param name="cdItem">O código (cdItem) do item de saída.</param>
        /// <param name="cdCD">O código (cdCD) do CD a ser pesquisado.</param>
        /// <param name="cdSistema">Codigo do sistema.</param>
        /// <returns>A quantidade de itens de entrada para um determinado item de venda.</returns>
        public int ObterQuantidadeItensEntrada(long cdItem, long cdCD, long cdSistema)
        {
            var args = new
            {
                cdItem = cdItem,
                cdCD = cdCD,
                cdSistema = cdSistema,
            };

            return this.Resource.Query<int>(Sql.ItemDetalhe.PesquisarItensEntradaPorSaidaCD_Count, args).SingleOrDefault();
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
            var args = new
            {
                cdItem = cdItem,
                cdV9D = cdV9D,
                cdSistema = cdSistema
            };

            return this.Resource.Query<int>(Sql.ItemDetalhe.ItemPertenceFornecedor, args).Single() > 0;
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
            var args = new
            {
                cdItemEntrada = cdItemEntrada,
                cdItemSaida = cdItemSaida,
                cdSistema = cdSistema
            };

            return this.Resource.Query<long>(Sql.ItemDetalhe.ItemDetalheEntradaPertenceItemDetalheSaida, args).SingleOrDefault();
        }

        /// <summary>
        /// Altera as informações de unidade de medida e fator conversão, informados na tela de custos do item.
        /// </summary>
        /// <param name="itemDetalhe">O ItemDetalhe.</param>
        /// <remarks>Modifica apenas os campos tpUnidadeMedida e vlFatorConversao.</remarks>
        public void AlterarDadosCustos(ItemDetalhe itemDetalhe)
        {
            this.Resource.Execute(Sql.ItemDetalhe.AlterarDadosCustos, new { idItemDetalhe = itemDetalhe.IDItemDetalhe, cdSistema = itemDetalhe.CdSistema, vlFatorConversao = itemDetalhe.VlFatorConversao, tpUnidadeMedida = itemDetalhe.TpUnidadeMedida.Value });
        }

        /// <summary>
        /// Verifica se o item detalhe entrada pertence ao CD.
        /// </summary>
        /// <param name="cdItemSaida">O código do item detalhe saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true caso o item detalhe saída pertença ao CD, caso contrário retorna false.</returns>
        public bool ItemDetalheSaidaPertenceCD(long cdItemSaida, int cdCD, int cdSistema)
        {
            var args = new
            {
                cdItemSaida = cdItemSaida,
                cdCD = cdCD,
                cdSistema = cdSistema
            };

            return this.Resource.Query<int>(Sql.ItemDetalhe.ItemDetalheSaidaPertenceCD, args).Single() > 0;
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
                relacionamentoSGP,
                cdDepartamento,
                cdItemDetalhe,
                cdV9D,
                idRegiaoCompra,
                cdSistema,
                idReturnSheet
            };

            return this.Resource.Query<ItemDetalhe, FornecedorParametro, Fornecedor, RegiaoCompra, ItemDetalhe>(
                Sql.ItemDetalhe.ObterItensDetalheReturnSheet,
                args,
                MapItemDetalheReturnSheet,
                "SplitOn1,SplitOn2,SplitOn3")
                .AsPaging(paging);
        }

        /// <summary>
        /// Busca itens de acordo com o filtro especificado.
        /// </summary>
        /// <param name="filtro">The filtro.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfazem o filtro.</returns>
        public IEnumerable<ResultadoConsultaItem> PesquisarAbertoPorBandeira(ItemFiltro filtro, Paging paging)
        {
            return Resource.Query<ItemDetalhe, Departamento, Bandeira, ResultadoConsultaItem>(
                Sql.ItemDetalhe.PesquisarAbertoPorBandeira_Paging,
                new
            {
                filtro.IDBandeira,
                filtro.cdItem,
                filtro.dsItem,
                filtro.IdFineline,
                filtro.cdPlu,
                filtro.IdDepartamento,
                tpStatus = filtro.status.Value,
                IdUsuario = RuntimeContext.Current.User.Id,
                filtro.cdSistema,
                tipoPermissao = (int?)null
            },
                (id, de, ba) =>
                {
                    return new ResultadoConsultaItem
                    {
                        bandeira = ba.DsBandeira,
                        cdItem = id.CdItem,
                        descricao = id.DsItem,
                        dsDepartamento = de.dsDepartamento,
                        IDBandeira = ba.IDBandeira,
                        IDItemDetalhe = id.IDItemDetalhe,
                        manipulado = id.TpManipulado.Value != null ? id.TpManipulado.Description : null,
                        plu = id.CdPLU,
                        receituario = id.TpReceituario.Value != null ? id.TpReceituario.Description : null,
                        tamanho = id.DsTamanhoItem,
                        upc = id.CdUPC,
                        vinculado = id.TpVinculado.Value != null ? id.TpVinculado.Description : null
                    };
                },
                "SplitOn1,SplitOn2")
                .AsPaging(paging, Sql.ItemDetalhe.PesquisarAbertoPorBandeira_Paging, Sql.ItemDetalhe.PesquisarAbertoPorBandeira_Count);
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
            return Resource.Query<ResultadoConsultaItemPorLoja>(
                Sql.ItemDetalhe.ObterInformacoesEstoquePorLoja_Paging,
                new { cdItem, idBandeira, idLoja })
                .AsPaging(paging, Sql.ItemDetalhe.ObterInformacoesEstoquePorLoja_Paging, Sql.ItemDetalhe.ObterInformacoesEstoquePorLoja_Count);
        }

        /// <summary>
        /// Obtém as informações cadastrais do item.
        /// </summary>
        /// <param name="cdItem">O código de item.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>As informações cadastrais do item.</returns>
        public Tuple<ItemDetalhe, Bandeira> ObterInformacoesCadastrais(int cdItem, int idBandeira)
        {
            var args = new
            {
                cdItem,
                idBandeira
            };

            var result = StoredProcedure.QueryOne<SelecionarItemInformacoesResult>("PR_SelecionarItemInformacoes", args);
            if (result == null)
            {
                return null;
            }

            var itemDetalhe = SelecionarItemInformacoesResult.ConverterParaItemDetalhe(result);
            var bandeira = SelecionarItemInformacoesResult.ConverterParaBandeira(result);
            return Tuple.Create(itemDetalhe, bandeira);
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
            return Resource.Query<dynamic, ItemDetalhe, ItemCusto, ItemCusto>(
                Sql.ItemDetalhe.ObterItemCustos_Paging,
                new { cdItem, idBandeira, idLoja, idUsuario = RuntimeContext.Current.User.Id, tipoPermissao = (TipoPermissao?)null },
                (loja, itemDetalhe, itemCusto) =>
                {
                    itemCusto.Loja = DtoHelper.DefinirCodigoDescricao<Loja>(loja.Loja as string, (l) => l.cdLoja, (l) => l.nmLoja);
                    itemCusto.Loja.IDLoja = loja.IDLoja;
                    itemCusto.ItemDetalhe = itemDetalhe;

                    return itemCusto;
                },
                "IDItemDetalhe,dtRecebimento")
                .AsPaging(paging, Sql.ItemDetalhe.ObterItemCustos_Paging, Sql.ItemDetalhe.ObterItemCustos_Count);
        }

        /// <summary>
        /// Obtém o valor do tipo de reabastecimento.
        /// </summary>
        /// <param name="idItemDetalheEntrada">O id do item detalhe de entrada.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>Retorna o valor do tipo de reabastecimento.</returns>
        public ValorTipoReabastecimento ObterValorTipoReabastecimento(long idItemDetalheEntrada, int idCD)
        {
            return (ValorTipoReabastecimento)this.Resource.Query<Int16>(
                Sql.ItemDetalhe.ObterValorTipoReabastecimento,
                new { idItemDetalheEntrada, idCD }).SingleOrDefault();
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
            var result = Resource.Query<Loja>(Sql.ItemDetalhe.ObterTraitsPorItem, new { idItemDetalhe = idItemDetalhe, cdSistema = cdSistema });

            return paging == null ? result : result.AsPaging(paging);
        }

        /// <summary>
        /// Verifica se o item detalhe de entrada está vinculado a uma compra casada.
        /// </summary>
        /// <param name="cdItemDetalheEntrada">O código do item detalhe de entrada.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna true se o item estiver vinculado a compra casada, do contrário retorna false.</returns>
        public bool EstaVinculadoCompraCasada(long cdItemDetalheEntrada, long cdSistema)
        {
            return this.Resource.Query<int>(Sql.ItemDetalhe.EstaVinculadoCompraCasada, new { cdItemDetalheEntrada, cdSistema }).Single() > 0;
        }

        private static ItemDetalhe MapItemDetalheReturnSheet(ItemDetalhe itemDetalhe, FornecedorParametro fornecedorParametro, Fornecedor fornecedor, RegiaoCompra regiaoCompra)
        {
            itemDetalhe.FornecedorParametro = fornecedorParametro;
            itemDetalhe.Fornecedor = fornecedor;
            itemDetalhe.RegiaoCompra = regiaoCompra;

            return itemDetalhe;
        }

        private ItemDetalhe MapItemDetalhe(ItemDetalhe itemDetalhe, Fornecedor fornecedor, FineLine fineline, Subcategoria subcategoria, Categoria categoria, Departamento departamento, Divisao divisao)
        {
            itemDetalhe.Fornecedor = fornecedor;
            itemDetalhe.FineLine = fineline;
            ////itemDetalhe.FineLine.Subcategoria = subcategoria;
            itemDetalhe.Subcategoria = subcategoria;
            ////itemDetalhe.Subcategoria.Categoria = categoria;
            itemDetalhe.Categoria = categoria;
            itemDetalhe.Departamento = departamento;
            itemDetalhe.Departamento.Divisao = divisao;
            return itemDetalhe;
        }

        private ItemDetalhe MapItemDetalheExtra(ItemDetalhe itemDetalhe, RegiaoCompra regiaoCompra, AreaCD areaCd)
        {
            itemDetalhe.RegiaoCompra = regiaoCompra;
            itemDetalhe.AreaCD = areaCd;
            return itemDetalhe;
        }
    }
}
