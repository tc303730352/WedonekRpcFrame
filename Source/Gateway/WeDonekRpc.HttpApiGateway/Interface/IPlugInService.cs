using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IPlugInService
    {
        void Add (IHttpPlugIn plugIn);
        void Add (IHttpPlugIn plugIn, int index);
        void Delete (string name);
    }
}