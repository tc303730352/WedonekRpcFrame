using RpcStore.RemoteModel.SysConfig.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.SysConfig;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 系统配置管理
    /// </summary>
    internal class SysConfigApi : ApiController
    {
        private readonly ISysConfigService _Service;
        public SysConfigApi (ISysConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="config">配置资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public void Add ([NullValidate("rpc.store.sysconfig.config.null")] SysConfigAdd config)
        {
            this._Service.AddSysConfig(config);
        }
        /// <summary>
        /// 获取基础配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BasicSysConfig GetBasicConfig ([NullValidate("rpc.store.sysconfig.get.param.null")] BasicGetParam param)
        {
            return this._Service.GetBasicConfig(param);
        }
        /// <summary>
        /// 设置通用配置
        /// </summary>
        /// <param name="obj"></param>
        [ApiPrower("rpc.store.admin")]
        public void SetBasicConfig ([NullValidate("rpc.store.sysconfig.config.null")] BasicConfigSet obj)
        {
            this._Service.SetBasicConfig(obj);
        }
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.sysconfig.id.error", 1)] long id)
        {
            this._Service.DeleteSysConfig(id);
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="id">配置ID</param>
        /// <returns>配置资料</returns>
        public SysConfigDatum Get ([NumValidate("rpc.store.sysconfig.id.error", 1)] long id)
        {
            SysConfigDatum datum = this._Service.GetSysConfig(id);
            if (datum.SystemType == "sys.store" && ( datum.Name == "admin" || datum.Name == "user" ) && !base.UserState.CheckPrower("rpc.store.admin"))
            {
                dynamic obj = datum.Value.Json<dynamic>();
                obj["Pwd"] = string.Empty;
                datum.Value = JsonTools.Json(obj);
            }
            return datum;
        }

        /// <summary>
        /// 查询系统配置
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ConfigQueryData> Query ([NullValidate("rpc.store.sysconfig.param.null")] UI_QuerySysConfig param)
        {
            ConfigQueryData[] results = this._Service.QuerySysConfig(param.Query, param, out int count);
            return new PagingResult<ConfigQueryData>(count, results);
        }

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.sysconfig.param.null")] UI_SetSysConfig param)
        {
            this._Service.SetSysConfig(param.Id, param.ConfigSet);
        }

        /// <summary>
        /// 设置配置的启用状态
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetIsEnable ([NullValidate("rpc.store.sysconfig.param.null")] UI_SetSysConfigIsEnable param)
        {
            this._Service.SetSysConfigIsEnable(param.Id, param.IsEnable);
        }

    }
}
