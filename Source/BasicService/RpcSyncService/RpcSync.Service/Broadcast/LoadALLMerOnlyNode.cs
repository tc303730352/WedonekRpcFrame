using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    /// <summary>
    /// 加载当前集群下的所有指令集
    /// </summary>
    [IocName("ALLMerOnly")]
    internal class LoadALLMerOnlyNode : IInitBroadcast
    {
        private readonly IRemoteServerGroupCollect _Server;
        public LoadALLMerOnlyNode (IRemoteServerGroupCollect server)
        {
            this._Server = server;
        }

        public bool CheckIsUsable (BroadcastMsg msg)
        {
            return !msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
        }

        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            long rpcMerId = msg.RpcMerId.HasValue ? msg.RpcMerId.Value : source.RpcMerId;
            MerServer[] servers = this._Server.GetAllServer(rpcMerId);
            body.RpcMerId = rpcMerId;
            body.Dictate = servers.GetAllDictate(msg.RegionId);
        }
    }
}
