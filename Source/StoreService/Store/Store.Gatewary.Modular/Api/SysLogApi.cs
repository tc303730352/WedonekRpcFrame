using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.SysLog.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.SysLog;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 系统日志查询
    /// </summary>
    internal class SysLogApi : ApiController
    {
        private readonly ISysLogService _Service;
        public SysLogApi(ISysLogService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取系统日志详细
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>系统日志数据</returns>
        public SystemLogData Get([NumValidate("rpc.store.syslog.id.error", 1)] long id)
        {
            return this._Service.GetSysLog(id);
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<SystemLog> Query([NullValidate("rpc.store.syslog.param.null")] UI_QuerySysLog param)
        {
            SystemLog[] results = this._Service.QuerySysLog(param.Query, param, out int count);
            return new PagingResult<SystemLog>(count, results);
        }

    }
}
