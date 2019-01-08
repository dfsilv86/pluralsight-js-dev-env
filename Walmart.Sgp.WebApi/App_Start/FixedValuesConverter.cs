using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.WebApi.App_Start
{
    public class FixedValuesConverter : JsonConverter
    {
        private static readonly Dictionary<Type, Action<JsonWriter, object>> Serializators = new Dictionary<Type, Action<JsonWriter, object>>();
        private static readonly Dictionary<Type, Func<object, object>> Constructors = new Dictionary<Type, Func<object, object>>();
        private static readonly Dictionary<Type, Func<JsonReader, Type, object>> Deserializators = new Dictionary<Type, Func<JsonReader, Type, object>>();

        public override bool CanConvert(Type objectType)
        {
            return typeof(IFixedValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            lock (Deserializators)
            {
                lock (Constructors)
                {
                    if (!Deserializators.ContainsKey(objectType) || !Constructors.ContainsKey(objectType))
                    {
                        RegisterDeserializator(objectType);
                    }
                }
            }

            Func<JsonReader, Type, object> worker = null;

            worker = Deserializators[objectType];

            var deserializedValue = worker(reader, objectType);

            return deserializedValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (null == value)
            {
                writer.WriteValue((string)null);
                return;
            }

            Type fixedValuesType = value.GetType();

            lock (Serializators)
            {
                if (!Serializators.ContainsKey(fixedValuesType))
                {
                    RegisterSerializator(fixedValuesType);
                }
            }

            Action<JsonWriter, object> worker = null;

            worker = Serializators[fixedValuesType];

            worker(writer, value);
        }

        private static Type GetUnderlyingType(Type objectType)
        {
            Type theType = objectType;

            while (theType != typeof(object) && (theType.Name != typeof(FixedValuesBase<>).Name))
            {
                theType = objectType.BaseType;
            }

            if (theType == typeof(object))
            {
                throw new InvalidOperationException(Texts.UnableToDetermineFixedValueUnderlyingType.With(objectType.Name));
            }

            return theType.GenericTypeArguments[0];
        }

        #region Workers

        private object DeserializeString(JsonReader reader, Type objectType)
        {
            string value = (string)reader.Value;

            object deserializedValue = Constructors[objectType](value);

            return Convert.ChangeType(deserializedValue, objectType);
        }

        private object DeserializeInt32(JsonReader reader, Type objectType)
        {
            object deserializedValue = null;

            if (null == reader.Value)
            {
                deserializedValue = Constructors[objectType](0);
            }
            else
            {
                int value = (int)Convert.ChangeType(reader.Value, typeof(int));

                deserializedValue = Constructors[objectType](value);
            }

            return Convert.ChangeType(deserializedValue, objectType);
        }

        private object DeserializeInt16(JsonReader reader, Type objectType)
        {
            object deserializedValue = null;

            if (null == reader.Value)
            {
                deserializedValue = Constructors[objectType](0);
            }
            else
            {
                short value = (short)Convert.ChangeType(reader.Value, typeof(short));

                deserializedValue = Constructors[objectType](value);
            }

            return Convert.ChangeType(deserializedValue, objectType);
        }

        private object DeserializeNullableInt16(JsonReader reader, Type objectType)
        {
            object deserializedValue = null;

            if (null == reader.Value)
            {
                deserializedValue = new short?();
            }
            else
            {
                short? value = (short?)Convert.ChangeType(reader.Value, typeof(short));

                deserializedValue = Constructors[objectType](value);
            }

            return Convert.ChangeType(deserializedValue, objectType);
        }

        private void SerializeString(JsonWriter writer, object value)
        {
            string convertedValue = (string)Convert.ChangeType(value, typeof(string));

            writer.WriteValue(convertedValue);
        }

        private void SerializeInt32(JsonWriter writer, object value)
        {
            int convertedValue = (int)Convert.ChangeType(value, typeof(int));

            writer.WriteValue(convertedValue);
        }

        private void SerializeNullableInt32(JsonWriter writer, object value)
        {
            int convertedValue = (int)Convert.ChangeType(value, typeof(int));

            writer.WriteValue(convertedValue);
        }

        private void SerializeInt16(JsonWriter writer, object value)
        {
            short convertedValue = (short)Convert.ChangeType(value, typeof(short));

            writer.WriteValue(convertedValue);
        }

        private void SerializeNullableInt16(JsonWriter writer, object value)
        {
            short convertedValue = (short)Convert.ChangeType(value, typeof(short));

            writer.WriteValue(convertedValue);
        }

        #endregion

        #region Register

        private void RegisterDeserializator(Type objectType)
        {
            Type underlyingType = GetUnderlyingType(objectType);

            var conversionOperator = FixedValuesHelper.GetConversionOperator(objectType);

            Constructors[objectType] = (theValue) => conversionOperator.Invoke(null, new object[] { theValue });

            // TODO: adicionar outros tipos caso sejam usados em FixedValues
            if (underlyingType == typeof(int))
            {
                Deserializators[objectType] = DeserializeInt32;
            }
            else if (underlyingType == typeof(short))
            {
                Deserializators[objectType] = DeserializeInt16;
            }
            else if (underlyingType == typeof(short?))
            {
                Deserializators[objectType] = DeserializeNullableInt16;
            }
            else if (underlyingType == typeof(string))
            {
                Deserializators[objectType] = DeserializeString;
            }
        }

        private void RegisterSerializator(Type fixedValuesType)
        {
            Type underlyingType = GetUnderlyingType(fixedValuesType);

            // TODO: adicionar outros tipos caso sejam usados em FixedValues
            if (underlyingType == typeof(int))
            {
                Serializators[fixedValuesType] = SerializeInt32;
            }
            else if (underlyingType == typeof(int?))
            {
                Serializators[fixedValuesType] = SerializeNullableInt32;
            }
            else if (underlyingType == typeof(short))
            {
                Serializators[fixedValuesType] = SerializeInt16;
            }
            else if (underlyingType == typeof(short?))
            {
                Serializators[fixedValuesType] = SerializeNullableInt16;
            }
            else if (underlyingType == typeof(string))
            {
                Serializators[fixedValuesType] = SerializeString;
            }
        }

        #endregion
    }
}