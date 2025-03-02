namespace WeDonekRpc.Helper.Interface
{
    public interface IDataSyncClass : System.IDisposable
    {
        int AddTime { get; }
        int ResetTime { get; }
        bool IsInit { get; }

        string Error { get; }
        int HeartbeatTime { get; set; }
        bool Init ();

        void ResetLock ();
    }
}