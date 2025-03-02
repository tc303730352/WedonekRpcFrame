using System.Threading.Tasks;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IAsyncRedisStack<T>
    {
        Task<bool> CopyTo(T[] array, int index);
        Task<long> GetCount();
        Task<long> Push(T item);
        Task<long> Push(T[] item);
        Task<T[]> ToArray();
        Task<T> TryPeek();
        Task<T[]> TryPeekRange(int index, int size);

        Task<T> TryPeek(int index);
        Task<T> TryDequeue();
    }
}