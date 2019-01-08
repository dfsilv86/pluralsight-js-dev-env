using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Walmart.Sgp.Infrastructure.Web.Serialization
{
    /// <summary>
    /// Classe para compartilhar código usado pelos converters.
    /// </summary>
    internal static class SharedConverterHelper
    {
        /// <summary>
        /// Cria um reader de JSON e deserializa um objeto.
        /// </summary>
        /// <param name="reader">O reader.</param>
        /// <param name="serializer">O serializador.</param>
        /// <param name="jsonObject">O objeto json.</param>
        /// <param name="target">A instancia sendo deserializada.</param>
        /// <typeparam name="T">O tipo do objeto sendo deserializado.</typeparam>
        public static void CreateReaderDeserialize<T>(JsonReader reader, JsonSerializer serializer, JObject jsonObject, T target)
        {
            JsonReader jsonObjectReader = jsonObject.CreateReader();
            jsonObjectReader.Culture = reader.Culture;
            jsonObjectReader.DateParseHandling = reader.DateParseHandling;
            jsonObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jsonObjectReader.FloatParseHandling = reader.FloatParseHandling;

            serializer.Populate(jsonObjectReader, target);
        }
    }
}
