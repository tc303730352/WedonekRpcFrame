using System;
using CSRedis;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisGeoController
    {
        long Add (string key, params (decimal longitude, decimal latitude, object member)[] adds);
        bool Add<T> (string key, decimal lng, decimal lat, T member);
        decimal? Distance (string key, object member1, object member2, GeoUnit unit);
        bool Exists (string key);
        string[] Gets (string key, object[] members);
        (decimal longitude, decimal latitude)?[] Pos (string key, object[] members);
        string[] Radius (string key, decimal lng, decimal lat, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        T[] Radius<T> (string key, decimal lng, decimal lat, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        bool SetExpire (string key, TimeSpan time);
        Result[] RadiusByMember<T, Result> (string key, T member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);

        string[] RadiusByMember<T> (string key, T member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
    }
}