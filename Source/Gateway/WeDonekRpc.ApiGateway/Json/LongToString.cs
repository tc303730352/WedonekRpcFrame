using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.ApiGateway.Json
{
    internal class LongToString : JsonConverter<long>
    {
        public override long Read ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            return 0;
        }

        public override void Write ( Utf8JsonWriter writer, long value, JsonSerializerOptions options )
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
