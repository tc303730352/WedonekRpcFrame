using RpcStore.RemoteModel.ServerVer.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerVer;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务集群内节点类型版本控制
    /// </summary>
    internal class ServerVerApi : ApiController
    {
        private readonly IRpcMerServerVerService _Service;

        public ServerVerApi (IRpcMerServerVerService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取服务集群中的节点版本信息
        /// </summary>
        /// <param name="rpcMerId"></param>
        /// <returns></returns>
        public ServerVerInfo[] GetVerList ([NullValidate("rpc.store.mer.id.null")] long rpcMerId)
        {
            return this._Service.GetVerList(rpcMerId);
        }
        /// <summary>
        /// 设置正式版本号
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void SetCurrentVer (ServerVerSet set)
        {
            this._Service.SetCurrentVer(set);
        }
    }
}
