using System;
using CSRedis;
using WeDonekRpc.CacheClient.Cache;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisController : CacheController, IRedisController
    {
        private CSRedisClient client;
        protected CSRedisClient _Client
        {
            get
            {
                if ( client == null )
                {
                    client = RedisCommon.RedisClient;
                }
                return client;
            }
        }
        public RedisController () : base(Interface.CacheType.Redis)
        {
        }
        public bool Exists ( string key )
        {
            key = RpcCacheService.FormatKey(key);
            return RedisCommon.RedisClient.Exists(key);
        }
        public bool SetExpire ( string key, TimeSpan time )
        {
            key = RpcCacheService.FormatKey(key);
            return RedisCommon.RedisClient.Expire(key, time);
        }
    }
}
