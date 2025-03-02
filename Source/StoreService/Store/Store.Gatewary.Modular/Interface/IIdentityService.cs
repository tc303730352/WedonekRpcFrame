using WeDonekRpc.Model;
using RpcStore.RemoteModel.Identity.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IIdentityService
    {
        /// <summary>
        /// 添加身份标识
        /// </summary>
        /// <param name="datum">身份标识资料</param>
        /// <returns></returns>
        long AddIdentityApp (IdentityDatum datum);

        /// <summary>
        /// 删除身份标识
        /// </summary>
        /// <param name="id">标识ID</param>
        void DeleteIdentityApp (long id);

        /// <summary>
        /// 获取身份标识
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        IdentityAppData GetIdentityApp (long id);

        /// <summary>
        /// 查询身份标识
        /// </summary>
        /// <param name="query">查询信息</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        IdentityApp[] QueryIdentity (IdentityQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改身份标识
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="datum">身份标识资料</param>
        void SetIdentity (long id, IdentityDatum datum);

        /// <summary>
        /// 设置身份标识启用状态
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="isEnable">是否启用</param>
        void SetIdentityIsEnable (long id, bool isEnable);

    }
}
