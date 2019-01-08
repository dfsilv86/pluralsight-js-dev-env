using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Interface para marcação dos fixed values, utilizada pelo model binder.
    /// </summary>
    public interface IFixedValue
    {
        /// <summary>
        /// Obtém o valor.
        /// </summary>
        object ValueAsObject { get; }
    }
}
