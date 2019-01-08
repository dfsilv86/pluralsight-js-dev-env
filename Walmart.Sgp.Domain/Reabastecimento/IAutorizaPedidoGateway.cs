using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um table data gateway para autoriza pedido.
    /// </summary>
    public interface IAutorizaPedidoGateway : IDataGateway<AutorizaPedido>
    {
        /// <summary>
        /// Obtém as autorizações de pedido pelo id da sugestão de pedido.
        /// </summary>
        /// <param name="idSugestaoPedido">O id da sugestão de pedido.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As autorizações.</returns>
        IEnumerable<AutorizaPedido> ObterAutorizacoesPorSugestaoPedido(int idSugestaoPedido, Paging paging);
    }
}
