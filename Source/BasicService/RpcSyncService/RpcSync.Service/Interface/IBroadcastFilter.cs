using WeDonekRpc.Model;

namespace RpcSync.Service.Interface
{
    internal interface IBroadcastFilter
    {
        string FilterName { get; }
        /// <summary>
        /// 检查是否适用
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CheckIsUsable(BroadcastMsg msg);
    }
}
