using System.Collections.Concurrent;
using System.Threading;
using WeDonekRpc.TcpServer.Interface;
using WeDonekRpc.TcpServer.Log;
using WeDonekRpc.TcpServer.Model;
using WeDonekRpc.TcpServer.Server;

namespace WeDonekRpc.TcpServer.Manage
{
    internal class ReplyPageManage
    {
        private static readonly ConcurrentDictionary<uint, ReplyPage> _DataPageList = new ConcurrentDictionary<uint, ReplyPage>();
        private static uint _ReplyId = 1;
        private static uint _CreatePageId ()
        {
            uint id = Interlocked.Increment(ref _ReplyId);
            if (id == 0)
            {
                return Interlocked.Increment(ref _ReplyId);
            }
            return id;
        }
        public static void Send (GetDataPage arg, object data)
        {
            if (arg.PageId == 0)
            {
                Page page = Page.GetReplyPage(ref arg, data);
                if (!arg.Client.Send(page, out string error))
                {
                    IoLogSystem.AddReplyError(page, arg.Client, error);
                }
                return;
            }
            ReplyPage reply = new ReplyPage
            {
                client = arg.Client,
                page = Page.GetReplyPage(ref arg, data),
                id = _CreatePageId(),
                retryNum = 0
            };
            if (_DataPageList.TryAdd(reply.id, reply))
            {
                if (!arg.Client.Send(reply))
                {
                    _RetrySend(reply);
                }
            }
            else
            {
                IoLogSystem.AddReplyError(reply, "socket.server.reply.error");
            }
        }
        public static void ComplateSend (uint pageId)
        {
            _ = _DataPageList.TryRemove(pageId, out _);
        }
        public static void SendError (uint pageId)
        {
            if (_DataPageList.TryGetValue(pageId, out ReplyPage page))
            {
                _RetrySend(page);
            }
        }
        private static void _SendError (ReplyPage page, string error)
        {
            if (_DataPageList.TryRemove(page.id, out page))
            {
                IoLogSystem.AddReplyError(page, error);
            }
        }
        private static void _RetrySend (ReplyPage page)
        {
            page.retryNum += 1;
            if (page.retryNum > Config.SocketConfig.ReplyRetryNum)
            {
                _SendError(page, "socket.reply.error");
                return;
            }
            IClient cli = page.client;
            if (TcpServer.GetServer(cli.ServerId, out ServerInfo server))
            {
                IClient client = server.FindClient(cli.BindParam);
                if (client == null)
                {
                    _SendError(page, "socket.client.con.null");
                }
                else if (!client.Send(page))
                {
                    _RetrySend(page);
                }
            }
        }
    }
}
