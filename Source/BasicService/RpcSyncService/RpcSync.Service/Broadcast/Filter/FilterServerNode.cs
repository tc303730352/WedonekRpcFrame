using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    internal class FilterServerNode : IBroadcastFilter
    {
        public string FilterName => "Server";

        public bool CheckIsUsable (BroadcastMsg msg)
        {
            return !msg.ServerId.IsNull();
        }

    }
}
