
using RpcClient.Attr;

using RpcModularModel.Resource;
using RpcModularModel.Resource.Model;

namespace RpcModular.Service
{
        [ClassLifetimeAttr(ClassLifetimeType.单例)]
        internal class ResourceService : IResourceService
        {
                private static readonly string _Ver = null;
                static ResourceService()
                {
                        _Ver = RpcClient.RpcClient.Config.GetConfigVal("resource:VerNum", "0.0.0");
                }
                public IResourceContainer GetContainer(string name, ResourceType type)
                {
                        ResourceType range = RpcClient.RpcClient.Config.GetConfigVal("resource:UpRange", (ResourceType)6);
                        if ((type & range) == type)
                        {
                                return new ResourceContainer(name, type);
                        }
                        return null;
                }

                public static void Submit(string name, ResourceType type, ResourceDatum[] list)
                {
                        new SyncResource
                        {
                                ResourceType = type,
                                ModularName = name,
                                ModularVer = _Ver,
                                Resources = list
                        }.SyncSend();
                }
        }
}
