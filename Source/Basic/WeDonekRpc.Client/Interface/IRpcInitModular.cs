namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 扩展服务模块
    /// </summary>
    public interface IRpcInitModular
    {
        /// <summary>
        /// 启动加载
        /// </summary>
        /// <param name="option"></param>
        void Load (RpcInitOption option);

        /// <summary>
        /// 服务初始化
        /// </summary>
        void Init (IIocService ioc);
        /// <summary>
        /// 初始化结束-开始服务前
        /// </summary>
        /// <param name="ioc"></param>
        /// <param name="service"></param>
        void InitEnd (IIocService ioc, IRpcService service);
    }
}