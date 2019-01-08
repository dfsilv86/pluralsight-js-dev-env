using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma origem de dados cálculo.
    /// </summary>
    public class OrigemDadosCalculo : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDOrigemDadosCalculo;
            }

            set
            {
                IDOrigemDadosCalculo = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDOrigemDadosCalculo.
        /// </summary>
        public int IDOrigemDadosCalculo { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string DsOrigem { get; set; }
    }
}
