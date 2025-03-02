using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerBind;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 集群绑定的服务节点关系管理
    /// </summary>
    internal class ServerBindApi : ApiController
    {
        private readonly IServerBindService _Service;
        public ServerBindApi (IServerBindService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取集群中的容器组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ContainerGroupItem[] GetContainerGroup ([NullValidate("rpc.store.get.param.null")] BindGetParam param)
        {
            return this._Service.GetContainerGroup(param);
        }
        /// <summary>
        /// 删除集群绑定的服务节点关系
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.serverbind.id.error", 1)] long id)
        {
            this._Service.DeleteServerBind(id);
        }
        /// <summary>
        /// 获取服务节点版本
        /// </summary>
        /// <param name="rpcMerId"></param>
        /// <returns></returns>
        public ServerBindVer[] GetServerBindVer ([NumValidate("rpc.store.mer.id.error", 1)] long rpcMerId, bool? isHold)
        {
            return this._Service.GetServerBindVer(rpcMerId, isHold);
        }
        /// <summary>
        /// 获取服务节点项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BindServerItem[] GetItems (ServerBindQueryParam query)
        {
            return this._Service.GetItems(query);
        }
        /// <summary>
        /// 获取拥有的组别和服务类别
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BindServerGroupType[] GetGroupAndType ([NullValidate("rpc.store.get.param.null")] BindGetParam param)
        {
            return this._Service.GetGroupAndType(param);
        }
        /// <summary>
        /// 检查是否绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public long[] CheckIsBind (BindServer param)
        {
            return this._Service.CheckIsBind(param.RpcMerId, param.ServerId);
        }
        /// <summary>
        /// 查询集群绑定的服务节点
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<BindRemoteServer> Query ([NullValidate("rpc.store.serverbind.param.null")] UI_QueryBindServer param)
        {
            BindRemoteServer[] results = this._Service.Query(param.RpcMerId, param.Query, param, out int count);
            return new PagingResult<BindRemoteServer>(count, results);
        }

        /// <summary>
        /// 修改集群绑定的服务节点列表
        /// </summary>
        /// <param name="bind">绑定信息</param>
        [ApiPrower("rpc.store.admin")]
        public void SetBindServer ([NullValidate("rpc.store.serverbind.bind.null")] BindServer bind)
        {
            this._Service.SetBindServer(bind);
        }

        /// <summary>
        /// 设置绑定的服务节点负载均衡时的权重
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SaveWeight (SaveWeight param)
        {
            this._Service.SaveWeight(param);
        }

    }
}
