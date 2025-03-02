using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Resource;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 资源服务
    /// </summary>
    internal class ResourceEvent : IRpcApiService
    {
        private readonly IResourceCollect _Resource;
        private readonly IResourceModularCollect _Modular;
        public ResourceEvent (IResourceCollect resource,
            IResourceModularCollect modular)
        {
            this._Resource = resource;
            this._Modular = modular;
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void ClearResource ()
        {
            this._Resource.Clear();
        }
        /// <summary>
        /// 检查无效资源
        /// </summary>
        public void InvalidResource ()
        {
            this._Resource.Invalid();
        }
        /// <summary>
        /// 同步资源
        /// </summary>
        /// <param name="obj">参数</param>
        /// <param name="source">来源</param>
        public void SyncResource (SyncResource obj, MsgSource source)
        {
            long id = this._Modular.GetModular(obj.ModularName, obj.ResourceType, source);
            this._Resource.Sync(id, source.VerNum, obj.Resources);
        }
    }
}
