using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Web.Serialization
{
    /// <summary>
    /// Serializador/deserializador de FileVaultTicket para Json.
    /// </summary>
    public class ProcessOrderConverter : JsonConverter
    {
        /// <summary>
        /// Indica se pode converter o tipo do objeto.
        /// </summary>
        /// <param name="objectType">O tipo do objeto.</param>
        /// <returns>True se for um FileVaultTicket.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ProcessOrder).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Deserializa o ticket.
        /// </summary>
        /// <param name="reader">O reader.</param>
        /// <param name="objectType">O tipo do objeto.</param>
        /// <param name="existingValue">O valor já lido.</param>
        /// <param name="serializer">O serializador.</param>
        /// <returns>O ticket.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jsonObject = JObject.Load(reader);

            ProcessOrder target = new ProcessOrder();

            SharedConverterHelper.CreateReaderDeserialize(reader, serializer, jsonObject, target);

            return target;
        }

        /// <summary>
        /// Serializa o ticket.
        /// </summary>
        /// <param name="writer">O writer.</param>
        /// <param name="value">O ticket.</param>
        /// <param name="serializer">O serializador.</param>
        /// <remarks>Remove a propriedade PartialPath.</remarks>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ProcessOrder ticket = (ProcessOrder)value;
            ProcessOrderModel asModel = ticket as ProcessOrderModel;

            var summarized = null != asModel ? asModel.Summarize(true, argument => argument.IsExposed) : ticket.Summarize(true, argument => argument.IsExposed);

            serializer.Serialize(writer, summarized);
        }
    }
}