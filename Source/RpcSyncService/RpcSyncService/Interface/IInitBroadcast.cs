using RpcModel;

using RpcSyncService.Model;

namespace RpcSyncService
{
        /// <summary>
        /// 广播节点加载接口
        /// </summary>
        internal interface IInitBroadcast
        {
                /// <summary>
                /// 检查是否适用
                /// </summary>
                /// <param name="msg"></param>
                /// <returns></returns>
                bool CheckIsUsable(BroadcastMsg msg);
                /// <summary>
                /// 初始化广播体
                /// </summary>
                /// <param name="msg">广播信息</param>
                /// <param name="source">发送原</param>
                /// <param name="body">广播消息体</param>
                /// <param name="error">错误信息</param>
                /// <returns>是否成功</returns>
                bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error);
        }
}