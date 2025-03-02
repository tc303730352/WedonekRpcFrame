using System.Collections.Generic;

using WeDonekRpc.TcpServer.FileUp.Collect;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.FileUp
{
    public class FileUpRouteCollect
    {
        private static readonly Dictionary<string, IStreamAllot> _FileAllot = new Dictionary<string, IStreamAllot>();

        public static bool AddAllot(IStreamAllot allot)
        {
            if (_FileAllot.ContainsKey(allot.DirectName))
            {
                return false;
            }
            _FileAllot.Add(allot.DirectName, allot);
            return true;
        }


        internal static bool GetStreamAllot(string name, out IStreamAllot stream)
        {
            if (!_FileAllot.TryGetValue(name, out IStreamAllot allot))
            {
                stream = null;
                return false;
            }
            stream = (IStreamAllot)allot.Clone();
            return true;
        }

        internal static bool FileAccredit(IStreamAllot allot, UpFile file, out FileUpState state, out int pageId, out string error)
        {
            if (!allot.FileAccredit(file, out error))
            {
                allot.Dispose();
                pageId = 0;
                state = null;
                return false;
            }
            FileAccredit accredit = FileAccreditCollect.ApplyAccredit(file, allot);
            if (!accredit.SyncUpState(out state, out error))
            {
                FileAccreditCollect.RemoveAccredit(accredit.PageId);
                pageId = 0;
                return false;
            }
            else
            {
                pageId = accredit.PageId;
                return true;
            }
        }
    }
}
