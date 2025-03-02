using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Resource;

namespace RpcSync.Collect
{
    public interface IResourceModularCollect
    {
        long GetModular(string name, ResourceType type, MsgSource source);
    }
}