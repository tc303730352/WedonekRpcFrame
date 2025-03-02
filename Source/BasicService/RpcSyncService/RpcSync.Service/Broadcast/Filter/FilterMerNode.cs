using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Collect.Model;
using RpcSync.Service.Broadcast;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    /// <summary>
    /// 加载所有集群下的节点
    /// </summary>
    internal class FilterMerNode : IBroadcastFilter
    {
        public string FilterName => "Mer";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return !msg.IsCrossGroup && msg.IsLimitOnly == false && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
        }

    
    }
}
