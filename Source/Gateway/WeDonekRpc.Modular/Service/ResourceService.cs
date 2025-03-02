
using WeDonekRpc.Modular.Config;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.ModularModel.Resource;
using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.Modular.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ResourceService : IResourceService
    {

        public IResourceContainer GetContainer (string name, ResourceType type)
        {
            if (( type & ModularConfig.UpRange ) == type)
            {
                return new ResourceContainer(name, type);
            }
            return null;
        }

        public static void Submit (string name, ResourceType type, ResourceDatum[] list)
        {
            new SyncResource
            {
                ResourceType = type,
                ModularName = name,
                Resources = list
            }.SyncSend();
        }
    }
}
