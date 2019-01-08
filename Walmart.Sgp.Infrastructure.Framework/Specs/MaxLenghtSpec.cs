using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Spec para tamanho maximo de string
    /// </summary>
    public class MaxLenghtSpec : SpecBase<string>
    {
        private int maxLenght;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MaxLenghtSpec"/>.
        /// </summary>
        /// <param name="maxLenght">Tamanho maximo do campo.</param>
        public MaxLenghtSpec(int maxLenght)
        {
            this.maxLenght = maxLenght;
        }

        /// <summary>
        /// Verifica se o object informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O object.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(string target)
        {
            if (target.Length > maxLenght)
            {
                return NotSatisfied(Texts.MaxLenghtExceed);
            }

            return Satisfied();
        }
    }
}
