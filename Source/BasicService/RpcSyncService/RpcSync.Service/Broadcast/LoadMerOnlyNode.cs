using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Model;
using RpcSync.Service.Interface;
using RpcSync.Service.Node;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    [IocName("MerOnly")]
    internal class LoadMerOnlyNode : IInitBroadcast
    {
        private readonly IRemoteServerGroupCollect _Server;
        private readonly INodeService _NodeService;
        public LoadMerOnlyNode (IRemoteServerGroupCollect server, INodeService nodeService)
        {
            this._NodeService = nodeService;
            this._Server = server;
        }

        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            List<RootNode> nodes = [];
            msg.TypeVal.ForEach(a => this._NodeService.Load(a, nodes));
            if (nodes.Count == 0)
            {
                throw new ErrorException(string.Format("rpc.sync.node.null[key={0}]", msg.TypeVal.Join(',')));
            }
            long rpcMerId = msg.RpcMerId.HasValue ? msg.RpcMerId.Value : source.RpcMerId;
            MerServer[] servers = this._Server.GetAllServer(rpcMerId);
            body.RpcMerId = rpcMerId;
            body.Dictate = servers.GetDictate(nodes, msg.RegionId);
        }
    }
}
