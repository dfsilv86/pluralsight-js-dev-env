using System;
using System.Linq;
using Newtonsoft.Json;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.WebApi.App_Start
{
    public class LocalDateTimeConverter : JsonConverter
    {
        private static readonly Type[] EnabledTypes = new[] { typeof(DateTime), typeof(DateTime?) };

        public override bool CanConvert(Type objectType)
        {
            return EnabledTypes.Contains(objectType);    
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {            
            return objectType == EnabledTypes[0] ? ReadDateTime(reader) : ReadNullableDateTime(reader);            
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateValue = (DateTime?)value;
            if (!dateValue.HasValue)
            {
                writer.WriteNull();
            }
            else
            {
                var date = DateTime.SpecifyKind(dateValue.Value, DateTimeKind.Local);
                writer.WriteValue(date);
            }
        }

        private object ReadDateTime(JsonReader reader)
        {
            var inputValue = reader.Value;

            if (null == inputValue)
            {
                throw new InvalidCastException(Texts.ErrorWhenConvertingDateAValueMustBeInformed.With(reader.Path));
            }

            var date = DateTime.Parse(inputValue.ToString());
            var localDate = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return localDate;
        }

        private object ReadNullableDateTime(JsonReader reader)
        {
            var inputValue = reader.Value;

            if (null == inputValue)
            {
                return new DateTime();
            }

            DateTime date;

            var isDate = DateTime.TryParse(inputValue.ToString(), out date);
            if (isDate)
            {
                return DateTime.SpecifyKind(date, DateTimeKind.Local);
            }

            DateTime? localDate = DateTime.Now;
            return localDate;
        }
    }
}