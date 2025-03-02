using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.RunState.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点运行状态
    /// </summary>
    internal class RunStateApi : ApiController
    {
        private readonly IRunStateService _Service;
        public RunStateApi(IRunStateService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务节点运行状态
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns>运行状态</returns>
        public ServerRunState Get([NumValidate("rpc.store.runstate.serverId.error", 1)] long serverId)
        {
            return this._Service.GetRunState(serverId);
        }

        /// <summary>
        /// 查询服务节点运行状态
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>运行状态</returns>
        public PagingResult<RunState> Query([NullValidate("rpc.store.runstate.param.null")] BasicPage param)
        {
            RunState[] results = this._Service.QueryRunState(param, out int count);
            return new PagingResult<RunState>(count, results);
        }

    }
}
