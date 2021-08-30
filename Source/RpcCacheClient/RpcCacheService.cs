using RpcCacheClient.Cache;
using RpcCacheClient.Config;
using RpcCacheClient.Interface;
using RpcCacheClient.Redis;
namespace RpcCacheClient
{
        /// <summary>
        /// 缓存服务
        /// </summary>
        public class RpcCacheService
        {
                private static int _VerNum = 0;
                //已初始化的缓存
                private static int _InitCache = 0;
                private static int _RedisDb = 0;

                internal static CacheType DefCacheType
                {
                        get;
                        private set;
                }
                internal static int RedisDb => _RedisDb;

                /// <summary>
                /// 设置缓存版本
                /// </summary>
                /// <param name="num"></param>
                public static void SetVerNum(int num)
                {
                        _VerNum = num;
                }
                /// <summary>
                /// 初始化缓存服务
                /// </summary>
                /// <param name="sysKey">缓存隔离键</param>
                /// <param name="memcached">Memcached服务地址</param>
                /// <param name="redis">redis服务地址</param>
                public static void Init(CacheConfig config, CacheType defType)
                {
                        _SysKey = config.SysKey;
                        DefCacheType = defType;
                        if (config.Redis != null && (4 & _InitCache) != 4)
                        {
                                _RedisDb = config.Redis.DefaultDatabase;
                                if (RedisHelper.InitCache(config.Redis))
                                {
                                        _InitCache += 4;
                                }
                        }
                        if (config.Memcached != null && (2 & _InitCache) != 2)
                        {
                                if (Memcached.MemcachedCache.InitCache(config.Memcached))
                                {
                                        _InitCache += 2;
                                }
                        }
                }
                /// <summary>
                /// 获取Memcached缓存 cas模式
                /// </summary>
                /// <param name="isCasModel">是否采用CAS模式</param>
                /// <returns></returns>
                public static ICacheController GetCache(bool isCasModel)
                {
                        return new CacheController(isCasModel);
                }

                private static string _SysKey = null;
                internal static string FormatKey(string key)
                {
                        return string.Join("_", key, _SysKey, _VerNum);
                }
                internal static string FormatSub(string key)
                {
                        return string.Concat(key, "_", _SysKey);
                }
                public static ICacheController GetCache(CacheType type)
                {
                        return GetCache(type, _RedisDb);
                }
                public static ICacheController GetCache(CacheType type, int db = -1)
                {
                        if (type == CacheType.Memcached && (2 & _InitCache) != 2)
                        {
                                return null;
                        }
                        else if (type == CacheType.Redis && (4 & _InitCache) != 4)
                        {
                                return null;
                        }
                        return new CacheController(type, db);
                }
                public static IRedisCacheController GetRedis()
                {
                        return new RedisCacheController(_RedisDb);
                }
                public static IRedisCacheController GetRedis(int db)
                {
                        return new RedisCacheController(db);
                }
                public static void Close()
                {
                        RedisHelper.Close();
                        Memcached.MemcachedCache.Close();
                        Local.LocalCache.Close();
                }
        }
}
