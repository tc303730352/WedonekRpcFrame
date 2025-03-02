using WeDonekRpc.Model;
using RpcSync.Collect.Model;

namespace RpcSync.Service.Interface
{
    /// <summary>
    /// 广播节点加载接口
    /// </summary>
    public interface IInitBroadcast
    {
        /// <summary>
        /// 初始化广播体
        /// </summary>
        /// <param name="msg">广播信息</param>
        /// <param name="source">发送原</param>
        /// <param name="body">广播消息体</param>
        /// <returns>是否成功</returns>
        void InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body);
    }
}