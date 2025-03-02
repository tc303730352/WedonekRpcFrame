
namespace WeDonekRpc.TcpServer.Enum
{
    /// <summary>
    /// 包的状态
    /// </summary>
    internal enum PageStatus : byte
    {
        等待发送 = 0,
        发送中 = 1,
        已发送 = 2,
        接收完成 = 3,
        发送错误 = 4,
        回复完成 = 5
    }

    /// <summary>
    /// 客户端状态
    /// </summary>
    public enum ClientStatus : byte
    {
        未连接 = 0,
        已关闭 = 1,
        已连接 = 2
    }

}
