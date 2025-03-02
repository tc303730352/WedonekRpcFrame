using RpcCentral.Model.DB;

namespace RpcCentral.Collect
{
    public interface IRpcMerCollect
    {
        bool CheckConAccredit(string remoteIp, string appId, out string error);
        RpcMer GetMer(long id);
        bool GetMer(string appid, out RpcMer mer, out string error);
        void Refresh(long merId);
    }
}