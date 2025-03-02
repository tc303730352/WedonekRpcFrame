using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisController : ICacheController
    {
        bool Exists (string key);
        bool SetExpire (string key, TimeSpan time);
    }
}