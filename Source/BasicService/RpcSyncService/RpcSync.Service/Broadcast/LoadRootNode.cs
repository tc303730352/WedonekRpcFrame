using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    [IocName("Root")]
    internal class LoadRootNode : IInitBroadcast
    {
        private readonly IRemoteServerCollect _Server;
        private readonly INodeService _NodeService;
        public LoadRootNode (IRemoteServerCollect server, INodeService nodeService)
        {
            this._NodeService = nodeService;
            this._Server = server;
        }
        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            List<long> nodes = [];
            msg.TypeVal.ForEach(a => this._NodeService.Load(a, nodes));
            if (nodes.Count == 0)
            {
                throw new ErrorException(string.Format("rpc.sync.node.null[key={0}]", msg.TypeVal.Join(',')));
            }
            body.ServerId = this._Server.GetServerId(msg.RegionId, nodes);
        }
    }
}
