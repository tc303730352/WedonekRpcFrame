using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IServerGroupService
    {
        /// <summary>
        /// 添加服务组
        /// </summary>
        /// <param name="group">服务组资料</param>
        /// <returns></returns>
        long AddServerGroup(ServerGroupAdd group);

        /// <summary>
        /// 检查服务组类型值是否重复
        /// </summary>
        /// <param name="typeVal">类别值</param>
        void CheckGroupTypeVal(string typeVal);

        /// <summary>
        /// 删除服务组
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteServerGroup(long id);

        /// <summary>
        /// 获取所有服务组别
        /// </summary>
        /// <returns>服务组</returns>
        ServerGroupItem[] GetAllServerGroup();

        ServerGroupList[] GetGroupAndType (RpcServerType? serverType);

        /// <summary>
        /// 获取服务组信息
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>服务组</returns>
        ServerGroupDatum GetServerGroup(long id);

        /// <summary>
        /// 修改服务组名字
        /// </summary>
        /// <param name="id">服务组ID</param>
        /// <param name="name">名字</param>
        void SetServerGroup(long id, string name);

    }
}
