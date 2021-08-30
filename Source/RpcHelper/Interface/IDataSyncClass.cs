namespace RpcHelper
{
        public interface IDataSyncClass : System.IDisposable
        {
                bool IsInit { get; }

                string Error { get; }
                int HeartbeatTime { get; set; }
                bool Init();

                void ResetLock();
        }
}