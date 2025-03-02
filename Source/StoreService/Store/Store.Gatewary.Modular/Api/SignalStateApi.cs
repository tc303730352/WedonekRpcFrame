using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.SignalState.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点和各节点之间的通信状态
    /// </summary>
    internal class SignalStateApi : ApiController
    {
        private readonly ISignalStateService _Service;
        public SignalStateApi(ISignalStateService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务节点和各节点之间的通信状态
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public ServerSignalState[] Get([NumValidate("rpc.store.signalstate.serverId.error", 1)] long serverId)
        {
            return this._Service.GetSignalState(serverId);
        }

    }
}
