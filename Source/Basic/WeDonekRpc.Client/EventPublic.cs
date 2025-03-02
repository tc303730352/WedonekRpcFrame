namespace WeDonekRpc.Client
{
    /// <summary>
    /// 本地事件发布
    /// </summary>
    public class EventPublic
    {
        private readonly string _EventName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">事件名</param>
        public EventPublic (string name)
        {
            this._EventName = name;
        }
        public EventPublic ()
        {
            this._EventName = this.GetType().Name;
        }
        /// <summary>
        /// 同步发布
        /// </summary>
        public void Public ()
        {
            RpcClient.LocalEvent.Public(this, this._EventName);
        }
        /// <summary>
        /// 异步发布
        /// </summary>
        public void AsyncPublic ()
        {
            RpcClient.LocalEvent.AsyncPublic(this, this._EventName);
        }
        /// <summary>
        /// 同步发布
        /// </summary>
        public void Public (string name)
        {
            RpcClient.LocalEvent.Public(this, name);
        }
        /// <summary>
        /// 异步发布
        /// </summary>
        public void AsyncPublic (string name)
        {
            RpcClient.LocalEvent.AsyncPublic(this, name);
        }
    }
}
