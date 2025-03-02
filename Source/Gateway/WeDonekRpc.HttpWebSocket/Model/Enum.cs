namespace WeDonekRpc.HttpWebSocket.Model
{
    public enum PageType : byte
    {
        Continuation = 0,
        Text = 1,
        Binary = 2,
        ping = 9,
        pong = 10,
        close = 8
    }
    /// <summary>
    /// 包加载的进度
    /// </summary>
    internal enum PageLoadProgress
    {
        加载中 = 0,
        头部加载完成 = 1,
        加载完成 = 2,
        加载发生错误 = 3,
        包体加载完成 = 4
    }
}
