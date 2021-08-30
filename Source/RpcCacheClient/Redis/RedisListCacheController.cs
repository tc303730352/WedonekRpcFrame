using RpcCacheClient.Interface;

namespace RpcCacheClient.Redis
{
        /// <summary>
        /// redis 集合操作类
        /// </summary>
        public class RedisListCacheController : IRedisListController
        {
                private readonly IRedisCacheController _Redis = null;
                public RedisListCacheController(string name, int db = -1)
                {
                        this._CacheKey = RpcCacheService.FormatKey(name);
                        this._Redis = new RedisCache(db);
                }
                private readonly string _CacheKey = null;

                #region List
                public long Insert<T>(T[] data)
                {
                        return this._Redis.ListInsert<T>(this._CacheKey, data);
                }
                public long Insert<T>(T data)
                {
                        return this._Redis.ListInsert<T>(this._CacheKey, data);
                }
                public long Remove<T>(T data)
                {
                        return this._Redis.ListRemove<T>(this._CacheKey, data);
                }
                public bool Get<T>(int index, int size, out T[] data)
                {
                        return this._Redis.GetList<T>(this._CacheKey, index, size, out data);
                }
                public long Count => this._Redis.GetListCount(this._CacheKey);
                public int AddTop<T>(T data, int top)
                {
                        return this._Redis.AddTopList<T>(this._CacheKey, data, top);
                }
                public int AddTop<T>(T data, int start, int end)
                {
                        return this._Redis.AddTopList<T>(this._CacheKey, data, start, end);
                }

                public T TryUpdate<T>(int index, T data)
                {
                        return this._Redis.TryUpdateList<T>(this._CacheKey, index, data);
                }
                public long Append<T>(T[] data)
                {
                        return this._Redis.ListAppend<T>(this._CacheKey, data);
                }

                public long Append<T>(T data)
                {
                        return this._Redis.ListAppend<T>(this._CacheKey, data);
                }

                public bool Get<T>(int index, out T data)
                {
                        return this._Redis.GetList<T>(this._CacheKey, index, out data);
                }
                public bool Get<T>(out T[] data)
                {
                        return this._Redis.GetList<T>(this._CacheKey, out data);
                }
                #endregion
        }
}
