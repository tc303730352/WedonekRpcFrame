using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
        public interface IQueueCollect : System.IDisposable
        {
                void Subscribe();
                void BindRoute(string routeKey);
                bool Public(IRemoteBroadcast config, TcpRemoteMsg msg);
                bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, long[] serverId);
                bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, string[] typeVal);
                bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, long rpcMerId, string[] typeVal);
        }
}