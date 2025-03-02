namespace WeDonekRpc.HttpWebSocket.Interface
{
    public interface IApiService
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        ISocketRequest Request { get; }
        /// <summary>
        /// 响应
        /// </summary>
        ISocketResponse Response { get; }
        /// <summary>
        /// 当前会话
        /// </summary>
        IClientSession Session { get; }
        /// <summary>
        /// 关闭客户端链接
        /// </summary>
        /// <param name="time"></param>
        void CloseCon (int time);
        /// <summary>
        /// 关闭客户端链接
        /// </summary>
        void CloseCon ();
    }
}