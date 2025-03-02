using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;
using Store.Gatewary.Modular.Interface;
using GetSysConfig = RpcStore.RemoteModel.SysConfig.GetSysConfig;

namespace Store.Gatewary.Modular.Services
{
    internal class SysConfigService : ISysConfigService
    {
        public void AddSysConfig (SysConfigAdd config)
        {
            new AddSysConfig
            {
                Config = config,
            }.Send();
        }

        public void DeleteSysConfig (long id)
        {
            new DeleteSysConfig
            {
                Id = id,
            }.Send();
        }
        public void SetBasicConfig (BasicConfigSet set)
        {
            new SetBasicConfig
            {
                Config = set
            }.Send();
        }
        public BasicSysConfig GetBasicConfig (BasicGetParam param)
        {
            return new GetBasicConfig
            {
                GetParam = param
            }.Send();
        }
        public SysConfigDatum GetSysConfig (long id)
        {
            return new GetSysConfig
            {
                Id = id,
            }.Send();
        }

        public ConfigQueryData[] QuerySysConfig (QuerySysParam query, IBasicPage paging, out int count)
        {
            return new QuerySysConfig
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetSysConfig (long id, SysConfigSet configSet)
        {
            new SetSysConfig
            {
                Id = id,
                ConfigSet = configSet,
            }.Send();
        }

        public void SetSysConfigIsEnable (long id, bool isEnable)
        {
            new SetSysConfigIsEnable
            {
                Id = id,
                IsEnable = isEnable,
            }.Send();
        }

    }
}
