using System;

using RpcClient;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{

        public class RpcMerConfigService : IRpcMerConfigService
        {
                private readonly IRpcMerConfigCollect _MerConfig = null;

                private IServerTypeCollect _SystemType => RpcClient.RpcClient.Unity.Resolve<IServerTypeCollect>();

                public RpcMerConfigService(IRpcMerConfigCollect config)
                {
                        this._MerConfig = config;
                }
                public Guid Add(AddMerConfig add)
                {
                        return this._MerConfig.Add(add);
                }

                public void Drop(Guid id)
                {
                        this._MerConfig.Drop(id);
                }

                public RpcMerConfig GetConfig(Guid id)
                {
                        return this._MerConfig.GetConfig(id);
                }

                public RpcMerConfigDatum[] GetConfigs(long rpcMerId)
                {
                        RpcMerConfig[] configs = this._MerConfig.GetConfigs(rpcMerId);
                        if (configs.Length == 0)
                        {
                                return new RpcMerConfigDatum[0];
                        }
                        ServerType[] types = this._SystemType.GetServiceTypes(configs.ConvertAll(a => a.SystemTypeId));
                        return configs.ConvertMap<RpcMerConfig, RpcMerConfigDatum>((a, b) =>
                        {
                                b.SystemName = types.Find(c => c.Id == a.SystemTypeId, c => c.SystemName);
                                return b;
                        });
                }

                public void Set(Guid id, SetMerConfig param)
                {
                        this._MerConfig.Set(id, param);
                }
        }
}
