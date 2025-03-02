using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    /// <summary>
    /// 加载集群下的所有节点
    /// </summary>
    internal class FilterALLMerNode : IBroadcastFilter
    {
        public string FilterName => "AllMerNode";

        public bool CheckIsUsable (BroadcastMsg msg)
        {
            return !msg.IsCrossGroup && !msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
        }

    }
}
