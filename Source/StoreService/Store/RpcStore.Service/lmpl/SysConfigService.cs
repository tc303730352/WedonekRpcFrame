using WeDonekRpc.Client;

using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.SysConfig;
using RpcStore.RemoteModel;
using RpcStore.RemoteModel.SysConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class SysConfigService : ISysConfigService
    {
        private readonly ISysConfigCollect _Config;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerRegionCollect _ServerRegion;
        private readonly IContainerGroupCollect _ContainerGroup;
        private readonly IServerCollect _Server;

        public SysConfigService (ISysConfigCollect config,
            IServerTypeCollect serverType,
            IServerRegionCollect serverRegion,
            IContainerGroupCollect containerGroup,
            IServerCollect server)
        {
            this._Config = config;
            this._ServerType = serverType;
            this._ServerRegion = serverRegion;
            this._ContainerGroup = containerGroup;
            this._Server = server;
        }
        public void SetBasicConfig (BasicConfigSet config)
        {
            this._Config.SetBasicConfig(config);
        }
        public BasicSysConfig FindBasicConfig (BasicGetParam obj)
        {
            return this._Config.FindBasicConfig(obj.RpcMerId, obj.Name);
        }

        public void Add (SysConfigAdd add)
        {
            if (add.ValueType == SysConfigValueType.JSON)
            {
                add.Value = Tools.CompressJson(add.Value);
            }
            this._Config.Add(add);
        }

        public void Delete (long id)
        {
            SysConfigModel config = this._Config.Get(id);
            this._Config.Delete(config);
        }

        public SysConfigDatum Get (long id)
        {
            SysConfigModel config = this._Config.Get(id);
            return config.ConvertMap<SysConfigModel, SysConfigDatum>();
        }

        public PagingResult<ConfigQueryData> Query (QuerySysParam query, IBasicPage paging)
        {
            SysConfigBasic[] configs = this._Config.Query(query, paging, out int count);
            if (configs.IsNull())
            {
                return new PagingResult<ConfigQueryData>();
            }
            Dictionary<string, string> types = this._ServerType.GetNames(configs.Distinct(c => !c.SystemType.IsNull(), c => c.SystemType));
            Dictionary<long, string> servers = this._Server.GetNames(configs.Distinct(c => c.ServerId != 0, c => c.ServerId));
            Dictionary<int, string> regions = this._ServerRegion.GetNames(configs.Distinct(c => c.RegionId != 0, c => c.RegionId));
            Dictionary<long, string> container = this._ContainerGroup.GetNames(configs.Distinct(c => c.ContainerGroup != 0, c => c.ContainerGroup));
            ConfigQueryData[] list = configs.ConvertMap<SysConfigBasic, ConfigQueryData>((a, b) =>
             {
                 b.VerNumStr = a.VerNum.FormatVerNum();
                 if (a.IsBasicConfig)
                 {
                     return b;
                 }
                 if (a.ServerId != 0)
                 {
                     b.ServerName = servers.GetValueOrDefault(a.ServerId);
                 }
                 if (a.SystemType.IsNotNull())
                 {
                     b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
                 }
                 if (a.RegionId != 0)
                 {
                     b.Region = regions.GetValueOrDefault(a.RegionId);
                 }
                 if (a.ContainerGroup != 0)
                 {
                     b.ContainerGroupName = container.GetValueOrDefault(a.ContainerGroup);
                 }
                 return b;
             });
            return new PagingResult<ConfigQueryData>(count, list);
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            SysConfigModel config = this._Config.Get(id);
            _ = this._Config.SetIsEnable(config, isEnable);
        }
        public void Set (long id, SysConfigSet set)
        {
            SysConfigModel config = this._Config.Get(id);
            _ = this._Config.Set(config, set);
        }
    }
}
