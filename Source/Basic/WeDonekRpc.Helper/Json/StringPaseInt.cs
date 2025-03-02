using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    internal class StringPaseInt : JsonConverter<int>
    {
        public override int Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return int.Parse(reader.GetString());
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            return 0;
        }

        public override void Write (Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
        }
    }
}
