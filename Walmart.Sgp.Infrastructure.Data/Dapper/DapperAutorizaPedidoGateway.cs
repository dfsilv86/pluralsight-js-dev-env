using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para autoriza pedido utilizando o Dapper.
    /// </summary>
    public class DapperAutorizaPedidoGateway : EntityDapperDataGatewayBase<AutorizaPedido>, IAutorizaPedidoGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperAutorizaPedidoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperAutorizaPedidoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "AutorizaPedido", "IdAutorizaPedido")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get 
            {
                return new string[] { "IdLoja", "IdDepartamento", "dtPedido", "dtAutorizacao", "IdUser" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém as autorizações de pedido pelo id da sugestão de pedido.
        /// </summary>
        /// <param name="idSugestaoPedido">O id da sugestão de pedido.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As autorizações.</returns>
        public IEnumerable<AutorizaPedido> ObterAutorizacoesPorSugestaoPedido(int idSugestaoPedido, Paging paging)
        {
            return Resource.Query<AutorizaPedido, Loja, Departamento, Usuario, AutorizaPedido>(
                Sql.AutorizaPedido.ObterAutorizacoesPorSugestaoPedido,
                new { idSugestaoPedido },
                (a, l, d, u) =>
                {
                    a.Loja = l;
                    a.Departamento = d;
                    a.User = u;

                    return a;
                },
                "SplitOn1,SplitOn2,SplitOn3")
                .AsPaging(paging);
        }
        #endregion
    }
}
