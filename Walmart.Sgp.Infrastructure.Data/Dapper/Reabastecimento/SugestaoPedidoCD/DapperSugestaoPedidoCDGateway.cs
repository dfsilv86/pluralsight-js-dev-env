using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Implementação de um table data gateway para SugestaoPedidoCD utilizando o Dapper.
    /// </summary>
    public class DapperSugestaoPedidoCDGateway : EntityDapperDataGatewayBase<SugestaoPedidoCD>, ISugestaoPedidoCDGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSugestaoPedidoCDGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperSugestaoPedidoCDGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "SugestaoPedidoCD", "IDSugestaoPedidoCD")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idFornecedorParametro", "idItemDetalhePedido", "idItemDetalheSugestao", "idCD", "dtPedido", "dtEnvioPedido", "dtCancelamentoPedido", "dtCancelamentoPedidoOriginal", "dtInicioForecast", "dtFimForecast", "tpWeek", "tpInterval", "cdReviewDate", "vlLeadTime", "qtVendorPackage", "vlEstoqueSeguranca", "tempoMinimoCD", "tpCaixaFornecedor", "vlPesoLiquido", "vlTipoReabastecimento", "vlCusto", "qtdPackCompra", "qtdPackCompraOriginal", "qtdOnHand", "qtdOnOrder", "qtdForecast", "qtdPipeline", "IdOrigemDadosCalculo", "blFinalizado", "tpStatusEnvio", "dhEnvioSugestao" };
            }
        }

        /// <summary>
        /// Obtém um SugestaoPedidoCD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade SugestaoPedidoCD.</returns>
        public SugestaoPedidoCD ObterPorIdEstruturado(long id)
        {
            var args = new { dtSolicitacao = (DateTime?)null, idDepartamento = (int?)null, idCD = (int?)null, idItem = (int?)null, idFornecedorParametro = (int?)null, statusPedido = (int?)null, idSugestaoPedidoCD = id, itemPesoVariavel = 2 };

            return this.Resource.Query<SugestaoPedidoCD, ItemDetalhe, FornecedorParametro, CD, FineLine, OrigemDadosCalculo, dynamic, SugestaoPedidoCD>(
                Sql.SugestaoPedidoCD.Pesquisar,
                args,
                MapSugestaoPedidoCD,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,cdItemSugestao").SingleOrDefault();
        }

        /// <summary>
        /// Pesquisar SugestaoPedidoCD com base nos filtros
        /// </summary>
        /// <param name="filtro">Filtro da busca.</param>
        /// <param name="paging">Parametro de paginacao.</param>
        /// <returns>Retorna lista de SugestaoPedidoCD.</returns>
        public IEnumerable<SugestaoPedidoCD> Pesquisar(SugestaoPedidoCDFiltro filtro, Paging paging)
        {
            var args = new
            {
                dtSolicitacao = filtro.DtSolicitacao.Date,
                idDepartamento = filtro.IdDepartamento,
                idCD = filtro.IdCD,
                idItem = filtro.IdItem,
                idFornecedorParametro = filtro.IdFornecedorParametro,
                statusPedido = filtro.StatusPedido.HasValue ? filtro.StatusPedido.Value : 2,
                idSugestaoPedidoCD = (int?)null,
                itemPesoVariavel = filtro.ItemPesoVariavel
            };

            return this.Resource.Query<SugestaoPedidoCD, ItemDetalhe, FornecedorParametro, CD, FineLine, OrigemDadosCalculo, dynamic, SugestaoPedidoCD>(
                Sql.SugestaoPedidoCD.Pesquisar,
                args,
                MapSugestaoPedidoCD,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,cdItemSugestao").AsPaging(paging);
        }

        /// <summary>
        /// Verificar se existem sugestões finalizadas para o mesmo item detalhe de saída para a mesma data, departamento e CD.
        /// </summary>
        /// <param name="idSugestaoPedidoCD">O identificador da SugestaoPedidoCD.</param>
        /// <returns>Retorna true caso existam sugestões finalizadas para o mesmo item detalhe de saída para a mesma data, departamento e CD, do contrário retorna false.</returns>
        public bool ExisteSugestoesFinalizadasMesmoItemDetalheSaida(long idSugestaoPedidoCD)
        {
            return this.Resource.Query<int>(
                Sql.SugestaoPedidoCD.ExisteSugestoesFinalizadasMesmoItemDetalheSaida,
                new { idSugestaoPedidoCD }).Single() > 0;

        }

        /// <summary>
        /// Pequisar sugestões por filtro.
        /// </summary>
        /// <param name="dtSolicitacao">Data de solicitacao do pedido.</param>
        /// <param name="idDepartamento">ID do departamento.</param>
        /// <param name="idCD">ID do CD.</param>
        /// <param name="idItem">ID do item. (Entrada ou Saida)</param>
        /// <param name="idFornecedorParametro">ID do FornecedorParametro.</param>
        /// <param name="statusPedido">Filtrar por status do pedido: 0 - Nao finalizado, 1 - Finalizado, 2 - Todos</param>
        /// <returns>Retorna sugestões encontradas com base no filtro.</returns>
        public IEnumerable<SugestaoPedidoCD> ObterSugestoesPorFiltro(DateTime dtSolicitacao, int? idDepartamento, int? idCD, int? idItem, int? idFornecedorParametro, int? statusPedido)
        {
            var args = new { dtSolicitacao = dtSolicitacao.Date, idDepartamento, idCD, idItem, idFornecedorParametro, statusPedido = statusPedido.HasValue ? statusPedido.Value : 2, idSugestaoPedidoCD = (int?)null };

            return this.Resource.Query<SugestaoPedidoCD, ItemDetalhe, FornecedorParametro, CD, FineLine, OrigemDadosCalculo, dynamic, SugestaoPedidoCD>(
                Sql.SugestaoPedidoCD.ObterSugestoesPorFiltro,
                args,
                MapSugestaoPedidoCD,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,cdItemSugestao").ToList();
        }

        /// <summary>
        /// Verifica se existe grade aberta para a data e item informado.
        /// </summary>
        /// <param name="idItemDetalheSaida">O item a pesquisar.</param>
        /// <param name="idFornecedorParametro">O id fornecedor parametro.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Se existe ou não grade aberta.</returns>
        public bool VerificaItemSaidaGradeAbertaSugestaoCD(int idItemDetalheSaida, int idFornecedorParametro, int cdSistema, DateTime dtPedido)
        {
            var dt = dtPedido.ToString("yyyy-MM-dd 00:00:00", RuntimeContext.Current.Culture);
            var result = this.Resource.ExecuteScalar<int>(
                Sql.SugestaoPedidoCD.QtdPedidosGradeAberta,
                new { idItemDetalheSaida = idItemDetalheSaida, idFornecedorParametro = idFornecedorParametro, dtPedido = dt, cdSistema = cdSistema });

            return result > 0;
        }

        private static SugestaoPedidoCD MapSugestaoPedidoCD(SugestaoPedidoCD spc, ItemDetalhe itemDetalhePedido, FornecedorParametro fp, CD cd, FineLine fn, OrigemDadosCalculo ods, dynamic itemDetalheSugestao)
        {
            itemDetalhePedido.FineLine = fn;

            spc.ItemDetalhePedido = itemDetalhePedido;
            spc.ItemDetalheSugestao = new ItemDetalhe()
            {
                CdItem = (int)itemDetalheSugestao.cdItemSugestao,
                DsItem = itemDetalheSugestao.dsItemSugestao,
                FineLine = fn
            };

            spc.FornecedorParametro = fp;
            spc.CD = cd;
            spc.OrigemDadosCalculo = ods;

            return spc;
        }
    }
}
