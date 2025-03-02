using WeDonekRpc.ModularModel.Resource.Model;

namespace RpcSync.Collect
{
    public interface IResourceCollect
    {
        void Clear ();
        void Invalid ();
        void Sync (long modularId, int ver, ResourceDatum[] lists);
    }
}