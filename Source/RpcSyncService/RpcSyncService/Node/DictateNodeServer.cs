using System.Collections.Generic;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Node
{
        internal class DictateNodeServer : INodeServer
        {
                public DictateNodeServer(string[] lower)
                {
                        this._LowerDictate = lower;
                }
                public string[] _LowerDictate = null;
                public void Load(List<RootNode> dictates)
                {
                        this._LowerDictate.ForEach(a => DictateNodeCollect.Load(a, dictates));
                }
        }
}
