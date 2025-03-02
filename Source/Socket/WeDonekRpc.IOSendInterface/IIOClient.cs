namespace WeDonekRpc.IOSendInterface
{
    public interface IIOClient
    {
        bool ReplyMsg<T> (T msg, out string error);
    }
}
