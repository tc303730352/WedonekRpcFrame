using SocketTcpServer.Interface;

namespace SocketTcpServer.Model
{
        internal class ReplyClient : ISocketClient
        {
                private readonly GetDataPage _Page;
                public ReplyClient(GetDataPage arg)
                {
                        this._Page = arg;
                }
                public bool ReplyMsg<T>(T msg, out string error)
                {
                        Page page = Page.GetReply(this._Page, msg);
                        return this._Page.Client.Send(page, out error);
                }
        }
}
