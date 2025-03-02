using WeDonekRpc.ModularModel.Visit.Model;

namespace RpcSync.Collect
{
    public interface IDicateVisitCollect
    {
        void AddLog(RpcDictateVisit[] adds, long serverId);
        void AddNode(RpcVisit visit, long serverId);
        void AddNode(RpcVisit[] visits, long serverId);
        void ClearVisit();
    }
}