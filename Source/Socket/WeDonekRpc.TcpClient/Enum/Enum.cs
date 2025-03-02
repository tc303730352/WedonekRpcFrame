
namespace WeDonekRpc.TcpClient.Enum
{

    /// <summary>
    /// 包的状态
    /// </summary>
    internal enum PageStatus
    {
        等待发送 = 2,
        已发送 = 4,
        回复完成 = 32
    }

    /// <summary>
    /// 回凋类型
    /// </summary>
    internal enum SyntonyType
    {
        同步,
        异步
    }

    /// <summary>
    /// 客户端状态
    /// </summary>
    internal enum ClientStatus
    {
        未连接 = 0,
        正在连接 = 1,
        等待发送 = 2,
        链接成功 = 3,
        以关闭 = 4
    }

}
