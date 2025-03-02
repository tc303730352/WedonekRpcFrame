using RpcStore.RemoteModel.ServerVer.Model;
using Store.Gatewary.Modular.Model.ServerVer;

namespace Store.Gatewary.Modular.Interface
{
    public interface IRpcMerServerVerService
    {
        ServerVerInfo[] GetVerList (long rpcMerId);
        void SetCurrentVer (ServerVerSet set);
    }
}