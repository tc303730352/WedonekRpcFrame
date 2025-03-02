namespace WeDonekRpc.IOSendInterface
{
    public enum UpFileProgress : byte
    {
        待上传 = 0,
        上传中 = 1,
        连接断开 = 2,
        已取消 = 3
    }
    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendType : byte
    {
        字符串 = 0,
        对象 = 16,
        字节流 = 32
    }
    /// <summary>
    /// 包加载的进度
    /// </summary>
    public enum PageLoadProgress
    {
        加载中 = 0,
        头部加载完成 = 1,
        加载完成 = 2,
        包待校验 = 3,
        包校验错误 = 4
    }
}
