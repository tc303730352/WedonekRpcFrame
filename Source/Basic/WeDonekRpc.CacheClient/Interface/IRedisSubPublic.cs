using System;

namespace WeDonekRpc.CacheClient.Interface
{
    /// <summary>
    /// Redis关注和发布
    /// </summary>
    public interface IRedisSubPublic
    {
        bool AddSubscriber<T> ( string name, Action<T> callback );
        long PublicMsg<T> ( string name, T data );
        void Remove ( string name );
    }
}