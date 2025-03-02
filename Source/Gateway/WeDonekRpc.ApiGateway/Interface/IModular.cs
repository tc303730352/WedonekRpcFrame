namespace WeDonekRpc.ApiGateway.Interface
{
    /// <summary>
    /// 网关模块
    /// </summary>
    public interface IModular : System.IDisposable
    {
        /// <summary>
        /// 模块服务名
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        /// 说明
        /// </summary>
        string Show { get; }
        /// <summary>
        /// Api路由格式
        /// </summary>
        string ApiRouteFormat { get; }
        /// <summary>
        /// 加载模块中的资源
        /// </summary>
        /// <param name="option"></param>
        void Load(IGatewayOption option);
        /// <summary>
        /// 初始化模块
        /// </summary>
        void InitModular();
        /// <summary>
        /// 开始提供服务
        /// </summary>
        void Start();
    }
}
