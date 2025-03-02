namespace WeDonekRpc.CacheClient.Interface
{
    public interface ISyncRedisQueue<T>
    {
        bool CopyTo (T[] array, int index);
        long Enqueue (T item);
        long Enqueue (T[] item);
        long GetCount ();
        bool TryDequeue (out T data);
        bool TryPeek (out T[] data);
        bool TryPeekRange (int index, int size, out T[] data);

        bool TryPeek (int index, out T data);
    }
}
