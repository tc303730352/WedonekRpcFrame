using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerType.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IServerTypeService
    {
        /// <summary>
        /// 添加服务类别
        /// </summary>
        /// <param name="datum">类别资料</param>
        /// <returns></returns>
        long AddServerType(ServerTypeAdd datum);

        /// <summary>
        /// 检查类别值是否重复
        /// </summary>
        /// <param name="typeVal">类别值</param>
        void CheckServerTypeVal(string typeVal);

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteServerType(long id);

        /// <summary>
        /// 获取服务类别资料
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>服务类型</returns>
        ServerType GetServerType(long id);

        /// <summary>
        /// 获取服务组下的类别
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>服务类型</returns>
        ServerType[] GetServerTypeByGroup(long groupId);

        /// <summary>
        /// 查询服务类别
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ServerTypeDatum[] QueryServerType(ServerTypeQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改节点类型资料
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="datum">类别资料</param>
        void SetServerType(long id, ServerTypeSet datum);

    }
}
