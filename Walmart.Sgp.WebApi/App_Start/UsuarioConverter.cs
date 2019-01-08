using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Walmart.Sgp.Domain.Acessos;

namespace Walmart.Sgp.WebApi.App_Start
{
    public class UsuarioConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Usuario).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jObject = JObject.Load(reader);

            Usuario target = new Usuario();

            JsonReader jObjectReader = jObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            serializer.Populate(jObjectReader, target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var usuario = (Usuario)value;

            var resumo = usuario.Resumir();

            serializer.Serialize(writer, resumo);
        }
    }
}