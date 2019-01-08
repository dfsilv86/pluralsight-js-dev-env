using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Extension methods para database.
    /// </summary>
    public static class DBExtensions
    {
        /// <summary>
        /// Converte o valor bool? para a string esperada no banco de dados ("S" || "N" || NULL).
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>A string.</returns>
        public static string ToDb(this bool? value)
        {            
            return value.HasValue ? value.Value.ToDb() : (string)null;
        }

        /// <summary>
        /// Converte o valor bool para a string esperada no banco de dados ("S" || "N").
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>A string.</returns>
        public static string ToDb(this bool value)
        {
            return value ? "S" : "N";
        }
    }
}
