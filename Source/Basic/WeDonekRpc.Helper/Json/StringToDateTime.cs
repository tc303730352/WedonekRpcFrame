using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    internal class StringToDateTime : JsonConverter<DateTime>
    {
        public override DateTime Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return DateTime.MinValue;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string str = reader.GetString();
                if (str == string.Empty)
                {
                    return DateTime.MinValue;
                }
                return DateTime.Parse(str);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                long time = reader.GetInt64();
                return Tools.GetTimeStamp(time);
            }
            return DateTime.MinValue;
        }

        public override void Write (Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
        }
    }
}
