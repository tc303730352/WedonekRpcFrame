using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    internal class FilterRootNode : IBroadcastFilter
    {
        public string FilterName => "Root";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return msg.IsCrossGroup && msg.IsLimitOnly == false && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
        }

      
    }
}
