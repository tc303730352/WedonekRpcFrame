using WeDonekRpc.Client.Attr;

namespace RpcSync.Service.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IRollbackTranService
    {
        void AddQueue (long tranId);
    }
}