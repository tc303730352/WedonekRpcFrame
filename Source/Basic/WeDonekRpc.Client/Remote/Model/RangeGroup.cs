using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;

using WeDonekRpc.Model;
namespace WeDonekRpc.Client.Remote.Model
{
    internal class RangeGroup
    {
        private readonly TransmitRange _Range = null;
        private readonly string value = null;
        private readonly IBalanced _Server = null;
        private readonly long _serverId = 0;
        private readonly long[] _ServerId;
        public RangeGroup (TransmitRange range, long[] serverId, BalancedConfig config) : this(serverId, config)
        {
            this._Range = range;
        }
        public RangeGroup (long[] serverId, BalancedConfig config)
        {
            this._ServerId = serverId;
            if (serverId.Length == 1)
            {
                this._serverId = serverId[0];
            }
            else
            {
                this._Server = BalancedCollect.GetBalanced(config.BalancedType, config.ToConfig(serverId));
            }
        }
        public RangeGroup (string value, long[] serverId, BalancedConfig config) : this(serverId, config)
        {
            this.value = value;
        }

        public bool CheckRange (long val)
        {
            return ( this._Range.IsFixed && this._Range.BeginRange == val ) || ( this._Range.BeginRange <= val && this._Range.EndRange > val );
        }
        public bool CheckRange (string val)
        {
            return this.value == val;
        }
        private static bool _DistributeNode (long serverId, IRemoteConfig config, out IRemote remote)
        {
            if (config.FilterServerId == serverId)
            {
                remote = null;
                return false;
            }
            else
            {
                return RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out remote);
            }
        }
        public bool DistributeNode (IRemoteConfig config, out IRemote remote)
        {
            if (this._serverId != 0)
            {
                return _DistributeNode(this._serverId, config, out remote);
            }
            return this._Server.DistributeNode(config, out remote);
        }

        internal IRemoteCursor GetAllNode ()
        {
            if (this._serverId != 0)
            {
                return new RemoteCursor(this._ServerId);
            }
            return this._Server.GetAllNode();
        }
    }
}
