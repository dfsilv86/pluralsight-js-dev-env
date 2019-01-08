using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Informações necessárias para executar a importação automática de inventário.
    /// </summary>
    [Serializable]
    public class ImportarInventarioAutomaticoRequest
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        [ExposedParameter]
        public byte CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o código da bandeira.
        /// </summary>
        [ExposedParameter]
        public int IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        [ExposedParameter]
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do departamento.
        /// </summary>
        [ExposedParameter]
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o código do departamento.
        /// </summary>
        public int? CdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o id da categoria.
        /// </summary>
        [ExposedParameter]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o código da categoria.
        /// </summary>
        public int? CdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define a data do inventário.
        /// </summary>
        [ExposedParameter]
        public DateTime DataInventario { get; set; }
    }
}
