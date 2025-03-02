namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 雪花标识服务
    /// </summary>
    public interface IIdentityService
    {
        long CreateId();
    }
}