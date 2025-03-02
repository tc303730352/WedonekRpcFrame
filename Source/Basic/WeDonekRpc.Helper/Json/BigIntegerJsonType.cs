using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    public class BigIntegerJsonType : JsonConverter<BigInteger>
    {
        public override BigInteger Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return BigInteger.Zero;
            }
            return BigInteger.Parse(reader.GetString());
        }

        public override void Write (Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
        {
            if (value != BigInteger.Zero)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
