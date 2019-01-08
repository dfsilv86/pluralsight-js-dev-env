using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um serviço de autoriza pedido.
    /// </summary>
    public interface IAutorizaPedidoService : IDomainService<AutorizaPedido>
    {
        /// <summary>
        /// Verifica se existe autorização de pedido para a data, loja e departamento informada.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>True caso exista autorização, false caso contrário.</returns>
        bool ExisteAutorizacaoPedido(DateTime dtPedido, int idLoja, int idDepartamento);

        /// <summary>
        /// Cria o registro de autoriza pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        void AutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento);

        /// <summary>
        /// Obtém as autorizações de pedido pelo id da sugestão de pedido.
        /// </summary>
        /// <param name="idSugestaoPedido">O id da sugestão de pedido.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As autorizações.</returns>
        IEnumerable<AutorizaPedido> ObterAutorizacoesPorSugestaoPedido(int idSugestaoPedido, Paging paging);
    }
}
