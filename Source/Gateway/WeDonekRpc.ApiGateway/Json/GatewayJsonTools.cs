using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.ApiGateway.Json
{
    public class GatewayJsonTools
    {
        private static readonly JsonSerializerOptions _DeserializeOption;

        private static readonly JsonSerializerOptions _SerializerOption;
        static GatewayJsonTools ()
        {
            _DeserializeOption = JsonTools.GetDeserializeOption();
            _SerializerOption = JsonTools.GetSerializerOption();
        }

        public static void Enable ()
        {
            _SerializerOption.Converters.Add(new LongToString());
        }

        public static string Json ( object obj )
        {
            return JsonSerializer.Serialize(obj, _SerializerOption);
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
                new ErrorLog(e, "JSON反序列化失败!", "Type:" + type.FullName, "gateway")
                {
                         { "Json",str }
                }.Save();
                return default;
            }
        }

        public static T Json<T> ( string str )
        {
            try
            {
                return JsonSerializer.Deserialize<T>(str, _DeserializeOption);
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "JSON反序列化失败!", "Type:" + typeof(T).FullName, "gateway")
                {
                        { "Json",str }
                }.Save();
                return default;
            }
        }

        public static void Stop ()
        {
            Type type = typeof(LongToString);
            JsonConverter item = _SerializerOption.Converters.Where(c=> c.Type == type).FirstOrDefault();
            if ( item != null )
            {
                _SerializerOption.Converters.Remove(item);
            }
        }
    }
}
