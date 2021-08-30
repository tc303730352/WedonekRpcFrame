namespace RpcHelper
{
        public interface IDataSyncClass<T> : System.IDisposable
        {
                int HeartbeatTime { get; }
                bool IsInit { get; }

                bool Init(T model);
                void ResetData(T model);
        }
}