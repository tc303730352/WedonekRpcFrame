using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast.Filter
{
    /// <summary>
    /// 加载所有根节点
    /// </summary>
    internal class FilterALLOnlyNode : IBroadcastFilter
    {
        public string FilterName => "ALLOnly";

        public bool CheckIsUsable(BroadcastMsg msg)
        {
            return msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
        }
    }
}
