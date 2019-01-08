using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Helpers
{
    /// <summary>
    /// Classe para ajudar com calculos
    /// </summary>
    public static class CalcHelper
    {
        /// <summary>
        /// Executa uma divisao customizada
        /// </summary>
        /// <param name="dividendo">O dividendo</param>
        /// <param name="divisor">O divisor</param>
        /// <param name="limite">Limite: será arredondado para baixo se o valor ficar até este valor, incluindo o mesmo.</param>
        /// <returns>Resultado da divisao</returns>
        public static double CustomDivision(double dividendo, double divisor, double limite = 0.5)
        {
            if (divisor.Equals(0.0))
            {
                return divisor;
            }

            double answer = dividendo / divisor;
            double floor = Math.Floor(answer);
            double ceiling = Math.Ceiling(answer);

            double remainder = answer - floor;

            bool roundUp = Math.Abs(remainder) >= limite ? true : false;

            answer = roundUp ? ceiling : floor;

            return answer;

        }
    }
}
