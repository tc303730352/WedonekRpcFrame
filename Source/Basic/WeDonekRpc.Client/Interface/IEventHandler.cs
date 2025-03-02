namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 本地事件类
    /// </summary>
    /// <typeparam name="T">事件源</typeparam>
    public interface IEventHandler<T>
    {
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="data">事件源</param>
        /// <param name="eventName">事件名称</param>
        void HandleEvent(T data, string eventName);
    }
}
