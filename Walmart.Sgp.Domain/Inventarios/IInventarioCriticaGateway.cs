using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um table data gateway para crítica de inventário.
    /// </summary>
    public interface IInventarioCriticaGateway : IDataGateway<InventarioCritica>
    {
        /// <summary>
        /// Realiza a inativação das críticas do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        void InativarCriticas(Inventario inventario);

        /// <summary>
        /// Pesquisa as críticas de inventário pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro para críticas de inventário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As críticas de inventário.</returns>
        IEnumerable<InventarioCritica> Pesquisar(InventarioCriticaFiltro filtro, Paging paging);
    }
}
