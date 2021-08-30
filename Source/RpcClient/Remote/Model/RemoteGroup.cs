using System.Collections.Generic;
using System.Linq;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Remote.Model
{
        /// <summary>
        /// 负载均衡的节点组
        /// </summary>
        internal class RemoteGroup
        {
                public TransmitType TransmitType
                {
                        get;
                }
                private readonly RangeGroup[] _Group = null;
                private struct _tempGroup
                {
                        internal long serverId;
                        internal TransmitRange range;
                        internal string value;
                }
                public RemoteGroup(RangeServer[] servers, BalancedConfig config) : this(servers[0].TransmitType, servers, config)
                {
                }
                public RemoteGroup(TransmitType type, RangeServer[] servers, BalancedConfig config)
                {
                        this.TransmitType = type;
                        List<_tempGroup> t = new List<_tempGroup>();
                        servers.ForEach(a =>
                        {
                                a.Range.ForEach(b =>
                                {
                                        t.Add(new _tempGroup
                                        {
                                                serverId = a.ServerId,
                                                range = b,
                                                value = a.Value
                                        });
                                });
                        });
                        if (type == TransmitType.FixedType)
                        {
                                this._Group = t.GroupBy(a => a.value).Select(a => new RangeGroup(a.Key, a.Select(b => b.serverId).ToArray(), config)).ToArray();
                        }
                        else
                        {
                                this._Group = t.GroupBy(a => a.range).Select(a => new RangeGroup(a.Key, a.Select(b => b.serverId).ToArray(), config)).ToArray();
                        }
                }
                public bool RangeFind(IRemoteConfig config, string val, out IRemote remote)
                {
                        RangeGroup group = this._Group.Find(a => a.CheckRange(val));
                        if (group == null)
                        {
                                remote = null;
                                return false;
                        }
                        return group.DistributeNode(config, out remote);
                }
                public bool RangeFind(IRemoteConfig config, long val, out IRemote remote)
                {
                        RangeGroup group = this._Group.Find(a => a.CheckRange(val));
                        if (group == null)
                        {
                                remote = null;
                                return false;
                        }
                        return group.DistributeNode(config, out remote);
                }


        }
}
