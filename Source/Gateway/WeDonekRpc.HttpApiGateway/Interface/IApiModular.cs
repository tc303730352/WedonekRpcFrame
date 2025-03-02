using WeDonekRpc.ApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// HttpApi模块
    /// </summary>
    public interface IApiModular : IModular
    {
        /// <summary>
        /// 模块配置
        /// </summary>
        IModularConfig Config { get; }
        /// <summary>
        /// 模块路由
        /// </summary>
        IModularRouteService Route { get; }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool IsInit { get; }
        /// <summary>
        /// 加载路由配置
        /// </summary>
        /// <param name="config"></param>
        void InitRouteConfig(IApiModel config);
    }
}
