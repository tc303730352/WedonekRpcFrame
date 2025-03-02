using System;
using WeDonekRpc.ApiGateway.Attr;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IApiConfig
    {
        /// <summary>
        /// 是否需要授权
        /// </summary>
        bool IsAccredit { get; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// 权限
        /// </summary>
        string[] Prower { get; }
        /// <summary>
        /// Controller路由配置
        /// </summary>
        ApiRouteName Route { get; }

        bool? IsEnable { get; }
    }
}
