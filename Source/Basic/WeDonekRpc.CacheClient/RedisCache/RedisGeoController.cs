using System;
using CSRedis;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisGeoController : IRedisGeoController
    {
        private readonly CSRedisClient _Client;
        public RedisGeoController ()
        {
            this._Client = RedisCommon.RedisClient;
        }
        public bool Exists (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Exists(key);
        }

        public bool SetExpire (string key, TimeSpan time)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Expire(key, time);
        }

        public string[] Gets (string key, object[] members)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoHash(key, members);
        }
        public (decimal longitude, decimal latitude)?[] Pos (string key, object[] members)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoPos(key, members);
        }
        public decimal? Distance (string key, object member1, object member2, GeoUnit unit)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoDist(key, member1, member2, unit);
        }
        public string[] Radius (string key, decimal lng, decimal lat, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoRadius(key, lng, lat, radius, unit, count, sorting);
        }
        public T[] Radius<T> (string key, decimal lng, decimal lat, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoRadius<T>(key, lng, lat, radius, unit, count, sorting);
        }
        public bool Add<T> (string key, decimal lng, decimal lat, T member)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoAdd(key, lng, lat, member);
        }
        public long Add (string key, params (decimal longitude, decimal latitude, object member)[] adds)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoAdd(key, adds);
        }
        public Result[] RadiusByMember<T, Result> (string key, T member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoRadiusByMember<Result>(key, member, radius, unit, count, sorting);
        }
        public string[] RadiusByMember<T> (string key, T member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GeoRadiusByMember(key, member, radius, unit, count, sorting);
        }
    }
}
