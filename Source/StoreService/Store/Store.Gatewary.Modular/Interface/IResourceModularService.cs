using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IResourceModularService
    {
        /// <summary>
        /// 获取资源模块
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <param name="systemType">节点系统类型</param>
        /// <returns>资源模块</returns>
        BasicModular[] GetBasicModular(long rpcMerId, string systemType);

        /// <summary>
        /// 查询资源模块
        /// </summary>
        /// <param name="query">模块查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ResourceModularDatum[] QueryModular(ModularQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 设置资源模块备注信息
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="remark">备注</param>
        void SetModularRemark(long id, string remark);

    }
}
