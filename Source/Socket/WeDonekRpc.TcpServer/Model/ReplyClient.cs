using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpServer.Model
{
    internal class ReplyClient : IIOClient
    {
        private readonly GetDataPage _Page;
        public ReplyClient (ref GetDataPage arg)
        {
            this._Page = arg;
        }
        public bool ReplyMsg<T> (T msg, out string error)
        {
            GetDataPage data = this._Page;
            Page page = Page.GetReply(ref data, msg);
            return this._Page.Client.Send(page, out error);
        }
    }
}
