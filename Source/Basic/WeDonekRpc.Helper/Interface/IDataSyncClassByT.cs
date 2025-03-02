namespace WeDonekRpc.Helper.Interface
{
    public interface IDataSyncClass<T> : System.IDisposable
    {
        int HeartbeatTime { get; }
        bool IsInit { get; }

        bool Init ( T model );
        void ResetData ( T model );
    }
}