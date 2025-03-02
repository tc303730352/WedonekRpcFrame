using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.FileUp.Collect
{
    internal class FileAccreditCollect
    {
        static FileAccreditCollect ()
        {
            _ = new Timer(new TimerCallback(_CheckTimeout), null, 1000, 1000);
        }
        /// <summary>
        /// 连接的客户端列表
        /// </summary>
        private static readonly ConcurrentDictionary<int, FileAccredit> _AccreditList = new ConcurrentDictionary<int, FileAccredit>();

        private static readonly ConcurrentDictionary<string, int> _FileAccredit = new ConcurrentDictionary<string, int>();

        private static void _CheckTimeout (object state)
        {
            if (_AccreditList.Count == 0)
            {
                return;
            }
            int[] keys = _AccreditList.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime;
            keys.ForEach(a =>
            {
                if (_AccreditList.TryGetValue(a, out FileAccredit accredit))
                {
                    if (accredit.CheckIsOverTime(time))
                    {
                        RemoveAccredit(a);
                    }
                }
            });
        }

        public static FileAccredit ApplyAccredit (UpFile file, IStreamAllot stream)
        {
            if (!_FileAccredit.TryGetValue(stream.FileId, out int pageId))
            {
                return _AddAccredit(file, stream);
            }
            else
            {
                return !_GetAccredit(pageId, out FileAccredit accredit) ? _AddAccredit(file, stream) : accredit;
            }
        }
        private static FileAccredit _AddAccredit (UpFile file, IStreamAllot stream)
        {
            FileAccredit accredit = new FileAccredit(file, stream);
            _ = _AccreditList.TryAdd(accredit.PageId, accredit);
            return accredit;
        }
        private static bool _GetAccredit (int pageId, out FileAccredit accredit)
        {
            if (!_AccreditList.TryGetValue(pageId, out accredit))
            {
                return false;
            }
            else if (accredit.UpFileStatus == UpFileStatus.上传中)
            {
                return true;
            }
            return false;
        }
        public static void BindAccredit (string fileId, int pageId)
        {
            _ = _FileAccredit.TryAdd(fileId, pageId);
        }
        public static void RemoveBind (string fileId)
        {
            _ = _FileAccredit.TryRemove(fileId, out _);
        }

        internal static void WriteStream (byte[] stream)
        {
            int pageId = BitConverter.ToInt32(stream, 0);
            if (_AccreditList.TryGetValue(pageId, out FileAccredit accredit))
            {
                ushort blockId = BitConverter.ToUInt16(stream, 4);
                accredit.WriteStream(blockId, stream);
            }
        }

        internal static void RemoveAccredit (int pageId)
        {
            if (_AccreditList.TryRemove(pageId, out FileAccredit accredit))
            {
                accredit.Dispose();
            }
        }

        internal static bool SyncUpState (int pageId, out FileUpResult result, out string error)
        {
            if (!_AccreditList.TryGetValue(pageId, out FileAccredit accredit))
            {
                result = null;
                error = "socket.up.file.no.accredit";
                return false;
            }
            else
            {
                result = accredit.GetUpResult();
                error = null;
                return true;
            }
        }
    }
}
