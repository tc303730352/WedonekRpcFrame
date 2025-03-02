namespace WeDonekRpc.Client.Rabbitmq.Interface
{
    public interface ISubscribe : System.IDisposable
    {
        /// <summary>
        /// 交换机名
        /// </summary>
        string Exchange { get; }
        /// <summary>
        /// 绑定路由
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="routeKey"></param>
        void BindRoute(string queue, string routeKey);
        /// <summary>
        /// 绑定路由
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="routeKey"></param>
        void BindRoute(string queue, string[] routeKey);
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="routeKey"></param>
        void QueueUnbind(string queue, string routeKey);
    }
}