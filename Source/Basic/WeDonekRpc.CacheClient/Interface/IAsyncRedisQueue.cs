using System.Threading.Tasks;
namespace WeDonekRpc.CacheClient.Interface
{
    /// <summary>
    /// 基于异步的先进先出集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncRedisQueue<T>
    {
        Task<bool> CopyTo(T[] array, int index);
        Task<long> Enqueue(T item);
        Task<long> Enqueue(T[] item);
        Task<long> GetCount();
        Task<T[]> ToArray();
        Task<T> TryDequeue();
        Task<T> TryPeek();
        Task<T[]> TryPeekRange(int index, int size);

        Task<T> TryPeek(int index);
    }
}