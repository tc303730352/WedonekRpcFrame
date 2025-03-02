using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 扩展服务
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IRpcExtendService
    {
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="service"></param>
        void Load(IRpcService service);
    }
}
