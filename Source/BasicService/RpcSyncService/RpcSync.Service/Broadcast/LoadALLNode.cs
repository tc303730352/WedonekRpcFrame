using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    /// <summary>
    /// 加载区域所有节点
    /// </summary>
    [IocName("ALL")]
    internal class LoadALLNode : IInitBroadcast
    {
        private readonly IRemoteServerCollect _Server;
        public LoadALLNode (IRemoteServerCollect server)
        {
            this._Server = server;
        }
        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            body.ServerId = this._Server.GetAllServer(msg.RegionId);
        }
    }
}
