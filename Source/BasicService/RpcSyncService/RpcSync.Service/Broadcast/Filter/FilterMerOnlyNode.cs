using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    internal class FilterMerOnlyNode : IBroadcastFilter
    {
        public string FilterName => "MerOnly";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return !msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
        }

    }
}
