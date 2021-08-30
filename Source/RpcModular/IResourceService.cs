
using RpcModularModel.Resource;

namespace RpcModular
{
        public interface IResourceService
        {
                IResourceContainer GetContainer(string name, ResourceType type);
        }
}