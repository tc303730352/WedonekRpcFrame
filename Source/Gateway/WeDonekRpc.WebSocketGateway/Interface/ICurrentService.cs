namespace WeDonekRpc.WebSocketGateway.Interface
{
    /// <summary>
    /// 当前请求
    /// </summary>
    public interface ICurrentService : IApiSocketService
    {
        /// <summary>
        /// 是否有值
        /// </summary>
        bool IsHasValue { get; }
        /// <summary>
        /// 当前模块
        /// </summary>
        ICurrentModular Modular { get; }
    }
}