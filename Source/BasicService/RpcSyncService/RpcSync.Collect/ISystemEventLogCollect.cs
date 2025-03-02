using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface ISystemEventLogCollect
    {
        void Adds(SysEventAddLog[] logs);
    }
}