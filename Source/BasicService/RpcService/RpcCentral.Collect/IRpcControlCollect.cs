using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect
{
    public interface IRpcControlCollect
    {
        RpcControlServer[] GetControlServer ();
        void Refresh ();
    }
}