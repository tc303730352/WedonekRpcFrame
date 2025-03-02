using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    internal class StringPaseLong : JsonConverter<long>
    {
        public override long Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string str = reader.GetString();
                if (str == string.Empty)
                {
                    return 0;
                }
                return long.Parse(str);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }
            return 0;
        }

        public override void Write (Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
        }
    }
}
