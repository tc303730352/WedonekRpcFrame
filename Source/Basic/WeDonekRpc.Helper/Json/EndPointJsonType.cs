using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    public class EndPointJsonType : JsonConverter<IPEndPoint>
    {
        public override IPEndPoint? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            return IPEndPoint.Parse(reader.GetString());
        }

        public override void Write (Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
