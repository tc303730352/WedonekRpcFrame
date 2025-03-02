using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IServerVisitCensusDAL
    {
        void Add(ServerVisitCensusModel add);
        void Adds(ServerVisitCensusModel[] adds);
        bool CheckIsExists(long serverId, string dictate);
        void Clear();
        void Sync(RpcVisitCensus[] list);
    }
}