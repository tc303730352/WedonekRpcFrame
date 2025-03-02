using WeDonekRpc.Client.Attr;

namespace RpcSync.Service.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface ISingleTranService
    {
        void AddQueue (long[] tranId);
    }
}