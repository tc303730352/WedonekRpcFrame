using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    public class StringPaseBool : JsonConverter<bool>
    {
        public override bool Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return false;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString() == PublicDataDic.TrueValue;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32() != 0;
            }
            return reader.GetBoolean();
        }

        public override void Write (Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value ? PublicDataDic.TrueValue : PublicDataDic.FalseValue);
        }
    }
}
