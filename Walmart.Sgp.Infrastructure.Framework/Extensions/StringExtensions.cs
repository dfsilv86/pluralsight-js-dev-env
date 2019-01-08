using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Extensions
{
    /// <summary>
    /// Extensões para string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Limita uma mensagem em um número específico de caracteres, adicionando reticências no final caso necessário.
        /// </summary>
        /// <param name="message">A mensagem.</param>
        /// <param name="limit">O número de caracteres limite.</param>
        /// <returns>A mensagem, truncada caso seja maior que o número de caracteres informado, com reticências no final.</returns>
        public static string LimitTo(this string message, int limit)
        {
            if (null == message)
            {
                return message;
            }

            if (message.Length > limit)
            {
                if (limit > 6)
                {
                    return message.Substring(0, limit - 3) + "...";
                }
                else
                {
                    return message.Substring(0, limit);
                }
            }
            else
            {
                return message;
            }
        }
    }
}
