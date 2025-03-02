using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Enum;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Manage
{
    internal class PageManage
    {

        private static readonly ConcurrentDictionary<uint, PageDetailed> _DataPageList = new ConcurrentDictionary<uint, PageDetailed>();

        private static readonly Timer _Timer = new Timer(new TimerCallback(_CheckTimeout), null, 1000, 1000);
        private static void _CheckTimeout (object state)
        {
            if (_DataPageList.Count == 0)
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime;
            uint[] idList = _DataPageList.Where(a => a.Value.TimeOut <= time).Select(a => a.Key).ToArray();
            if (idList.Length > 0)
            {
                foreach (uint a in idList)
                {
                    if (_DataPageList.TryRemove(a, out PageDetailed page))
                    {
                        if (page.Status == PageStatus.等待发送)
                        {
                            _SendError(page, "socket.send.timeout");
                        }
                        else
                        {
                            _SendError(page, "socket.receive.timeout");
                        }
                    }
                }
            }
        }
        private static void _SendError (PageDetailed page, string error)
        {
            page.Status = PageStatus.回复完成;
            page.Error = error;
            page.IsError = true;
            _PageComplate(page);
        }
        public static bool SubmitPage (uint pageId, ReadOnlySpan<byte> data, byte dataType)
        {
            if (_DataPageList.TryRemove(pageId, out PageDetailed page))
            {
                page.ReturnData = data.ToArray();
                page.DataType = dataType;
                page.Status = PageStatus.回复完成;
                _PageComplate(page);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="stream"></param>
        private static bool _SyncSend (Page page, out byte[] stream, out string error)
        {
            Model.SyntonySet returnSet = new SyntonySet(page.SyncTimeOut);
            PageDetailed obj = _AddPage(returnSet, page);
            if (_SyncSend(page, returnSet))
            {
                if (obj.Status == PageStatus.回复完成)
                {
                    if (obj.IsError)
                    {
                        stream = null;
                        error = obj.Error;
                        return false;
                    }
                    else
                    {
                        error = null;
                        stream = obj.ReturnData;
                        return true;
                    }
                }
                else
                {
                    error = obj.Status == PageStatus.已发送 ? "socket.receive.overtime" : "socket.send.overtime";
                }
            }
            else
            {
                error = "socket.sync.overtime";
            }
            stream = null;
            return false;
        }
        private static void _AsyncSend (Page page, Async async, object arg)
        {
            SyntonySet returnSet = new SyntonySet(async, arg);
            _ = _AddPage(returnSet, page);
            if (!_SendData(page, out string error))
            {
                SendError(page.PageId, error);
            }
        }
        private static bool _SyncSend (Page page, out string error)
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
                else
                {
                    error = obj.Status == PageStatus.已发送 ? "socket.receive.overtime" : "socket.send.overtime";
                }
            }
            else
            {
                error = "socket.sync.overtime";
            }
            return false;
        }



        private static bool _SyncSend (Page page, SyntonySet syncSet)
        {
            using (syncSet)
            {
                if (!_SendData(page, out string error))
                {
                    SendError(page.PageId, error);
                    return true;
                }
                return syncSet.SyncData();
            }
        }


        private static bool _SendData (Page page, out string error)
        {
            if (!ClientManage.GetServer(page.ServerId, out ServerInfo server, out error))
            {
                return false;
            }
            server.SendPage(Page.GetDataPage(page));
            return true;
        }

        public static void SendSuccess (uint pageId)
        {
            if (_DataPageList.TryGetValue(pageId, out PageDetailed page))
            {
                if (( ConstDicConfig.SinglePagType & page.PageType ) == ConstDicConfig.SinglePagType)
                {
                    _Complate(pageId);
                }
                else
                {
                    page.Status = PageStatus.已发送;
                }
            }
        }
        public static void _Complate (uint pageId)
        {
            if (_DataPageList.TryRemove(pageId, out PageDetailed page))
            {
                page.Status = PageStatus.回复完成;
                _PageComplate(page);
            }
        }
        public static bool SubmitPage (uint pageId)
        {
            if (_DataPageList.TryRemove(pageId, out PageDetailed page))
            {
                page.Status = PageStatus.回复完成;
                _PageComplate(page);
                return true;
            }
            return false;
        }
        public static void SendError (uint pageId, string error)
        {
            if (_DataPageList.TryRemove(pageId, out PageDetailed page))
            {
                _SendError(page, error);
            }
        }
        private static void _PageComplate (PageDetailed page)
        {
            if (page.SyntonySet.SyntonyType == SyntonyType.异步 && page.SyntonySet.Async != null)
            {
                IAsyncEvent e = new IAsyncEvent
                {
                    DataType = (SendType)page.DataType,
                    Content = page.ReturnData,
                    Arg = page.SyntonySet.Arg,
                    Error = page.Error
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
        /// <param name="returnSet"></param>
        /// <param name="page"></param>
        /// <param name="returnSet"></param>
        private static PageDetailed _AddPage (SyntonySet returnSet, Page page)
        {
            PageDetailed detailed = new PageDetailed(returnSet, page);
            return _DataPageList.TryAdd(page.PageId, detailed) ? detailed : null;
        }
        private static bool _RegPage (SyntonySet returnSet, Page page)
        {
            PageDetailed detailed = new PageDetailed(returnSet, page);
            return _DataPageList.TryAdd(page.PageId, detailed);
        }
        public static bool Send (Page page, out string error)
        {
            return _SyncSend(page, out error);
        }
        public static void Send (Page page, Async async, object arg)
        {
            _AsyncSend(page, async, arg);
        }
        public static bool Send<Result> (Page page, out Result result, out string error)
        {
            if (_SyncSend(page, out byte[] stream, out error))
            {
                result = ToolsHelper.DeserializeData<Result>(stream);
                return true;
            }
            result = default;
            return false;
        }
        public static bool Send (Page page, out string result, out string error)
        {
            if (_SyncSend(page, out byte[] stream, out error))
            {
                result = ToolsHelper.DeserializeStringData(stream);
                return true;
            }
            result = null;
            return false;
        }
    }
}
