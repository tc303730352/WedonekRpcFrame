using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    public class IPAddressJsonType : JsonConverter<IPAddress>
    {
        public override IPAddress? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            return IPAddress.Parse(reader.GetString());
        }

        public override void Write (Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
