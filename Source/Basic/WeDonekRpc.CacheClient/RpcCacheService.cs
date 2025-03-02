using WeDonekRpc.CacheClient.Cache;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Local;
using WeDonekRpc.CacheClient.Memcached;
using WeDonekRpc.CacheClient.Redis;
using WeDonekRpc.CacheClient.RedisCache;
using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public class RpcCacheService
    {
        private static int _VerNum = 0;
        //已初始化的缓存
        private static int _InitCache = 0;

        internal static CacheType DefCacheType
        {
            get;
            private set;
        }
        /// <summary>
        /// 设置缓存版本
        /// </summary>
        /// <param name="num"></param>
        public static void SetVerNum (int num)
        {
            _VerNum = num;
        }
        public static void Load (IIocContainer ioc)
        {
            ioc.RegisterSingle<IMemcachedController>(new MemcachedService());
            ioc.RegisterSingle<IRedisController>(new RedisController());
            ioc.RegisterSingle<IRedisBitController>(new RedisBitController());
            ioc.RegisterSingle<IRedisSubPublic>(new RedisSubPublic());
            ioc.RegisterSingle<IRedisStringController>(new RedisStringController());
            ioc.RegisterSingle<IRedisHashController>(new RedisHashController());
            ioc.RegisterSingle<IRedisSetController>(new RedisSetController());
            ioc.RegisterSingle<IRedisSortedSetController>(new RedisSortedSetController());
            ioc.RegisterSingle<IRedisGeoController>(new RedisGeoController());
            ioc.RegisterSingle<IRedisListController>(new RedisListController());
            ioc.RegisterSingle<ILocalCacheController>(new LocalCacheService());
            ioc.RegisterSingle<ICacheController>(new CacheController());
            ioc.RegisterSingle<ICasMemcachedCache>(new CasMemcachedCache());
        }
        /// <summary>
        /// 初始化缓存服务
        /// </summary>
        /// <param name="config">缓存配置</param>
        /// <param name="defType">默认缓存类型</param>
        public static void Init (CacheConfig config, CacheType defType)
        {
            _SysKey = config.SysKey;
            DefCacheType = defType;
            CacheTimeConfig.Init();
            if (config.Redis != null && ( 4 & _InitCache ) != 4)
            {
                if (RedisCommon.InitCache(config.Redis))
                {
                    _InitCache += 4;
                }
            }
            if (config.Memcached != null && ( 2 & _InitCache ) != 2)
            {
                if (Memcached.MemcachedCache.InitCache(config.Memcached))
                {
                    _InitCache += 2;
                }
            }
        }
        public static bool CheckIsInit (CacheType type)
        {
            if (type == CacheType.Memcached)
            {
                return ( 2 & _InitCache ) == 2;
            }
            else if (type == CacheType.Redis)
            {
                return ( 4 & _InitCache ) == 4;
            }
            return true;
        }


        private static string _SysKey = null;
        internal static string FormatKey (string key)
        {
            return string.Format("{0}_{1}:{2}", key, _SysKey, _VerNum);
        }

        internal static string[] FormatKey (string[] keys)
        {
            return keys.ConvertAll(a => string.Format("{0}_{1}:{2}", a, _SysKey, _VerNum));
        }
        public static void Close ()
        {
            RedisCommon.Close();
            Memcached.MemcachedCache.Close();
            Local.LocalCache.Close();
            CacheTimeConfig.Close();
        }


    }
}
