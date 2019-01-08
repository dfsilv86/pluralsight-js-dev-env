using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Equality comparer para UsuarioRO que realiza a comparação pela propriedade Login.
    /// </summary>
    public class UsuarioTOEqualityComparer : IEqualityComparer<UsuarioTO>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare</param>
        /// <param name="y"> The second object of type T to compare</param>
        /// <returns>true if the specified objects are equal; otherwise, false</returns>
        public bool Equals(UsuarioTO x, UsuarioTO y)
        {
            return x.Login == y.Login;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The System.Object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(UsuarioTO obj)
        {
            return obj.Login.GetHashCode();
        }
    }
}
