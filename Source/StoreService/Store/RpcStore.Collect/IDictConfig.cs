using WeDonekRpc.Client.Attr;

namespace RpcStore.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IDictConfig
    {
        /// <summary>
        /// 日志组
        /// </summary>
        string LogGroup { get; }
        /// <summary>
        /// 配置项说明
        /// </summary>
        string ConfigItemShow { get; }
    }
}