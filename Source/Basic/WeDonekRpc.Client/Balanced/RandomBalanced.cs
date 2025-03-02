using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 随机
    /// </summary>
    internal class RandomBalanced : IBalanced
    {
        public BalancedType BalancedType
        {
            get;
        } = BalancedType.random;
        public RandomBalanced (long[] server)
        {
            this._ServerList = server;
            this._ServerNum = server.Length;
        }
        /// <summary>
        /// 服务器列表
        /// </summary>
        private readonly long[] _ServerList = System.Array.Empty<long>();

        private readonly int _ServerNum = 0;


        private static bool _CheckIsUsable (long[] servers, int num, bool isCloseTrace, out IRemote server)
        {
            long serverId = num == 1 ? servers[0] : servers[Tools.GetRandom() % num];
            if (serverId == 0)
            {
                server = null;
                return false;
            }
            else if (RemoteServerCollect.GetUsableServer(serverId, isCloseTrace, out server))
            {
                return true;
            }
            else if (servers.Length == 1)
            {
                return false;
            }
            else
            {
                servers = servers.FindAll(a => a != serverId);
                return _CheckIsUsable(servers, servers.Length, isCloseTrace, out server);
            }
        }
        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServerList);
        }
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            long serverId = this._ServerList[Tools.GetRandom() % this._ServerNum];
            if (serverId == 0)
            {
                server = null;
                return false;
            }
            else if (serverId != config.FilterServerId && RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out server))
            {
                return true;
            }
            else
            {
                long[] servers = this._ServerList.FindAll(a => a != serverId && a != config.FilterServerId);
                return _CheckIsUsable(servers, servers.Length, config.IsProhibitTrace, out server);
            }
        }
    }
}