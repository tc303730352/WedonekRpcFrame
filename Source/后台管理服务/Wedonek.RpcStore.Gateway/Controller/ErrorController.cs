using HttpApiGateway.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 错误信息
        /// </summary>
        internal class ErrorController : HttpApiGateway.ApiController
        {
                private readonly IErrorService _Service = null;

                public ErrorController(IErrorService service)
                {
                        this._Service = service;
                }
                /// <summary>
                /// 添加错误
                /// </summary>
                /// <param name="add">错误信息</param>
                /// <returns>错误Id</returns>
                public long Add(ErrorAddDatum add)
                {
                        return this._Service.AddError(add);
                }
                /// <summary>
                /// 获取错误信息
                /// </summary>
                /// <param name="errorId">错误Id</param>
                /// <param name="lang">语言</param>
                /// <returns>错误信息</returns>
                public ErrorAddDatum GetError(
                        [NumValidate("rpc.error.id.error",1)]
                        long errorId,
                        [NullValidate("rpc.error.lang.null")]
                       [LenValidate("rpc.error.lang.len",2,20)]
                        [FormatValidate("rpc.error.lang.error", ValidateFormat.纯字母)]
                             string lang)
                {
                        return this._Service.GetError(errorId, lang);
                }
                /// <summary>
                /// 查询错误信息
                /// </summary>
                /// <param name="query">查询参数</param>
                /// <param name="count">错误总量</param>
                /// <returns>错误数据</returns>
                public ErrorData[] Query(PagingParam<ErrorParam> query, out long count)
                {
                        return this._Service.QueryError(query, out count);
                }
                /// <summary>
                /// 设置错误的友好提示
                /// </summary>
                /// <param name="param">参数</param>
                public void SetMsg(SetErrorParam param)
                {
                        this._Service.SetErrorMsg(param);
                }
        }
}
