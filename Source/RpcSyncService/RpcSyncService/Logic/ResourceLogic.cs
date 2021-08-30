using System;

using RpcModel;

using RpcModularModel.Resource;

using RpcSyncService.Collect;

namespace RpcSyncService.Logic
{
        internal class ResourceLogic
        {

                public static void SyncResource(SyncResource obj, MsgSource source)
                {
                        Guid id = ResourceModularCollect.GetModular(obj.ModularName, obj.ResourceType, source);
                        ResourceCollect.Sync(id, obj.ModularVer, obj.Resources);
                }
        }
}
