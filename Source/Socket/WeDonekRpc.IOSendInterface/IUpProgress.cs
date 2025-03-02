namespace WeDonekRpc.IOSendInterface
{
    public interface IUpProgress
    {
        long AlreadyUpNum { get; }
        int ConsumeTime { get; }
        UpFileProgress Progress { get; }
        long UpSpeed { get; }
    }
}