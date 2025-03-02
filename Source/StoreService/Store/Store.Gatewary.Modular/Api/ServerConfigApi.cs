using RpcStore.RemoteModel.ServerConfig.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerConfig;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点管理
    /// </summary>
    internal class ServerConfigApi : ApiController
    {
        private readonly IServerConfigService _Service;
        public ServerConfigApi (IServerConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增服务节点
        /// </summary>
        /// <param name="datum">节点资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long AddServer ([NullValidate("rpc.store.server.datum.null")] ServerConfigAdd datum)
        {
            return this._Service.AddServer(datum);
        }
        /// <summary>
        /// 获取服务节点资料
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public RemoteServerModel GetServerDatum ([NumValidate("rpc.store.server.serverId.error", 1)] long serverId)
        {
            return this._Service.GetServerDatum(serverId);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        [ApiPrower("rpc.store.admin")]
        public void DeleteServer ([NumValidate("rpc.store.server.serverId.error", 1)] long serverId)
        {
            this._Service.DeleteServer(serverId);
        }

        /// <summary>
        /// 获取服务节点资料
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns></returns>
        public RemoteServerDatum GetServer ([NumValidate("rpc.store.server.serverId.error", 1)] long serverId)
        {
            return this._Service.GetServer(serverId);
        }

        /// <summary>
        /// 查询服务节点
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<RemoteServer> QueryServer ([NullValidate("rpc.store.server.param.null")] UI_QueryServer param)
        {
            RemoteServer[] results = this._Service.QueryServer(param.Query, param, out int count);
            return new PagingResult<RemoteServer>(count, results);
        }
        /// <summary>
        /// 获取服务项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ServerItem[] GetItems ([NullValidate("rpc.store.server.param.null")] ServerConfigQuery query)
        {
            return this._Service.GetItems(query);
        }
        /// <summary>
        /// 修改服务节点资料
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetServer ([NullValidate("rpc.store.server.param.null")] UI_SetServer param)
        {
            this._Service.SetServer(param.ServerId, param.Datum);
        }
        /// <summary>
        /// 设置服务节点版本号
        /// </summary>
        /// <param name="param"></param>
        [ApiPrower("rpc.store.admin")]
        public void SetVerNum ([NullValidate("rpc.store.server.param.null")] LongParam<int> param)
        {
            this._Service.SetVerNum(param.Id, param.Value);
        }
        /// <summary>
        /// 设置服务节点状态
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetServiceState ([NullValidate("rpc.store.server.param.null")] UI_SetServiceState param)
        {
            this._Service.SetServiceState(param.ServiceId, param.State);
        }

    }
}
