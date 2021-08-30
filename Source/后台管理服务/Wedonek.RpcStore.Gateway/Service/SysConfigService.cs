using HttpApiGateway.Model;

using RpcClient;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Gateway.Service
{
        public class SysConfigService : ISysConfigService
        {
                private readonly ISysConfigCollect _Config = null;
                private readonly IServerTypeCollect _ServerType = null;
                private readonly IServerCollect _Server = null;
                public SysConfigService(ISysConfigCollect config, IServerTypeCollect type, IServerCollect server)
                {
                        this._ServerType = type;
                        this._Config = config;
                        this._Server = server;
                }

                public long Add(SysConfigAddParam add)
                {
                        if (add.ValueType == SysConfigValueType.JSON)
                        {
                                add.Value = Tools.CompressJson(add.Value);
                        }
                        return this._Config.AddSysConfig(add);
                }

                public void Drop(long id)
                {
                        this._Config.DropConfig(id);
                }

                public SysConfigDatum Get(long id)
                {
                        return this._Config.GetSysConfig(id);
                }

                public SysConfigInfo[] Query(PagingParam<QuerySysParam> query, out long count)
                {
                        SysConfigDatum[] configs = this._Config.Query(query.Param, query.ToBasicPaging(), out count);
                        if (configs.Length == 0)
                        {
                                return new SysConfigInfo[0];
                        }
                        ServerType[] types = this._ServerType.GetServiceTypes(configs.Convert(a => a.SystemTypeId != 0, a => a.SystemTypeId));
                        ServerConfigDatum[] servers = this._Server.GetServices(configs.Convert(a => a.ServerId != 0, a => a.ServerId));
                        return configs.ConvertMap<SysConfigDatum, SysConfigInfo>((a, b) =>
                        {
                                if (a.ServerId != 0)
                                {
                                        b.Range = string.Format("限定节点({0})使用", servers.Find(c => c.Id == a.ServerId, c => c.ServerName));
                                }
                                else if (a.SystemTypeId != 0)
                                {
                                        b.Range = string.Format("限定节点类型({0})使用", types.Find(c => c.Id == a.SystemTypeId, c => c.SystemName));
                                }
                                else
                                {
                                        b.Range = "限定集群内";
                                }
                                return b;
                        });
                }

                public void Set(SysConfigSet config)
                {
                        this._Config.SetSysConfig(config.Id, config.ConvertMap<SysConfigSet, SysConfigSetParam>());
                }
        }
}
