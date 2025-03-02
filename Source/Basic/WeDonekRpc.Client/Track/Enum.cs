namespace WeDonekRpc.Client.Track
{
    /// <summary>
    /// 链路记录器类型
    /// </summary>
    public enum TraceType
    {
        Local = 0,
        Zipkin = 1,
    }
    /// <summary>
    /// 采样范围
    /// </summary>
    public enum TrackRange
    {
        ALL = 14,
        Gateway = 2,
        RpcMsg = 4,
        RpcQueue = 8
    }
    /// <summary>
    /// 链路深度
    /// </summary>
    public enum TrackDepth
    {
        基本 = 0,
        发起的参数 = 2,
        响应的数据 = 4,
        接收的数据 = 8,
        返回的数据 = 16
    }
    /// <summary>
    /// 链路阶段
    /// </summary>
    public enum TrackStage
    {
        /// <summary>
        /// 客户端发起请求
        /// </summary>
        cs,
        /// <summary>
        ///  服务器接受请求，开始处理
        /// </summary>
        sr,
        /// <summary>
        /// 服务器完成处理，给客户端应答；
        /// </summary>
        ss,
        /// <summary>
        /// 客户端接受应答从服务器
        /// </summary>
        cr
    }
}
