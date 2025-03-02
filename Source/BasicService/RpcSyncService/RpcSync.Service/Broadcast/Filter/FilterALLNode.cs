using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    /// <summary>
    /// 加载区域所有节点
    /// </summary>
    internal class FilterALLNode : IBroadcastFilter
    {
        public string FilterName => "ALL";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return msg.IsCrossGroup && !msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
        }
    }
}
