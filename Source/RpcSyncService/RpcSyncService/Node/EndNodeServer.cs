using System.Collections.Generic;

using RpcSyncService.Model;

namespace RpcSyncService.Node
{
        internal class EndNodeServer : INodeServer
        {
                public EndNodeServer(RootNode node)
                {
                        this._Dictate = node;
                }
                private readonly RootNode _Dictate = null;
                public void Load(List<RootNode> dictates)
                {
                        dictates.Add(this._Dictate);
                }
        }
}
