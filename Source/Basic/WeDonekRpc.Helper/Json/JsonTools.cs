using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper.Json
{
    public class JsonTools
    {
        private static readonly JsonSerializerOptions _DeserializeOption = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            MaxDepth = 64,
            IncludeFields = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private static readonly JsonSerializerOptions _SerializerOption = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            MaxDepth = 64,
            IncludeFields = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        static JsonTools ()
        {
            _SerializerOption.Converters.Add(new EndPointJsonType());
            _SerializerOption.Converters.Add(new IPAddressJsonType());
            _SerializerOption.Converters.Add(new BigIntegerJsonType());
            _DeserializeOption.Converters.Add(new ObjectJsonConvert());
            _DeserializeOption.Converters.Add(new EndPointJsonType());
            _DeserializeOption.Converters.Add(new IPAddressJsonType());
            _DeserializeOption.Converters.Add(new BigIntegerJsonType());
            _DeserializeOption.Converters.Add(new JsonStringEnumConverter());
            _DeserializeOption.Converters.Add(new StringPaseBool());
            _DeserializeOption.Converters.Add(new StringPaseInt());
            _DeserializeOption.Converters.Add(new StringPaseLong());
            _DeserializeOption.Converters.Add(new StringToNumber());
            _DeserializeOption.Converters.Add(new StringToDateTime());
        }
        public static JsonSerializerOptions GetSerializerOption ()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                MaxDepth = 64,
                IncludeFields = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new EndPointJsonType());
            options.Converters.Add(new IPAddressJsonType());
            options.Converters.Add(new BigIntegerJsonType());
            return options;
        }
        public static JsonSerializerOptions GetDeserializeOption ()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                MaxDepth = 64,
                IncludeFields = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new ObjectJsonConvert());
            options.Converters.Add(new EndPointJsonType());
            options.Converters.Add(new IPAddressJsonType());
            options.Converters.Add(new BigIntegerJsonType());
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new StringPaseBool());
            options.Converters.Add(new StringPaseInt());
            options.Converters.Add(new StringPaseLong());
            options.Converters.Add(new StringToNumber());
            options.Converters.Add(new StringToDateTime());
            return options;
        }
        public static void JsonReadConverter<T> ( JsonConverter<T> add )
        {
            _DeserializeOption.Converters.Add(add);
        }
        public static void JsonWriteConverter<T> ( JsonConverter<T> add )
        {
            _SerializerOption.Converters.Add(add);
        }
        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Json<T> ( string str )
        {
            try
            {
                return JsonSerializer.Deserialize<T>(str, _DeserializeOption);
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "JSON反序列化失败!", "Type:" + typeof(T).FullName, "Json")
                {
                        { "Json",str }
                }.Save();
                return default;
            }
        }


        public static dynamic Json ( string str, Type type )
        {
            if ( str.IsNull() )
            {
                return Tools.GetTypeDefValue(type);
            }
            try
            {
                return JsonSerializer.Deserialize(str, type, _DeserializeOption);
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "JSON反序列化失败!", "Type:" + type.FullName, "Json")
                {
                         { "Json",str }
                }.Save();
                return default;
            }
        }
        /// <summary>
        /// 将对象JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Json ( object obj )
        {
            return JsonSerializer.Serialize(obj, _SerializerOption);
        }
        public static string Json<T> ( T obj )
        {
            try
            {
                return JsonSerializer.Serialize<T>(obj, _SerializerOption);
            }
            catch ( Exception e )
            {
                string type = typeof(T).FullName;
                new ErrorLog(e, "JSON序列化失败!", "Type:" + type, "Json").Save();
                return default;
            }
        }
        public static string Json ( object obj, Type type )
        {
            try
            {
                return JsonSerializer.Serialize(obj, type, _SerializerOption);
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "JSON序列化失败!", "Type: " + type.FullName, "Json").Save();
                return default;
            }
        }
    }
}
