using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    internal class FilterRootOnlyNode : IBroadcastFilter
    {
        public string FilterName => "RootOnly";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
        }

     
    }
}
