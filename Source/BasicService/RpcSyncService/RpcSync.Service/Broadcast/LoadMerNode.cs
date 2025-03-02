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
    /// <summary>
    /// 加载所有集群下的节点
    /// </summary>
    [IocName("Mer")]
    internal class LoadMerNode : IInitBroadcast
    {
        private readonly IRemoteServerGroupCollect _Server;
        private readonly INodeService _NodeService;
        public LoadMerNode (IRemoteServerGroupCollect server, INodeService nodeService)
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
            body.ServerId = servers.GetServer(nodes, msg.RegionId);
        }
    }
}
