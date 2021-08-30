using Newtonsoft.Json;
using System;

namespace RpcHelper.Json
{
        internal class IJsonConverter : JsonConverter
        {
                public override bool CanRead => true;
                public override bool CanWrite => false;
                public override bool CanConvert(Type objectType)
                {
                        if (!objectType.IsInterface)
                        {
                                return false;
                        }
                        return InterfaceCacheHelper.CanConvert(objectType);
                }

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                        if (reader.TokenType == JsonToken.Null)
                        {
                                return null;
                        }
                        return InterfaceCacheHelper.Read(reader, objectType, serializer);
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                      
                }
        }
}
