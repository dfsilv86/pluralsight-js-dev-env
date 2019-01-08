using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec VendorRoteiroDeveConterItensDSDSpec
    /// </summary>
    public class VendorRoteiroDeveConterItensDSDSpec : SpecBase<Roteiro>
    {
        private readonly Func<long, bool> m_possuiItensDSD;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VendorRoteiroDeveConterItensDSDSpec"/>
        /// </summary>
        /// <param name="possuiItensDSD">Função para validar se um vendor possui itens DSD.</param>
        public VendorRoteiroDeveConterItensDSDSpec(Func<long, bool> possuiItensDSD)
        {
            m_possuiItensDSD = possuiItensDSD;
        }

        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Roteiro target)
        {
            if (m_possuiItensDSD(target.cdV9D))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.VendorHasNotDSDItens);
        }
    }
}
