using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
namespace RpcSync.Service.ConfigService
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ConfigRefreshService : IConfigRefreshService
    {
        private readonly string _CacheKey = "ConfigUpdateTime";
        private const string _RefreshMsgKey = "Rpc_RefreshConfig";
        private readonly ISysConfigCollect _Config;
        private readonly ICacheController _Cache;
        private readonly IRemoteServerGroupCollect _RemoteGroup;
        private readonly IBroadcastService _Broadcast;
        private readonly IIocService _Ioc;
        public ConfigRefreshService (ISysConfigCollect config,
            IBroadcastService broadcast,
            IRemoteServerGroupCollect remoteGroup,
            IIocService ioc,
            ICacheController cache)
        {
            this._RemoteGroup = remoteGroup;
            this._Ioc = ioc;
            this._Broadcast = broadcast;
            this._Cache = cache;
            this._Config = config;
        }

        public void Refresh ()
        {
            ConfigItemToUpdateTime[] items = this._Config.GetToUpdateTime();
            if (items.Length == 0)
            {
                return;
            }
            ConfigItemToUpdateTime[] changes;
            if (this._Cache.TryGet(this._CacheKey, out ConfigItemToUpdateTime[] time))
            {
                changes = this._ToChanges(time, items);
                if (changes.Length == 0)
                {
                    return;
                }
            }
            else
            {
                changes = items;
            }
            _ = this._Cache.Set(this._CacheKey, items);
            ConfigItemToUpdateTime pub = changes.Find(c => c.RpcMerId == 0 && c.RegionId == 0 && c.ServerId == 0 && c.ContainerGroup == 0 && c.SystemType == string.Empty);
            if (pub != null)
            {
                this._Refresh();
            }
            else
            {
                this._RefreshItem(changes);
            }
        }
        private void _RefreshItem (ConfigItemToUpdateTime[] changes)
        {
            List<ConfigItemToUpdateTime> list = new List<ConfigItemToUpdateTime>(changes.Length);
            changes.ForEach(c =>
            {
                if (c.ServerId != 0)
                {
                    this._ServerNode(c);
                    list.Add(c);
                }
                else if (c.ContainerGroup != 0)
                {
                    this._ContainerGroup(c, list);
                }
                else
                {
                    this._Refresh(c, list);
                }
            });
            if (list.Count > 0)
            {
                string[] types = list.Select(c => c.SystemType).Distinct().ToArray();
                this._Config.Refresh(types);
                types.ForEach(c =>
                {
                    this._Send(c, list);
                });
            }
        }
        private void _Send (string type, List<ConfigItemToUpdateTime> items)
        {
            SysConfigItem config = this._Config.GetSysConfig(type);
            string body = new SysConfigRefresh { ConfigMd5 = config.Md5 }.ToJson();
            ConfigItemToUpdateTime sysType = items.Find(c => c.SystemType == type && c.IsSystemType());
            if (sysType != null)
            {
                BroadcastMsg msg = new BroadcastMsg
                {
                    IsExclude = false,
                    RpcMerId = sysType.RpcMerId,
                    RegionId = sysType.RegionId == 0 ? null : sysType.RegionId,
                    IsCrossGroup = sysType.RpcMerId == 0,
                    IsLimitOnly = false,
                    MsgKey = _RefreshMsgKey,
                    MsgBody = body,
                    TypeVal = type == string.Empty ? null : new string[]
                    {
                      sysType.SystemType
                    }
                };
                this._Broadcast.Send(msg, RpcClient.CurrentSource);
            }
            long[] serId = items.Where(c => c.SystemType == type && !c.IsSystemType()).Select(c => c.ServerId).Distinct().ToArray();
            if (serId.Length > 0)
            {
                BroadcastMsg msg = new BroadcastMsg
                {
                    IsExclude = false,
                    IsCrossGroup = true,
                    IsLimitOnly = false,
                    MsgKey = _RefreshMsgKey,
                    MsgBody = body,
                    ServerId = serId
                };
                this._Broadcast.Send(msg, RpcClient.CurrentSource);
            }
        }
        private void _Refresh (ConfigItemToUpdateTime cItem, List<ConfigItemToUpdateTime> list)
        {
            if (cItem.IsSystemType())
            {
                list.Add(cItem);
                return;
            }
            IRemoteServerCollect server = this._Ioc.Resolve<IRemoteServerCollect>();
            RemoteServer[] sers = server.GetAllServers();
            if (sers.Length > 0)
            {
                BasicRemoteServer[] servers = this._FormatServer(cItem, sers);
                if (!servers.IsNull())
                {
                    servers.ForEach(c =>
                    {
                        ConfigItemToUpdateTime item = cItem.ConvertMap<ConfigItemToUpdateTime, ConfigItemToUpdateTime>();
                        item.ServerId = c.Id;
                        item.SystemType = c.SystemTypeVal;
                        item.RegionId = c.RegionId;
                        list.Add(item);
                    });
                }
            }
        }

        private void _Refresh ()
        {
            ISystemTypeCollect systemType = RpcClient.Ioc.Resolve<ISystemTypeCollect>();
            string[] allTypes = systemType.GetSystemTypeVals();
            this._Config.Refresh(allTypes);
            Dictionary<string, SysConfigItem> items = this._Config.GetSysConfig(allTypes);
            items.ForEach((c, item) =>
            {
                BroadcastMsg msg = new BroadcastMsg
                {
                    IsExclude = false,
                    IsCrossGroup = true,
                    IsLimitOnly = false,
                    MsgKey = _RefreshMsgKey,
                    MsgBody = new SysConfigRefresh { ConfigMd5 = item.Md5 }.ToJson(),
                    TypeVal = new string[]
                    {
                        c
                    }
                };
                this._Broadcast.Send(msg, RpcClient.CurrentSource);
            });
        }

        private BasicRemoteServer[] _FormatServer (ConfigItemToUpdateTime cItem, RemoteServer[] sers)
        {
            LinqKit.ExpressionStarter<RemoteServer> filter = LinqKit.PredicateBuilder.New<RemoteServer>();
            if (cItem.RegionId != 0)
            {
                filter = filter.And(a => a.RegionId == cItem.RegionId);
            }
            if (cItem.RpcMerId != 0)
            {
                MerServer[] servers = this._RemoteGroup.GetAllServer(cItem.RpcMerId);
                filter = filter.And(c => servers.IsExists(a => a.ServerId == c.Id));
            }
            if (cItem.VerNum != 0)
            {
                filter = filter.And(a => a.VerNum == cItem.VerNum);
            }
            if (!cItem.SystemType.IsNull())
            {
                long sysId = this._Ioc.Resolve<ISystemTypeCollect>().GetSystemTypeId(cItem.SystemType);
                filter = filter.And(a => a.SystemType == sysId);
            }
            if (filter.IsStarted)
            {
                sers = sers.Where(filter).ToArray();
            }
            if (sers.Length > 0)
            {
                BasicRemoteServer[] list = sers.ConvertMap<RemoteServer, BasicRemoteServer>();
                if (cItem.SystemType.IsNull())
                {
                    Dictionary<long, string> typeVal = this._Ioc.Resolve<ISystemTypeCollect>().GetSystemTypeVals(list.Distinct(c => c.SystemType));
                    list.ForEach(c =>
                    {
                        c.SystemTypeVal = typeVal[c.SystemType];
                    });
                }
                else
                {
                    list.ForEach(c =>
                    {
                        c.SystemTypeVal = cItem.SystemType;
                    });
                }
                return list;
            }
            return null;
        }
        private void _ContainerGroup (ConfigItemToUpdateTime cItem, List<ConfigItemToUpdateTime> list)
        {
            IContrainerCollect contrainer = this._Ioc.Resolve<IContrainerCollect>();
            long[] contId = contrainer.GetIds(cItem.ContainerGroup);
            if (contId.IsNull())
            {
                return;
            }
            IRemoteServerCollect server = this._Ioc.Resolve<IRemoteServerCollect>();
            RemoteServer[] sers = server.GetContainerServer(contId);
            if (sers.Length == 0)
            {
                return;
            }
            BasicRemoteServer[] servers = this._FormatServer(cItem, sers);
            if (!servers.IsNull())
            {
                servers.ForEach(c =>
                {
                    ConfigItemToUpdateTime item = cItem.ConvertMap<ConfigItemToUpdateTime, ConfigItemToUpdateTime>();
                    item.ServerId = c.Id;
                    item.SystemType = c.SystemTypeVal;
                    item.RegionId = c.RegionId;
                    list.Add(item);
                });
            }
        }
        private void _ServerNode (ConfigItemToUpdateTime cItem)
        {
            IRemoteServerCollect server = this._Ioc.Resolve<IRemoteServerCollect>();
            RemoteServerConfig config = server.GetServer(cItem.ServerId);
            cItem.SystemType = config.SystemType;
        }

        private ConfigItemToUpdateTime[] _ToChanges (ConfigItemToUpdateTime[] source, ConfigItemToUpdateTime[] sets)
        {
            List<ConfigItemToUpdateTime> list = new List<ConfigItemToUpdateTime>(sets.Length);
            source.ForEach(c =>
            {
                if (!sets.IsExists(c))
                {
                    list.Add(c);
                }
            });
            sets.ForEach(c =>
            {
                ConfigItemToUpdateTime sour = source.Find(a => a.Equals(c));
                if (sour == null || c.ToUpdateTime > sour.ToUpdateTime)
                {
                    list.Add(c);
                }
            });
            return list.ToArray();
        }

    }
}
