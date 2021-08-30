using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;
namespace RpcClient.Remote.Model
{
        internal class RangeGroup
        {
                private readonly TransmitRange _Range = null;
                private readonly string value = null;
                private readonly IBalanced _Server = null;
                private readonly long _serverId = 0;
                public RangeGroup(TransmitRange range, long[] serverId, BalancedConfig config) : this(serverId, config)
                {
                        this._Range = range;
                }
                public RangeGroup(long[] serverId, BalancedConfig config)
                {
                        if (serverId.Length == 1)
                        {
                                this._serverId = serverId[0];
                        }
                        else
                        {
                                this._Server = BalancedCollect.GetBalanced(config.BalancedType, config.ToConfig(serverId));
                        }
                }
                public RangeGroup(string value, long[] serverId, BalancedConfig config) : this(serverId, config)
                {
                        this.value = value;
                }

                public bool CheckRange(long val)
                {
                        return (this._Range.IsFixed && this._Range.BeginRange == val) || (this._Range.BeginRange <= val && this._Range.EndRanage > val);
                }
                public bool CheckRange(string val)
                {
                        return this.value == val;
                }
                private static bool _DistributeNode(long serverId, IRemoteConfig config, out IRemote remote)
                {
                        if (config.FilterServerId == serverId)
                        {
                                remote = null;
                                return false;
                        }
                        else
                        {
                                return RemoteServerCollect.GetRemoteServer(serverId, out remote) && remote.IsUsable;
                        }
                }
                public bool DistributeNode(IRemoteConfig config, out IRemote remote)
                {
                        if (this._serverId != 0)
                        {
                                return _DistributeNode(this._serverId, config, out remote);
                        }
                        return this._Server.DistributeNode(config, out remote);
                }
        }
}
