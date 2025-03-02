using RpcStore.RemoteModel.ServerVer.Model;

namespace RpcStore.Service.Interface
{
    public interface IRpcMerServerVerService
    {
        void SetCurrentVer (long rpcMerId, long sysTypeId, int verNum);
        ServerVerInfo[] GetVerList (long rpcMerId);
    }
}