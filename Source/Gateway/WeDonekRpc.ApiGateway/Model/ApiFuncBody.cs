using System;
using System.Reflection;

namespace WeDonekRpc.ApiGateway.Model
{
    public class ApiFuncBody
    {
        /// <summary>
        /// 是否需登陆
        /// </summary>
        public bool IsAccredit { get; set; }
        /// <summary>
        /// 网关类型
        /// </summary>
        public GatewayType GatewayType { get; set; }
        /// <summary>
        /// 方法体
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 访问接口所需权限
        /// </summary>
        public string[] Prower { get; set; }

        /// <summary>
        /// 上传配置
        /// </summary>
        public ApiUpSet UpConfig { get; set; }
        /// <summary>
        /// 请求参数类型
        /// </summary>
        public ApiPostParam[] PostParam { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public ResultBody[] Results { get; set; }


        /// <summary>
        /// 源类型
        /// </summary>
        public Type Source { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUri { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public ApiType ApiType { get; set; }
    }
}
