using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    /// <summary>
    /// 加载当前集群下的所有指令集
    /// </summary>
    internal class FilterALLMerOnlyNode : IBroadcastFilter
    {
        public string FilterName => "ALLMerOnly";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return !msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
        }

    }
}
