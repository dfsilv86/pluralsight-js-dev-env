using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa um parâmetro de loja/CD por departamento.
    /// </summary>    
    public class LojaCdParametro : LojaCdParametroBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaCdParametro"/>.
        /// </summary>
        public LojaCdParametro()
        {
            ReviewDates = new List<ReviewDateCD>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define tpReabastecimento.
        /// </summary>
        public TipoReabastecimento tpReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define ad datas de revisão.
        /// </summary>
        public IList<ReviewDateCD> ReviewDates { get; set; }
        #endregion
    }

}
