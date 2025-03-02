using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IRpcMerDAL
    {
        string GetMerAppId(long id);
        RpcMer GetRpcMer(string appId);
    }
}