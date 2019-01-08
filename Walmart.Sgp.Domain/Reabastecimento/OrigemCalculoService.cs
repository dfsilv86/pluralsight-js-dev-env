using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Implementação de IOrigemCalculoSevice.
    /// </summary>
    public class OrigemCalculoService : IOrigemCalculoService
    {
        #region Fields
        private readonly IOrigemCalculoGateway m_gateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="OrigemCalculoService"/>.
        /// </summary>
        /// <param name="gateway">O table data gateway origem de cálculo.</param>
        public OrigemCalculoService(IOrigemCalculoGateway gateway)
        {
            m_gateway = gateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém as disponibilidades das origens de cálculo (autómatico ou manual) para o dia informado.
        /// </summary>
        /// <param name="dia">O dia a ser consultado.</param>
        /// <returns>As disponibilidades.</returns>
        public DisponibilidadeOrigemCalculo ObterDisponibilidade(DateTime dia)
        {
            return m_gateway.ObterDisponibilidade(dia);
        }
        #endregion
    }
}
