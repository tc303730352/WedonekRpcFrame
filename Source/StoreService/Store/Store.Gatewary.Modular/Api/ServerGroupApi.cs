using RpcStore.RemoteModel.ServerGroup.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerGroup;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务组管理
    /// </summary>
    internal class ServerGroupApi : ApiController
    {
        private readonly IServerGroupService _Service;
        public ServerGroupApi (IServerGroupService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务组
        /// </summary>
        /// <param name="group">服务组资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.servergroup.group.null")] ServerGroupAdd group)
        {
            return this._Service.AddServerGroup(group);
        }
        /// <summary>
        /// 获取服务组和类别
        /// </summary>
        /// <returns></returns>
        public ServerGroupList[] GetGroupAndType (RpcServerType? serverType)
        {

            return this._Service.GetGroupAndType(serverType);
        }
        /// <summary>
        /// 检查服务组类型值是否重复
        /// </summary>
        /// <param name="typeVal">类别值</param>
        public void CheckGroupTypeVal ([NullValidate("rpc.store.servergroup.typeVal.null")] string typeVal)
        {
            this._Service.CheckGroupTypeVal(typeVal);
        }

        /// <summary>
        /// 删除服务组
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.servergroup.id.error", 1)] long id)
        {
            this._Service.DeleteServerGroup(id);
        }

        /// <summary>
        /// 获取所有服务组别
        /// </summary>
        /// <returns>服务组</returns>
        public ServerGroupItem[] GetAll ()
        {
            return this._Service.GetAllServerGroup();
        }

        /// <summary>
        /// 获取服务组信息
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>服务组</returns>
        public ServerGroupDatum Get ([NumValidate("rpc.store.servergroup.id.error", 1)] long id)
        {
            return this._Service.GetServerGroup(id);
        }

        /// <summary>
        /// 修改服务组名字
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.servergroup.param.null")] UI_SetServerGroup param)
        {
            this._Service.SetServerGroup(param.Id, param.Name);
        }

    }
}
