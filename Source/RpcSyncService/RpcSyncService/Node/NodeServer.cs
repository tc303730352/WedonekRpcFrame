using System.Collections.Generic;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Node
{
        internal class NodeServer : INodeServer
        {
                public NodeServer(SystemType[] types)
                {
                        this._LowerNode = types.ConvertAll(a => new RootNode
                        {
                                Dictate = a.TypeVal,
                                Id = a.Id
                        }); ;
                }
                private readonly RootNode[] _LowerNode = null;

                public void Load(List<RootNode> dictates)
                {
                        dictates.AddRange(this._LowerNode);
                }
        }
}
