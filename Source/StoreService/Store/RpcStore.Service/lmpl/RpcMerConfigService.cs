using WeDonekRpc.Client;

using WeDonekRpc.Helper;
using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMerConfig;
using RpcStore.RemoteModel.MerConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{

    internal class RpcMerConfigService : IRpcMerConfigService
    {
        private readonly IRpcMerConfigCollect _MerConfig;

        private readonly IServerTypeCollect _SystemType;
        private readonly IRpcServerCollect _RpcServer;
        public RpcMerConfigService (IRpcMerConfigCollect config,
            IRpcServerCollect rpcServer,
            IServerTypeCollect serverType)
        {
            this._RpcServer = rpcServer;
            this._SystemType = serverType;
            this._MerConfig = config;
        }

        public void Delete (long id)
        {
            RpcMerConfigModel config = this._MerConfig.GetConfig(id);
            this._MerConfig.Delete(config);
            this._RpcServer.RefreshMerConfig(config.RpcMerId, config.SystemTypeId);
        }

        public RpcMerConfig GetConfig (long rpcMerId, long systemTypeId)
        {
            return this._MerConfig.GetConfig(rpcMerId, systemTypeId);
        }

        public RpcMerConfigDatum[] GetConfigs (long rpcMerId)
        {
            RpcMerConfigModel[] configs = this._MerConfig.GetConfigs(rpcMerId);
            if (configs.IsNull())
            {
                return new RpcMerConfigDatum[0];
            }
            Dictionary<long, string> sysType = this._SystemType.GetNames(configs.Distinct(c => c.SystemTypeId));
            return configs.ConvertMap<RpcMerConfigModel, RpcMerConfigDatum>((a, b) =>
            {
                b.SystemType = sysType.GetValueOrDefault(a.SystemTypeId);
                return b;
            });
        }

        public long Set (MerConfigArg param)
        {
            RpcMerConfig config = this._MerConfig.GetConfig(param.RpcMerId, param.SystemTypeId);
            if (config == null)
            {
                long id = this._MerConfig.Add(param);
                this._RpcServer.RefreshMerConfig(param.RpcMerId, param.SystemTypeId);
                return id;
            }
            else if (this._MerConfig.Set(config, new MerConfigSet
            {
                BalancedType = param.BalancedType,
                IsolateLevel = param.IsolateLevel,
                IsRegionIsolate = param.IsRegionIsolate
            }))
            {
                this._RpcServer.RefreshMerConfig(param.RpcMerId, param.SystemTypeId);
            }
            return config.Id;
        }
    }
}
