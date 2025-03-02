using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.Environment.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点环境
    /// </summary>
    internal class EnvironmentApi : ApiController
    {
        private readonly IEnvironmentService _Service;

        public EnvironmentApi (IEnvironmentService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务节点环境变量
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public ServerEnvironment Get ([NumValidate("rpc.store.server.id.error", 1)] long serverId)
        {
            return this._Service.Get(serverId);
        }
    }
}
