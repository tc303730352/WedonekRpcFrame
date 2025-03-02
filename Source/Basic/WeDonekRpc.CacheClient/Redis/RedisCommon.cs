using System;
using System.Threading.Tasks;
using CSRedis;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Log;
namespace WeDonekRpc.CacheClient.Redis
{
    internal class RedisCommon
    {
        public static CSRedisClient RedisClient { get; private set; } = null;

        public static bool InitCache ( RedisConfig config )
        {
            if ( config.ConConfig == null && config.ConList.IsNull() )
            {
                return false;
            }
            else if ( config.ConConfig != null )
            {
                config.ConConfig.Init();
                if ( config.ConConfig.ConString.IsNull() )
                {
                    return false;
                }
                RedisClient = new CSRedisClient(config.ConConfig.ConString, config.Sentinels, config.ReadOnly)
                {
                    CurrentDeserialize = _deserialize,
                    CurrentSerialize = _serialize
                };
            }
            else
            {
                config.ConList.ForEach(c => c.Init());
                RedisClient = new CSRedisClient(null, config.ConList.ConvertAll(c => c.ConString));
            }
            try
            {
                RedisHelper.Initialization(RedisClient);
                return true;
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "Redis初始化失败!", "Redis")
                {
                    LogContent = config.ToJson()
                }.Save();
                return false;
            }
        }
        private static string _serialize ( object data )
        {
            return JsonTools.Json(data);
        }
        private static object _deserialize ( string json, Type type )
        {
            return JsonTools.Json(json, type);
        }
        public static string RedisVer => RedisClient.NodesServerManager.Info().Find(c => c.node == "redis_version", c => c.value);

        public static string CacheLua ( string lua )
        {
            return RedisClient.ScriptLoad(lua);
        }

        public static void Close ()
        {
            RedisClient.Dispose();
        }

        internal static bool TryGet<T> ( string key, out T res )
        {
            string val = RedisClient.Get(key);
            if ( val.IsNull() )
            {
                res = default;
                return false;
            }
            res = StringParseTools.Parse(val, typeof(T));
            return true;
        }
        public static Task<bool> SetAsync<T> ( string key, T data, TimeSpan? expires )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            return RedisClient.SetAsync(key, data, expires.HasValue ? expires.Value : TimeSpan.Zero);
        }
        public static bool Set<T> ( string key, T data, TimeSpan? expires )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            return RedisClient.Set(key, data, expires.HasValue ? expires.Value : TimeSpan.Zero);
        }
        public static bool Add<T> ( string key, T data, TimeSpan? expires )
        {
            if ( !RedisClient.SetNx(key, data) )
            {
                return false;
            }
            else if ( expires.HasValue )
            {
                _ = RedisClient.Expire(key, expires.Value);
            }
            return true;
        }
        public static Task<bool> AddAsync<T> ( string key, T data, TimeSpan? expires )
        {
            return Task.Run<bool>(() =>
            {
                return Add(key, data, expires);
            });
        }

        public static bool Set<T> ( string key, string name, T data )
        {
            return RedisClient.HSet(key, name, data);
        }

        internal static bool Remove ( string key )
        {
            return RedisClient.Del(key) > 0;
        }

        internal static bool Replace<T> ( string key, T data, TimeSpan? expires )
        {
            if ( !RedisClient.Exists(key) )
            {
                return false;
            }
            return Set(key, data, expires);
        }
        internal static Task<bool> ReplaceAsync<T> ( string key, T data, TimeSpan? expires )
        {
            return Task.Run(() =>
            {
                if ( !RedisClient.Exists(key) )
                {
                    return Task.FromResult(false);
                }
                return SetAsync(key, data, expires);
            });
        }
    }
}
