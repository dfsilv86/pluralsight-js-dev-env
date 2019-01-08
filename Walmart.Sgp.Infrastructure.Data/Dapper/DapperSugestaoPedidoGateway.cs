using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para sugestao pedido utilizando o Dapper.
    /// </summary>
    public class DapperSugestaoPedidoGateway : EntityDapperDataGatewayBase<SugestaoPedido>, ISugestaoPedidoGateway, IOrigemCalculoGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSugestaoPedidoGateway"/>
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperSugestaoPedidoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "SugestaoPedido", "IDSugestaoPedido")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDItemDetalhePedido", "IDItemDetalheSugestao", "IdLoja", "dtPedido", "tpWeek", "tpInterval", "cdReviewDate", "vlLeadTime", "qtVendorPackage", "dtProximoReviewDate", "dtInicioForecast", "dtFimForecast", "vlEstoqueSeguranca", "vlShelfLife", "vlLeadTimeReal", "blAtendePedidoMinimo", "IDFornecedorParametro", "qtdPackCompra", "qtdPackCompraOriginal", "cdOrigemCalculo", "vlPackSugerido1", "vlModulo", "vlEstoque", "vlTotalPedidosAberto", "vlPipeline", "vlForecast", "vlForecastMedio", "vlEstoqueSegurancaQtd", "vlQtdDiasEstoque", "vlSugestaoPedido", "vlEstoqueOriginal", "vlFatorConversao", "blPossuiVendasUltimaSemana", "tpStatusEnvio", "dhEnvioSugestao" };
            }
        }

        /// <summary>
        /// Pesquisa sugestões de pedidos pelos filtros informados.
        /// </summary>
        /// <param name="request">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>
        /// As sugestões de pedido.
        /// </returns>
        public IEnumerable<SugestaoPedidoModel> PesquisarPorFiltros(SugestaoPedidoFiltro request, Paging paging)
        {
            request.dtPedido = request.dtPedido.Date;

            // A pesquisa retorna do tipo da model para apoio em tela.
            return this.Resource.Query<SugestaoPedidoModel, FornecedorParametro, Fornecedor, ItemDetalhe, FineLine, Loja, ItemDetalhe, SugestaoPedidoModel>(
                Sql.SugestaoPedido.PesquisarPorFiltros_Paging,
                request,
                (sp, fp, fo, id, fl, lj, id2) =>
                {
                    MapSugestaoPedidoFornecedorParametro(sp, fp, fo);
                    MapSugestaoPedidoItemDetalhes(sp, id, fl, id2);

                    sp.Loja = lj;

                    if (null != lj)
                    {
                        lj.IDLoja = sp.IdLoja;
                    }

                    return sp;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6")
                .AsPaging(paging, Sql.SugestaoPedido.PesquisarPorFiltros_Paging, Sql.SugestaoPedido.PesquisarPorFiltros_Count);
        }

        /// <summary>
        /// Obtém a lista de sugestões de pedido de um fornecedor em uma loja.
        /// </summary>
        /// <param name="idFornecedorParametro">O id do parâmetro de fornecedor.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>
        /// As sugestões de pedido.
        /// </returns>
        public IEnumerable<SugestaoPedidoModel> PesquisarPorFornecedorParametroELoja(long idFornecedorParametro, int idLoja)
        {
            return this.Resource.Query<SugestaoPedido, ItemDetalhe, Loja, SugestaoPedidoModel>(
                Sql.SugestaoPedido.PesquisarPorFornecedorParametroELoja,
                new { idFornecedorParametro, idLoja, dtPedido = DateTime.Today },
                (sp, id, lj) =>
                {
                    sp.ItemDetalhePedido = id;
                    sp.Loja = lj;
                    return new SugestaoPedidoModel(sp);
                },
                "SplitOn1,SplitOn2");
        }

        /// <summary>
        /// Consolida o pedido mínimo do fornecedor.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idFornecedorParametro">O id do parâmetro do fornecedor.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="blAdicionaPack">Se adiciona packs.</param>
        /// <remarks>blAdicionaPack vem da informação se o usuário atual é administrador.</remarks>
        public void ConsolidarPedidoMinimo(int idLoja, long idFornecedorParametro, DateTime dtPedido, bool blAdicionaPack)
        {
            this.StoredProcedure.Execute("PRC_ATENDEPEDIDOMINIMO", new { IDLoja = idLoja, IDFornecedorParametro = idFornecedorParametro, DataPedido = dtPedido.Date, blAdicionaPack, blFluxoTela = true });
        }

        /// <summary>
        /// Consolida o pedido mínimo do fornecedor para itens XDOC.
        /// </summary>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="idFornecedorParametro">O id do parâmetro do fornecedor.</param>
        public void ConsolidarPedidoMinimoXDoc(DateTime dtPedido, int idCD, long idFornecedorParametro)
        {
            this.StoredProcedure.Execute("PRC_ATENDEPEDIDOMINIMO_XDoc", new { DataPedido = dtPedido.Date, idCD = idCD, idFornecedorParametro = idFornecedorParametro, blFluxoTela = true });
        }

        /// <summary>
        /// Obtém uma sugestão pedido com dados do item detalhe do pedido e da loja.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestao pedido.</param>
        /// <returns>A sugestão pedido.</returns>
        /// <remarks>Usado pelo processo de alterar sugestão pedido.</remarks>
        public SugestaoPedido ObterEstruturado(int idSugestaoPedido)
        {
            return this.Resource.Query<SugestaoPedido, ItemDetalhe, Loja, ItemDetalhe, FineLine, OrigemDadosCalculo, SugestaoPedidoModel>(
                Sql.SugestaoPedido.ObterEstruturado,
                new { idSugestaoPedido },
                (sp, id, lj, id2, fl, ods) =>
                {
                    sp.ItemDetalhePedido = id;
                    sp.Loja = lj;
                    sp.ItemDetalheSugestao = id2;
                    sp.ItemDetalheSugestao.FineLine = fl;
                    sp.OrigemDadosCalculo = ods;
                    return new SugestaoPedidoModel(sp);
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5").SingleOrDefault();
        }

        /// <summary>
        /// Conta quantas sugestões de pedido existem para os filtros informados.
        /// </summary>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <returns>O número de registros.</returns>
        public int ContarPorDataPedidoLojaEDepartamento(DateTime dtPedido, int cdSistema, int idLoja, int idDepartamento)
        {
            return this.Resource.ExecuteScalar<int>(Sql.SugestaoPedido.ContarPorDataPedidoLojaEDepartamento, new { dtPedido, cdSistema, idLoja, idDepartamento });
        }

        /// <summary>
        /// Obtém as disponibilidades das origens de cálculo (autómatico ou manual) para o dia informado.
        /// </summary>
        /// <param name="dia">O dia a ser consultado.</param>
        /// <returns>As disponibilidades.</returns>
        public DisponibilidadeOrigemCalculo ObterDisponibilidade(DateTime dia)
        {
            var result = this.Resource.QueryOne<dynamic>(Sql.SugestaoPedido.ObterDisponibilidade, new { dtPedido = dia.Date });                

            var disponibilidade = new DisponibilidadeOrigemCalculo { Dia = dia.Date };

            if (result != null)                        
            {
                disponibilidade.ManualDisponivel = result.cdOrigemCalculo.Equals("M", StringComparison.OrdinalIgnoreCase);
                disponibilidade.AutomaticoDisponivel = !disponibilidade.ManualDisponivel;                   
            }

            return disponibilidade;
        }

        /// <summary>
        /// Verifica se existe grade aberta para a data e item informado.
        /// </summary>
        /// <param name="idItemDetalheSaida">O item a pesquisar.</param>
        /// <param name="idFornecedorParametro">O id fornecedor parametro.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Se existe ou não grade aberta.</returns>
        public bool VerificaItemSaidaGradeAberta(int idItemDetalheSaida, int idFornecedorParametro, int cdSistema, DateTime dtPedido)
        {
            var dt = dtPedido.ToString("yyyy-MM-dd 00:00:00", RuntimeContext.Current.Culture);
            var result = this.Resource.ExecuteScalar<int>(
                Sql.SugestaoPedido.QtdPedidosGradeAberta,
                new { IDFornecedorParametro = idFornecedorParametro, idItemDetalheSaida = idItemDetalheSaida, dtPedido = dt, cdSistema = cdSistema });
            
            return result > 0;
        }

        private static void MapSugestaoPedidoItemDetalhes(SugestaoPedidoModel sp, ItemDetalhe id, FineLine fl, ItemDetalhe id2)
        {
            sp.ItemDetalheSugestao = id;

            if (null != id)
            {
                id.IDItemDetalhe = sp.IDItemDetalheSugestao;
                id.FineLine = fl;

                if (null != fl)
                {
                    fl.IDFineLine = (int)id.IDFineline;
                }
            }

            sp.ItemDetalhePedido = id2;

            if (null != id2)
            {
                id2.IDItemDetalhe = sp.IDItemDetalhePedido;
            }
        }

        private static void MapSugestaoPedidoFornecedorParametro(SugestaoPedidoModel sp, FornecedorParametro fp, Fornecedor fo)
        {
            sp.FornecedorParametro = fp;

            if (null != fp)
            {
                fp.IDFornecedorParametro = sp.IDFornecedorParametro;
                sp.FornecedorParametro.Fornecedor = fo;

                if (null != fo)
                {
                    fo.IDFornecedor = fp.IDFornecedor;
                }
            }
        }       
    }
}
