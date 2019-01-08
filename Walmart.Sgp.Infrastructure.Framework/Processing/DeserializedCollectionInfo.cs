using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Informações sobre uma coleção deserializada.
    /// </summary>
    internal class DeserializedCollectionInfo
    {
        /// <summary>
        /// Obtém ou define o tipo da chave da coleção.
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do elemento da coleção.
        /// </summary>
        public Type ElementType { get; set; }

        /// <summary>
        /// Obtém ou define a coleção deserializada.
        /// </summary>
        public object Value { get; set; }
    }
}
