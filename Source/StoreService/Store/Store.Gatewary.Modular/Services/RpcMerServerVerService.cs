using RpcStore.RemoteModel.ServerVer;
using RpcStore.RemoteModel.ServerVer.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerVer;

namespace Store.Gatewary.Modular.Services
{
    internal class RpcMerServerVerService : IRpcMerServerVerService
    {
        public void SetCurrentVer (ServerVerSet set)
        {
            new SetServerVer
            {
                RpcMerId = set.RpcMerId,
                SystemTypeId = set.SystemTypeId,
                VerNum = set.VerNum,
            }.Send();
        }
        public ServerVerInfo[] GetVerList (long rpcMerId)
        {
            return new GetServerVers
            {
                RpcMerId = rpcMerId
            }.Send();
        }
    }
}
