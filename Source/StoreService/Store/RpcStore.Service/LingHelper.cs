using System.Text;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using WeDonekRpc.Helper;

namespace RpcStore.Service
{
    internal static class LingHelper
    {
        public static string GetEventKey (this EventSwitchSet add, int sysEventId)
        {
            StringBuilder str = new StringBuilder();
            _ = str.Append(sysEventId);
            _ = str.Append(",");
            add.EventConfig.ForEach((a, b) =>
            {
                _ = str.Append(a);
                _ = str.Append(",");
                _ = str.Append(b);
            });
            return str.ToString().GetMd5();
        }
        public static string FormatVerNum (this int verNum)
        {
            string ver = verNum.ToString().PadLeft(7, '0');
            if (ver.Length == 7)
            {
                return ver.Insert(3, ".").Insert(6, ".");
            }
            int index = ver.Length - 7;
            return ver.Insert(index + 3, ".").Insert(index + 6, ".");
        }

    }
}
