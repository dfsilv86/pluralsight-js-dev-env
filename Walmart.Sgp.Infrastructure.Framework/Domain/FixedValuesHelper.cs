using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Utilitários para FixedValues.
    /// </summary>
    public static class FixedValuesHelper
    {
        #region Fields
        private static Dictionary<Type, MethodInfo> s_operatorsCache = new Dictionary<Type, MethodInfo>();
        #endregion

        /// <summary>
        /// Obtém o operador de conversão do FixedValues informado.
        /// </summary>
        /// <param name="fixedValuesType">O tipo do FixedValues.</param>
        /// <returns>O operador de conversão.</returns>
        public static MethodInfo GetConversionOperator(Type fixedValuesType)
        {
            if (!s_operatorsCache.ContainsKey(fixedValuesType))
            {
                // TODO: existe uma forma melhor de buscar o operador?
                s_operatorsCache[fixedValuesType] = fixedValuesType
                    .GetMethods()
                    .FirstOrDefault(m => m.Name.StartsWith("op_Implicit", StringComparison.OrdinalIgnoreCase) && m.GetParameters().Count(p => p.ParameterType == typeof(string) || p.ParameterType == typeof(int) || p.ParameterType == typeof(short)) == 1);
            }

            return s_operatorsCache[fixedValuesType];
        }
    }
}
