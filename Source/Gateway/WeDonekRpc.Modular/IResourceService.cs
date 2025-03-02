
using WeDonekRpc.ModularModel.Resource;

namespace WeDonekRpc.Modular
{
    public interface IResourceService
    {
        IResourceContainer GetContainer (string name, ResourceType type);
    }
}