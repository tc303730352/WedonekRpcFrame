using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    internal class StringToNumber : JsonConverter<string>
    {
        public override string Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return PublicDataDic.TrueValue;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return PublicDataDic.FalseValue;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return Encoding.UTF8.GetString(reader.ValueSpan);
            }
            return null;
        }

        public override void Write (Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            
        }
    }
}
