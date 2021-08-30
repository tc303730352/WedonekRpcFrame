using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SocketTcpServer.Enum;
using SocketTcpServer.Interface;
using SocketTcpServer.Model;

using RpcHelper;

namespace SocketTcpServer.Manage
{
        internal class PageManage
        {
                static PageManage()
                {
                        new Timer(new TimerCallback(_CheckTimeout), null, 1000, 1000);
                }
                private static readonly ConcurrentDictionary<int, PageDetailed> _DataPageList = new ConcurrentDictionary<int, PageDetailed>();

                private static void _CheckTimeout(object state)
                {
                        if (_DataPageList.Count == 0)
                        {
                                return;
                        }
                        int time = HeartbeatTimeHelper.HeartbeatTime;
                        int[] idList = _DataPageList.Where(a => a.Value.TimeOut <= time).Select(a => a.Key).ToArray();
                        if (idList.Length > 0)
                        {
                                Parallel.ForEach(idList, a =>
                                {
                                        if (_DataPageList.TryRemove(a, out PageDetailed page))
                                        {
                                                if (page.Status == PageStatus.等待发送)
                                                {
                                                        _SendError(page, "socket.send.overtime");
                                                }
                                                else
                                                {
                                                        _SendError(page, "socket.receive.overtime");
                                                }
                                        }
                                });
                        }
                }
                private static void _SendError(PageDetailed page, string error)
                {
                        page.Status = PageStatus.回复完成;
                        page.Error = error;
                        page.IsError = true;
                        _PageComplate(page);
                }
                public static void SubmitPage(int pageId, byte[] data, SendType dataType)
                {
                        if (_DataPageList.TryRemove(pageId, out PageDetailed page) && page != null)
                        {
                                page.ReturnData = data;
                                page.DataType = dataType;
                                page.Status = PageStatus.回复完成;
                                _PageComplate(page);
                        }
                }
                /// <summary>
                /// 同步发送数据
                /// </summary>
                /// <param name="page"></param>
                /// <param name="returnSet"></param>
                private static bool _SyncSend(Page page, out byte[] stream, out string error)
                {
                        Model.SyntonySet returnSet = new SyntonySet(page.SyncTimeOut);
                        PageDetailed obj = _AddPage(returnSet, page);
                        if (_SyncSend(page, returnSet))
                        {
                                if (obj.Status == PageStatus.回复完成)
                                {
                                        stream = obj.ReturnData;
                                        error = obj.Error;
                                        return !obj.IsError;
                                }
                                else if (obj.Status == PageStatus.已发送)
                                {
                                        error = "socket.receive.overtime";
                                }
                                else
                                {
                                        error = "socket.send.overtime";
                                }
                        }
                        else
                        {
                                error = "socket.sync.overtime";
                        }
                        stream = null;
                        return false;
                }
                private static void _AsyncSend(Page page, Async async, object arg)
                {
                        SyntonySet returnSet = new SyntonySet(async, arg);
                        PageDetailed obj = _AddPage(returnSet, page);
                        ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(_SendData), page);
                }
                private static bool _SyncSend(Page page, out string error)
                {
                        SyntonySet returnSet = new SyntonySet(page.SyncTimeOut);
                        PageDetailed obj = _AddPage(returnSet, page);
                        if (_SyncSend(page, returnSet))
                        {
                                if (obj.Status == PageStatus.回复完成)
                                {
                                        error = obj.Error;
                                        return !obj.IsError;
                                }
                                else if (obj.Status == PageStatus.已发送)
                                {
                                        error = "socket.receive.overtime";
                                }
                                else
                                {
                                        error = "socket.send.overtime";
                                }
                        }
                        else
                        {
                                error = "socket.sync.overtime";
                        }
                        return false;
                }



                private static bool _SyncSend(Page page, SyntonySet syncSet)
                {
                        ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(_SendData), page);
                        return syncSet.SyncData();
                }

                private static void _SendData(object state)
                {
                        Page page = (Page)state;
                        if (!_SendData(page, out string error))
                        {
                                SendError(page.PageId, error);
                        }
                }
                private static bool _SendData(Page page, out string error)
                {
                        if (!SocketTcpServer.GetServer(page.ServerId, out Server.ServerInfo server, out error))
                        {
                                return false;
                        }
                        else if (!server.GetClient(page.ClientId, out IClient client, out error))
                        {
                                return false;
                        }
                        else
                        {
                                return client.Send(page, out error);
                        }
                }

                public static void SendSuccess(int pageId)
                {
                        if (_DataPageList.TryGetValue(pageId, out PageDetailed page))
                        {
                                if ((PageType.单向 & page.PageType) == PageType.单向)
                                {
                                        SubmitPage(pageId);
                                }
                                else
                                {
                                        page.Status = PageStatus.已发送;
                                }
                        }
                }
                public static bool SubmitPage(int pageId)
                {
                        if (_DataPageList.TryRemove(pageId, out PageDetailed page) && page != null)
                        {
                                page.Status = PageStatus.回复完成;
                                _PageComplate(page);
                                return true;
                        }
                        return false;
                }
                public static void SendError(int pageId, string error)
                {
                        if (_DataPageList.TryRemove(pageId, out PageDetailed page))
                        {
                                _SendError(page, error);
                        }
                }
                private static void _PageComplate(PageDetailed page)
                {
                        if (page.SyntonySet == null)
                        {
                                return;
                        }
                        if (page.SyntonySet.SyntonyType == SyntonyType.异步 && page.SyntonySet.Async != null)
                        {
                                IAsyncEvent e = new IAsyncEvent
                                {
                                        AddTime = DateTime.Now,
                                        DataType = page.DataType,
                                        Content = page.ReturnData,
                                        Arg = page.SyntonySet.Arg,
                                        Error = page.Error// error;
                                };
                                page.SyntonySet.Async.Invoke(e);
                        }
                        else if (page.SyntonySet.SyntonyType == SyntonyType.同步)
                        {
                                page.Status = PageStatus.回复完成;
                                page.SyntonySet.DisSync();
                        }
                }

                /// <summary>
                /// 添加一个监视的数据包
                /// </summary>
                /// <param name="pageId"></param>
                /// <param name="page"></param>
                /// <param name="returnSet"></param>
                private static PageDetailed _AddPage(SyntonySet returnSet, Page page)
                {
                        PageDetailed detailed = new PageDetailed(returnSet, page);
                        if (_DataPageList.TryAdd(page.PageId, detailed))
                        {
                                return detailed;
                        }
                        return null;
                }
                public static bool Send(Page page, out string error)
                {
                        return _SyncSend(page, out error);
                }
                public static void Send(Page page, Async async, object arg)
                {
                        _AsyncSend(page, async, arg);
                }
                public static bool Send<Result>(Page page, out Result result, out string error)
                {
                        if (_SyncSend(page, out byte[] stream, out error))
                        {
                                result = SocketHelper.DeserializeData<Result>(stream);
                                return true;
                        }
                        result = default;
                        return false;
                }
                public static bool Send<Result>(Page page, out string result, out string error)
                {
                        if (_SyncSend(page, out byte[] stream, out error))
                        {
                                result = SocketHelper.DeserializeStringData(stream);
                                return true;
                        }
                        result = null;
                        return false;
                }
        }
}
