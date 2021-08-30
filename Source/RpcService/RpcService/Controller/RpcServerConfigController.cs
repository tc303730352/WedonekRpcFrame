using RpcModel;

using RpcService.Collect;
using RpcService.Model;
using RpcService.Model.DAL_Model;

using RpcHelper;
namespace RpcService.Controller
{
        internal class RpcServerConfigController : DataSyncClass
        {
                public RpcServerConfigController(long rpcMerId, long sysTypeId)
                {
                        this.RpcMerId = rpcMerId;
                        this.SystemTypeId = sysTypeId;
                }
                public long RpcMerId
                {
                        get;
                }

                public long SystemTypeId
                {
                        get;
                }
                private RpcServiceConfig[] _Server = null;
                /// <summary>
                /// 是否启用区域隔离
                /// </summary>
                private bool _IsRegionIsolate = true;
                /// <summary>
                /// 隔离级别
                /// </summary>
                private bool _IsolateLevel = false;
                protected override bool SyncData()
                {
                        if (!RemoteServerGroupCollect.GetRemoteServer(this.RpcMerId, this.SystemTypeId, out RemoteConfig[] list, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (list.Length == 0)
                        {
                                this.Error = "rpc.server.list.null";
                                return false;
                        }
                        else if (!RpcMerConfigCollect.GetConfig(this.RpcMerId, this.SystemTypeId, out RpcMerConfig config, out error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (!RpcServerCollect.GetRemoteServerConfig(list.ConvertAll(a => a.ServerId), out BasicServer[] servers, out error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else
                        {
                                this._IsolateLevel = config.IsolateLevel;
                                this._IsRegionIsolate = config.IsRegionIsolate;
                                this._Server = servers.ConvertAll(a =>
                                {
                                        return this._GetServerConfig(a, list.Find(b => b.ServerId == a.Id, b => b.Weight));
                                });
                                return true;
                        }
                }
                private RpcServiceConfig _GetServerConfig(BasicServer server, int weight)
                {
                        if (weight == 0)
                        {
                                weight = server.Weight;
                        }
                        RpcServiceConfig config = new RpcServiceConfig
                        {
                                RegionId = server.RegionId,
                                ServerId = server.Id,
                                Weight = weight
                        };
                        if (!server.TransmitConfig.IsNull())
                        {
                                config.IsTransmit = true;
                                config.Transmit = server.TransmitConfig.ConvertAll(a =>
                                 {
                                         a.Range.ForEach(b =>
                                         {
                                                 if (b.BeginRange == b.EndRanage)
                                                 {
                                                         b.IsFixed = true;
                                                 }
                                         });
                                         return new ServerTransmit
                                         {
                                                 Range = a.Range,
                                                 TransmitType = a.TransmitType,
                                                 Value = a.Value,
                                                 TransmitId = a.TransmitId
                                         };
                                 });
                        }
                        return config;
                }
                public ServerConfig[] GetServers(int regionId)
                {
                        return this._Server.ConvertMap<RpcServiceConfig, ServerConfig>(a => a.RegionId == regionId);

                }
                public ServerConfig[] GetServers(int regionId, out ServerConfig[] back)
                {
                        if (!this._IsRegionIsolate)
                        {
                                back = null;
                                return this._Server.ConvertMap<RpcServiceConfig, ServerConfig>();
                        }
                        else if (this._IsolateLevel)
                        {
                                back = null;
                                return this._Server.ConvertMap<RpcServiceConfig, ServerConfig>(a => a.RegionId == regionId);
                        }
                        else
                        {
                                ServerConfig[] locals = this._Server.ConvertMap<RpcServiceConfig, ServerConfig>(a => a.RegionId == regionId);
                                if (locals.Length == 0)
                                {
                                        back = null;
                                        return this._Server.ConvertMap<RpcServiceConfig, ServerConfig>();
                                }
                                back = this._Server.ConvertMap<RpcServiceConfig, ServerConfig>(a => a.RegionId != regionId);
                                return locals;
                        }
                }
        }
}
