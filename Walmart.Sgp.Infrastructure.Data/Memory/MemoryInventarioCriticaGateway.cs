using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para crítica de inventário em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryInventarioCriticaGateway : MemoryDataGateway<InventarioCritica>, IInventarioCriticaGateway
    {
        /// <summary>
        /// Realiza a inativação das críticas do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void InativarCriticas(Inventario inventario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa as críticas de inventário pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro para críticas de inventário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As críticas de inventário.</returns>
        public IEnumerable<InventarioCritica> Pesquisar(InventarioCriticaFiltro filtro, Paging paging)
        {
            throw new NotImplementedException();
        }
    }
}