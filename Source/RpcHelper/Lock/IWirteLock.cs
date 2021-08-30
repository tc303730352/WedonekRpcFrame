namespace RpcHelper
{
        public interface IReadWirteLock : System.IDisposable
        {
                bool GetLock();
        }
}