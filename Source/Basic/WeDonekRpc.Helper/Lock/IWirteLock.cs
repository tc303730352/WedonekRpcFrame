namespace WeDonekRpc.Helper.Lock
{
    public interface IReadWirteLock : System.IDisposable
    {
        bool GetLock ();
    }
}