using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 单例
    /// </summary>
    internal class SingleBalanced : IBalanced
    {
        private readonly long[] _ServiceId;
        public SingleBalanced (long serverId)
        {
            this._ServiceId = new long[] { serverId };
            this._ServerId = serverId;

        }
        private readonly long _ServerId = 0;

        public BalancedType BalancedType
        {
            get;
        } = BalancedType.single;
        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServiceId);
        }
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            if (this._ServerId != config.FilterServerId && RemoteServerCollect.GetUsableServer(this._ServerId, config.IsProhibitTrace, out server))
            {
                return true;
            }
            else
            {
                server = null;
                return false;
            }
        }
    }
}
