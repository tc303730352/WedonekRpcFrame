namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IGlobal
    {
        /// <summary>
        /// 服务启动完成
        /// </summary>
        void ServiceStarted ();
        /// <summary>
        /// 服务正在启动
        /// </summary>
        void ServiceStarting ();

        /// <summary>
        /// 服务关闭事件
        /// </summary>
        void ServiceClose ();
        /// <summary>
        /// 服务加载事件
        /// </summary>
        /// <param name="option"></param>
        void Load (IGatewayOption option);
    }
}