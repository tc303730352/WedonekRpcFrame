using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 死信事件
    /// </summary>
    /// <param name="msg">死信消息</param>
    /// <param name="routeKey">路由Key</param>
    /// <param name="msgType">广播消息类型</param>
    public delegate void DeadMsg (DeadQueueMsg msg, string routeKey, BroadcastType msgType);
    /// <summary>
    /// 死信队列服务
    /// </summary>
    public interface IDeadQueueCollect
    {
        /// <summary>
        /// 初始化死信队列
        /// </summary>
        /// <returns></returns>
        bool InitQueue ();
        /// <summary>
        /// 绑定队列中的死信
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="callback">死信回调事件</param>
        void BindQueue (string queue, DeadMsg callback);
    }
}