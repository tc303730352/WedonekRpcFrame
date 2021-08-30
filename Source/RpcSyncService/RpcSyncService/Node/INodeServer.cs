using System.Collections.Generic;

using RpcSyncService.Model;

namespace RpcSyncService.Node
{
        internal interface INodeServer
        {
                void Load(List<RootNode> dictates);
        }
}