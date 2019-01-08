using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Extensions
{
    /// <summary>
    /// Extensão para obter uma chave de globalização no singular ou plural conforme um valor especificado e uma chave base.
    /// </summary>
    public static class PluralizingMessageExtensions
    {
        /// <summary>
        /// Determina o nome de uma chave de globalização (no singular ou plural) a partir de uma chave raiz e de uma quantidade.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <param name="key">A chave raiz.</param>
        /// <returns>O nome da chave que é composta pela raiz + "Singular" ou "Plural" conforme o número informado.</returns>
        public static string PluralizedMessage(this int number, string key)
        {
            string otherKey = number == 1 ? "Singular" : "Plural";

            return GlobalizationHelper.GetText("{0}{1}".With(key, otherKey));
        }
    }
}
