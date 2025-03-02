using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IBroadcastErrorLogDAL
    {
        void AddErrorLog(BroadcastErrorLogModel[] logs);
    }
}