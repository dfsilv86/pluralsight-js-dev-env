using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Infrastructure.Web.Serialization
{
    /// <summary>
    /// Serializador/deserializador de FileVaultTicket para Json.
    /// </summary>
    public class FileVaultTicketConverter : JsonConverter
    {
        /// <summary>
        /// Indica se pode converter o tipo do objeto.
        /// </summary>
        /// <param name="objectType">O tipo do objeto.</param>
        /// <returns>True se for um FileVaultTicket.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(FileVaultTicket).IsAssignableFrom(objectType);
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

            FileVaultTicket target = new FileVaultTicket();

            SharedConverterHelper.CreateReaderDeserialize(reader, serializer, jsonObject, target);

            return FileVaultTicket.Deserialize(target.Id, target.CreatedDate);
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
            var ticket = (FileVaultTicket)value;

            var stripped = new { Id = ticket.Id, CreatedDate = ticket.CreatedDate };

            serializer.Serialize(writer, stripped);
        }
    }
}