using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;
using RpcStore.Service.Interface;
using GetSysConfig = RpcStore.RemoteModel.SysConfig.GetSysConfig;

namespace RpcStore.Service.Event
{
    internal class SysConfigEvent : IRpcApiService
    {
        private readonly ISysConfigService _Service;
        public SysConfigEvent (ISysConfigService service)
        {
            this._Service = service;
        }
        public BasicSysConfig GetBasicConfig (GetBasicConfig obj)
        {
            return this._Service.FindBasicConfig(obj.GetParam);
        }
        public void SetBasicConfig (SetBasicConfig obj)
        {
            this._Service.SetBasicConfig(obj.Config);
        }
        public void AddSysConfig (AddSysConfig add)
        {
            this._Service.Add(add.Config);
        }

        public void DeleteSysConfig (DeleteSysConfig obj)
        {
            this._Service.Delete(obj.Id);
        }

        public SysConfigDatum GetSysConfig (GetSysConfig obj)
        {
            return this._Service.Get(obj.Id);
        }

        public PagingResult<ConfigQueryData> QuerySysConfig (QuerySysConfig query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetSysConfig (SetSysConfig set)
        {
            this._Service.Set(set.Id, set.ConfigSet);
        }

        public void SetSysConfigIsEnable (SetSysConfigIsEnable set)
        {
            this._Service.SetIsEnable(set.Id, set.IsEnable);
        }
    }
}
