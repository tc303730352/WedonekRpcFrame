using RpcCentral.Collect.Model;
using RpcCentral.Model.DB;

namespace RpcCentral.Collect
{
    public interface IRpcTokenCollect
    {
        string Apply(RpcMer mer, long serviceId);
        RpcTokenCache Get(string tokenId);
        void SetConServerId(RpcTokenCache token, long serverId);
    }
}