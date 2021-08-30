using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Service;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 熔断降级
        /// </summary>
        internal class ReduceInRankController : HttpApiGateway.ApiController
        {
                private readonly IReduceInRankService _ReduceInRank = null;
                public ReduceInRankController(IReduceInRankService reduce)
                {
                        this._ReduceInRank = reduce;
                }
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="id"></param>
                public void Drop([NullValidate("rpc.reduce.id.null")] long id)
                {
                        this._ReduceInRank.Drop(id);
                }
                /// <summary>
                /// 获取
                /// </summary>
                /// <param name="rpcMerId"></param>
                /// <param name="serverId"></param>
                /// <returns></returns>
                public ReduceInRankConfig Get([NullValidate("rpc.mer.id.null")] long rpcMerId, [NullValidate("rpc.server.id.null")] long serverId)
                {
                        return this._ReduceInRank.Get(rpcMerId, serverId);
                }

                /// <summary>
                /// 设置或添加
                /// </summary>
                /// <param name="datum"></param>
                public void Sync(AddReduceInRank datum)
                {
                        this._ReduceInRank.Sync(datum);
                }
        }
}
