namespace RpcCacheClient.Interface
{
        public interface IRedisListController
        {
                int AddTop<T>(T data, int top);
                int AddTop<T>(T data, int start, int end);
                long Append<T>(T data);
                long Append<T>(T[] data);
                bool Get<T>(int index, int size, out T[] data);
                bool Get<T>(int index, out T data);
                bool Get<T>(out T[] data);
                long Count { get; }
                long Insert<T>(T data);
                long Insert<T>(T[] data);
                long Remove<T>(T data);
                T TryUpdate<T>(int index, T data);
        }
}