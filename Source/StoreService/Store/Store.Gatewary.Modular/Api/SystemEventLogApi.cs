using RpcStore.RemoteModel.SysEventLog.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;
namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 系统事件日志
    /// </summary>
    internal class SystemEventLogApi : ApiController
    {
        private readonly ISystemEventLogService _Service;

        public SystemEventLogApi (ISystemEventLogService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 查询事件日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagingResult<SystemEventLogDto> Query (PagingParam<SysEventLogQuery> param)
        {
            return this._Service.Query(param.Query, param.ToBasicPaging());
        }
        /// <summary>
        /// 获取事件日志详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysEventLogData Get ([NumValidate("rpc.store.event.log.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
    }
}
