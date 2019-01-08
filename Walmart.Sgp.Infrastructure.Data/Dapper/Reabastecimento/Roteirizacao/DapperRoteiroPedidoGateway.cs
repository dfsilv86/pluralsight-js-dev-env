using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para RoteiroPedido utilizando o Dapper.
    /// </summary>
    public class DapperRoteiroPedidoGateway : EntityDapperDataGatewayBase<RoteiroPedido>, IRoteiroPedidoGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRoteiroPedidoGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRoteiroPedidoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RoteiroPedido", "IDRoteiroPedido")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idRoteiro", "idSugestaoPedido", "blAutorizado", "idUsuarioAutorizacao", "dhAutorizacao" };
            }
        }

        /// <summary>
        /// Busca uma lista de RoteiroPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="paging">Parametro de Paginação.</param>
        /// <returns>uma lista de RoteiroPedido</returns>
        public IEnumerable<RoteiroPedido> ObterRoteirosPedidosPorRoteiroEdtPedido(int idRoteiro, DateTime dtPedido, Paging paging)
        {
            var args = new
            {
                idRoteiro = idRoteiro,
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture)
            };

            return Resource.Query<RoteiroPedido, ItemDetalhe, RoteiroPedido>(
                Sql.RoteiroPedido.ObterRoteirosPedidosPorRoteiroEdtPedido,
                args,
                MapRoteiroPedido,
                "SplitOn1").AsPaging(paging);
        }

        /// <summary>
        /// Busca uma lista de SugestaoPedido
        /// </summary>
        /// <param name="idRoteiro">Id do RoteiroPedido</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <returns>uma lista de SugestaoPedido</returns>
        public IEnumerable<SugestaoPedido> BuscarSugestaoPedidoPorRoteiroItem(int idRoteiro, DateTime dtPedido, int idItemDetalhe)
        {
            var args = new
            {
                idRoteiro = idRoteiro,
                idItemDetalhe = idItemDetalhe,
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture)
            };

            return Resource.Query<SugestaoPedido, ItemDetalhe, Roteiro, SugestaoPedido>(
                Sql.RoteiroPedido.ObterSugestaoPedidoPorRoteiroItem,
                args,
                MapSugestaoPedido,
                "SplitOn1,SplitOn2");
        }

        /// <summary>
        /// Busca uma lista de SugestaoPedido
        /// </summary>
        /// <param name="idRoteiro">Id do Roteiro</param>
        /// <param name="dtPedido">A data do pedido</param>
        /// <returns>uma lista de SugestaoPedido</returns>
        public IEnumerable<SugestaoPedido> BuscarSugestaoPedidoPorRoteiro(int idRoteiro, DateTime dtPedido)
        {
            var args = new
            {
                idRoteiro = idRoteiro,
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture)
            };

            return Resource.Query<SugestaoPedido, ItemDetalhe, Roteiro, SugestaoPedido>(
                Sql.RoteiroPedido.ObterSugestaoPedidoPorRoteiro,
                args,
                MapSugestaoPedido,
                "SplitOn1,SplitOn2");
        }

        /// <summary>
        /// Obtém um RoteiroPedido pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade RoteiroPedido.</returns>
        public RoteiroPedido ObterPorId(long id)
        {
            return this.Find("IDRoteiroPedido=@IDRoteiroPedido", new { IDRoteiroPedido = id }).SingleOrDefault();
        }

        /// <summary>
        /// Obtém os pedidos roteirizados pelos filtros
        /// </summary>
        /// <param name="dtPedido">Data do pedido</param>
        /// <param name="idDepartamento">ID do departamento</param>
        /// <param name="cdV9D">Código Vendor 9 Dig.</param>
        /// <param name="stPedido">Status do pedido (Autorizado(true) ou Não autorizado (false))</param>
        /// <param name="roteiro">Descrição do roteiro</param>
        /// <param name="paging">Parametro de paginação</param>
        /// <returns>Lista de PedidoRoteirizadoConsolidado</returns>
        public IEnumerable<PedidoRoteirizadoConsolidado> ObterPedidosRoteirizados(DateTime dtPedido, int idDepartamento, long? cdV9D, bool? stPedido, string roteiro, Paging paging)
        {
            var args = new
            {
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture),
                idDepartamento = idDepartamento,
                cdV9D = cdV9D,
                stPedido = stPedido,
                roteiro = roteiro
            };

            var result = Resource.Query<PedidoRoteirizadoConsolidado>(Sql.RoteiroPedido.ObterPedidosRoteirizados, args).AsPaging(paging);
            foreach (var r in result)
            {
                var rp = this.ObterDadosAutorizacao(r.idRoteiro, dtPedido);
                if (rp != null)
                {
                    r.DhAutorizacao = rp.dhAutorizacao;
                    r.UsuarioAutorizacao = rp.Usuario.FullName;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtem um RoteiroPedido com dados de autorizacao e usuario populados.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>Um RoteiroPedido com dados de autorizacao populados.</returns>
        public RoteiroPedido ObterDadosAutorizacao(int idRoteiro, DateTime dtPedido)
        {
            var args = new
            {
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture),
                idRoteiro = idRoteiro
            };

            return Resource.Query<RoteiroPedido, Usuario, RoteiroPedido>(Sql.RoteiroPedido.ObterDadosAutorizacao, args, MapDadosAutorizacao, "SplitOn1").SingleOrDefault();
        }

        /// <summary>
        /// Busca os pedidos para serem autorizados.
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <returns>A lista de RoteiroPedidos.</returns>
        public IEnumerable<RoteiroPedido> ObterRoteirosPedidosParaAutorizar(int idRoteiro, DateTime dtPedido)
        {
            var args = new
            {
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture),
                idRoteiro = idRoteiro
            };

            return Resource.Query<RoteiroPedido>(Sql.RoteiroPedido.ObterRoteirosPedidosParaAutorizar, args);
        }

        /// <summary>
        /// Obtém a quantidade de pedidos não autorizados para a data corrente.
        /// </summary>
        /// <param name="idRoteiro">O identificador do roteiro.</param>
        /// <returns>Retorna a quantidade de pedidos não autorizados para a data corrente.</returns>
        public int QtdPedidosNaoAutorizadosParaDataCorrente(int idRoteiro)
        {
            return Resource.Query<int>(Sql.RoteiroPedido.QtdPedidosNaoAutorizadosParaDataCorrente, new { idRoteiro }).Single();
        }

        private SugestaoPedido MapSugestaoPedido(SugestaoPedido sp, ItemDetalhe id, Roteiro r)
        {
            sp.ItemDetalhePedido = id;
            sp.Roteiro = r;

            return sp;
        }

        private RoteiroPedido MapRoteiroPedido(RoteiroPedido rp, ItemDetalhe id)
        {
            rp.ItemDetalhe = id;

            return rp;
        }

        private RoteiroPedido MapDadosAutorizacao(RoteiroPedido rp, Usuario u)
        {
            rp.Usuario = u;
            return rp;
        }
    }
}