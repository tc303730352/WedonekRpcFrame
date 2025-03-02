namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisQueue<T>
    {
        long Count { get; }

        bool CopyTo(T[] array, int index);
        void Enqueue(T item);
        void Enqueue(T[] item);
        T[] ToArray();
        bool TryDequeue(out T result);
        bool TryPeek(out T result);
    }
}