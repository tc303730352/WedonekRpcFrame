namespace WeDonekRpc.Client
{
    /// <summary>
    /// 本地事务总线
    /// </summary>
    public class RpcLocalEvent
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        public void AsyncSend (string name = null)
        {
            RpcClient.LocalEvent.AsyncPublic(this, name);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        public void Send (string name = null)
        {
            RpcClient.LocalEvent.Public(this, name);
        }
    }
}
