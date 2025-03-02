using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface ISystemEventLogDAL
    {
        void Adds(SysEventAddLog[] logs);
    }
}