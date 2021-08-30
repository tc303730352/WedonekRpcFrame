using RpcHelper;
using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// Http配置参数
        /// </summary>
        public class HttpParamConfig
        {
                /// <summary>
                /// 请求的完整URI
                /// </summary>
                [NullValidate("rpc.task.http.config.uri.null")]
                [FormatValidate("rpc.task.http.config.uri.error", ValidateFormat.URL)]
                public string Uri
                {
                        get;
                        set;
                }
                /// <summary>
                /// 请求方式
                /// </summary>
                public string RequestMethod
                {
                        get;
                        set;
                }
                /// <summary>
                /// 请求类型
                /// </summary>
                [EnumValidate("rpc.task.http.config.reg.type.error", typeof(HttpReqType))]
                public HttpReqType ReqType
                {
                        get;
                        set;
                }
                /// <summary>
                /// Post参数
                /// </summary>
                [EntrustValidate("_CheckParam")]
                public string PostParam
                {
                        get;
                        set;
                }
                private static bool _CheckParam(HttpParamConfig obj, out string error)
                {
                        if (obj.RequestMethod == "POST" && string.IsNullOrEmpty(obj.PostParam))
                        {
                                error = "rpc.task.http.post.param.null";
                                return false;
                        }
                        error = null;
                        return true;
                }
        }
}
