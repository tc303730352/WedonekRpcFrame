using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using RpcSync.Service.Node;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    [IocName("RootOnly")]
    internal class LoadRootOnlyNode : IInitBroadcast
    {
        private readonly INodeService _NodeService;
        public LoadRootOnlyNode (INodeService nodeService)
        {
            this._NodeService = nodeService;
        }

        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            List<RootNode> nodes = [];
            msg.TypeVal.ForEach(a => this._NodeService.Load(a, nodes));
            if (nodes.Count == 0)
            {
                throw new ErrorException(string.Format("rpc.sync.node.null[key={0}]", msg.TypeVal.Join(',')));
            }
            body.Dictate = nodes.Select(a => a.Dictate).Distinct().ToArray();
        }
    }
}
