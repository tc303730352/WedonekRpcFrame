using WeDonekRpc.Client.Broadcast;
using WeDonekRpc.Client.RpcSysEvent;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    internal class NodeEventHandler
    {
        public static void Init ()
        {
            RemoteSysEvent.AddEvent<ResetServiceState>("Rpc_ResetState", _ResetState);
        }


        /// <summary>
        /// 刷新服务节点状态
        /// </summary>
        /// <param name="state">服务状态</param>
        /// <returns>远程回复消息</returns>
        private static TcpRemoteReply _ResetState (ResetServiceState state, MsgSource source)
        {
            RpcClient.RpcEvent.ServiceState(state.ServiceState);
            if (state.ServiceState == RpcServiceState.下线)
            {
                RpcClient.Close();
            }
            return null;
        }

    }
}
