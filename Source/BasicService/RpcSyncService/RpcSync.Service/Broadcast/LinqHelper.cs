using System.Text;
using WeDonekRpc.Helper;
using RpcSync.Model;
using RpcSync.Service.Node;

namespace RpcSync.Service.Broadcast
{
    internal static class LinqHelper
    {
        public static string[] GetDictate(this MerServer[] servers, List<RootNode> nodes, int? regionId)
        {
            if (servers.Length == 0)
            {
                return null;
            }
            else if (!regionId.HasValue)
            {
                return servers.Convert(a => nodes.Exists(b => b.Id == a.SystemType), a => a.TypeVal);
            }
            else
            {
                return servers.Convert(a => a.RegionId == regionId.Value && nodes.Exists(b => b.Id == a.SystemType), a => a.TypeVal);
            }
        }
        public static long[] GetServer(this MerServer[] servers, List<RootNode> nodes, int? regionId)
        {
            if (servers.Length == 0)
            {
                return null;
            }
            else if (!regionId.HasValue)
            {
                return servers.Convert(a => nodes.Exists(b => b.Id == a.SystemType), a => a.ServerId);
            }
            else
            {
                return servers.Convert(a => a.RegionId == regionId.Value && nodes.Exists(b => b.Id == a.SystemType), a => a.ServerId);
            }
        }
        public static string[] GetAllDictate(this MerServer[] servers, int? regionId)
        {
            if (!regionId.HasValue)
            {
                return servers.ConvertAll(a => a.TypeVal);
            }
            else
            {
                return servers.Convert(a => a.RegionId == regionId, a => a.TypeVal);
            }
        }

        public static long[] GetAllServer(this MerServer[] servers, int? regionId)
        {
            if (!regionId.HasValue)
            {
                return servers.ConvertAll(a => a.ServerId);
            }
            else
            {
                return servers.Convert(a => a.RegionId == regionId, a => a.ServerId);
            }
        }
    }
}
