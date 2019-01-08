using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa um parâmetro de loja/CD por departamento.
    /// </summary>
    public class LojaCdParametroPorDepartamento : LojaCdParametroBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define a data de revisão do CD.
        /// </summary>
        public ReviewDateCD ReviewDateCd { get; set; }
    }

}
