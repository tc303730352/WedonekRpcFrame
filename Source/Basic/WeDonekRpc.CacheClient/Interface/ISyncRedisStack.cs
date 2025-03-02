namespace WeDonekRpc.CacheClient.Interface
{
    public interface ISyncRedisStack<T>
    {
        bool CopyTo(T[] array, int index);
        long GetCount();
        long Push(T item);
        long Push(T[] item);
        bool TryPeek(out T data);
        bool TryPeekRange(int index, int size, out T[] data);

        bool TryPeek(int index, out T data);
        bool TryDequeue(out T data);
    }
}
