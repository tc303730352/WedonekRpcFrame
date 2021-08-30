using RpcHelper;

namespace AutoTaskService.Model
{
        /// <summary>
        /// Http配置参数
        /// </summary>
        internal class HttpParamConfig
        {
                /// <summary>
                /// 请求的完整URI
                /// </summary>
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
                public HttpReqType ReqType
                {
                        get;
                        set;
                }
                /// <summary>
                /// Post参数
                /// </summary>
                public string PostParam
                {
                        get;
                        set;
                }

        }
}
