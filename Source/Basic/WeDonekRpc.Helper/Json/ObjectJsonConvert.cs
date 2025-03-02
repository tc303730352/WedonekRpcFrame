using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Helper.Json
{
    internal class ObjectJsonConvert : JsonConverter<object>
    {
        private object _GetObject (ref Utf8JsonReader reader)
        {
            if (reader.TryGetDateTime(out DateTime time))
            {
                return time;
            }
            else if (reader.TryGetGuid(out Guid guid))
            {
                return guid;
            }
            else
            {
                return reader.GetString();
            }
        }
        private object _GetNumber (ref Utf8JsonReader reader)
        {
            if (reader.TryGetByte(out byte data))
            {
                return data;
            }
            else if (reader.TryGetInt16(out short num))
            {
                return num;
            }
            else if (reader.TryGetInt32(out int num1))
            {
                return num1;
            }
            else if (reader.TryGetInt64(out long num2))
            {
                return num2;
            }
            else if (reader.TryGetSingle(out float single))
            {
                return single;
            }
            else if (reader.TryGetDouble(out double doub))
            {
                return doub;
            }
            else
            {
                return reader.GetDecimal();
            }
        }
        private object _GetValue (ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return this._GetObject(ref reader);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return this._GetNumber(ref reader);
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                Dictionary<string, object> obj = [];
                string key = null;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }
                    else if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        key = reader.GetString();
                        continue;
                    }
                    obj[key] = this._GetValue(ref reader);
                }
                return obj;
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                List<object> array = [];
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }
                    else
                    {
                        array.Add(this._GetValue(ref reader));
                    }
                }
                return array;
            }
            return reader.GetString();
        }
        public override object Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return this._GetValue(ref reader);
        }

        public override void Write (Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {

        }
    }
}
