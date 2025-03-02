using RpcStore.RemoteModel.Error.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Error;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 错误管理
    /// </summary>
    internal class ErrorApi : ApiController
    {
        private readonly IErrorService _Service;
        public ErrorApi (IErrorService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public ErrorDatum Get ([NullValidate("rpc.store.error.code.null")] string code)
        {
            return this._Service.GetError(code);
        }

        /// <summary>
        /// 查询已有错误信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ErrorData> Query ([NullValidate("rpc.store.error.param.null")] UI_QueryError param)
        {
            ErrorData[] results = this._Service.QueryError(param.Query, param, out int count);
            return new PagingResult<ErrorData>(count, results);
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="datum">错误信息</param>
        [ApiPrower("rpc.store.admin")]
        public void SetMsg ([NullValidate("rpc.store.error.datum.null")] ErrorSet datum)
        {
            this._Service.SetErrorMsg(datum);
        }

        /// <summary>
        /// 同步错误信息（有修改，无添加）
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Sync ([NullValidate("rpc.store.error.datum.null")] ErrorDatum datum)
        {
            return this._Service.SyncError(datum);
        }

    }
}
