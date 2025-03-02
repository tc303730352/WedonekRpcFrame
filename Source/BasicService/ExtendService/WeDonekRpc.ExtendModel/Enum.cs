namespace WeDonekRpc.ExtendModel
{
    public enum StageType
    {
        Send = 0,
        Answer = 1
    }
    public enum AutoRetryTaskStatus
    {
        待重试 = 0,
        已重试成功 = 1,
        已重试失败 = 2,
        已取消 = 3
    }
}
