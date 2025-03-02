using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.ApiGateway.Interface
{
    public delegate void PlugInStateChange (IPlugIn plugIn);
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IPlugIn
    {
        /// <summary>
        /// 插件名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable { get; }
        /// <summary>
        /// 初始化并注册状态变更事件
        /// </summary>
        /// <param name="change"></param>
        void Init (PlugInStateChange change);

        /// <summary>
        /// 销毁事件
        /// </summary>
        /// <param name="change"></param>
        void Dispose (PlugInStateChange change);
    }
}
