using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Argumento (parâmetro) para um processamento.
    /// </summary>
    [DebuggerDisplay("Argument: {Name} of PO {ProcessOrderId}, Value: {Value}, Exposed: {IsExposed}")]
    [Serializable]
    public class ProcessOrderArgument : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id do argumento.
        /// </summary>
        public int ProcessOrderArgumentId { get; set; }

        /// <summary>
        /// Obtém ou define o id do argumento.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.ProcessOrderArgumentId;
            }

            set
            {
                this.ProcessOrderArgumentId = value;
            }
        }

        /// <summary>
        /// Obtém ou define o id do processamento.
        /// </summary>
        public int ProcessOrderId { get; set; }

        /// <summary>
        /// Obtém ou define o nome do argumento.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define o valor do argumento.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o valor do parâmetro pode ser exibido em tela.
        /// </summary>
        public bool IsExposed { get; set; }
    }
}
