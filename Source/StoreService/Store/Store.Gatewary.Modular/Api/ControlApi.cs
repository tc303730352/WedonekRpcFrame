using RpcStore.RemoteModel.Control.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Control;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务中心
    /// </summary>
    internal class ControlApi : ApiController
    {
        private readonly IControlService _Service;
        public ControlApi (IControlService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务中心
        /// </summary>
        /// <param name="datum">中控服务资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public int Add ([NullValidate("rpc.store.control.datum.null")] RpcControlDatum datum)
        {
            return this._Service.AddControl(datum);
        }

        /// <summary>
        /// 删除服务中心
        /// </summary>
        /// <param name="id">服务中心ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.control.id.error", 1)] int id)
        {
            this._Service.DeleteControl(id);
        }

        /// <summary>
        /// 获取服务中心信息
        /// </summary>
        /// <param name="id">服务中心ID</param>
        /// <returns></returns>
        public RpcControl Get ([NumValidate("rpc.store.control.id.error", 1)] int id)
        {
            return this._Service.GetControl(id);
        }

        /// <summary>
        /// 查询服务中心
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<RpcControlData> Query ([NullValidate("rpc.store.control.param.null")] BasicPage param)
        {
            RpcControlData[] results = this._Service.QueryControl(param, out int count);
            return new PagingResult<RpcControlData>(count, results);
        }

        /// <summary>
        /// 修改服务中心资料
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.control.param.null")] UI_SetControl param)
        {
            this._Service.SetControl(param.Id, param.Datum);
        }

    }
}
