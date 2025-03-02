using WeDonekRpc.ApiGateway.Model;
using System;

namespace WeDonekRpc.HttpApiGateway.Model
{
    public class RouteSet
    {
        /// <summary>
        /// 是否验证授权
        /// </summary>
        public bool? IsAccredit { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string RoutePath { get; set; }
        /// <summary>
        /// 路径地址是否为正则表达式
        /// </summary>
        public bool IsRegex { get; set; }
        /// <summary>
        /// 所需权限
        /// </summary>
        public string[] Prower { get; set; }
        /// <summary>
        /// Api事件类型
        /// </summary>
        public Type ApiEventType { get; set; }
        /// <summary>
        /// 上传配置
        /// </summary>
        public ApiUpSet UpSet { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnable { get; set; }
    }
}
