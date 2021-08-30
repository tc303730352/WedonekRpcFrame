using HttpApiGateway.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 集群服务节点组
        /// </summary>
        internal class RemoteGroupController : HttpApiGateway.ApiController
        {
                private readonly IRemoteGroupService _Group = null;

                public RemoteGroupController(IRemoteGroupService group)
                {
                        this._Group = group;
                }
                /// <summary>
                /// 删除绑定
                /// </summary>
                /// <param name="id"></param>
                public void Drop([NumValidate("rpc.bind.id.null", 1)] long id)
                {
                        this._Group.Drop(id);
                }
                /// <summary>
                /// 获取绑定的服务节点
                /// </summary>
                /// <param name="rpcMerId">集群Id</param>
                /// <returns>服务节点列表</returns>
                public BindRemoteServer[] Get([NumValidate("rpc.mer.id.null", 1)] long rpcMerId)
                {
                        return this._Group.Get(rpcMerId);
                }
                /// <summary>
                /// 绑定服务节点
                /// </summary>
                /// <param name="set">绑定信息</param>
                public void Set(BindServer set)
                {
                        this._Group.SetBindGroup(set);
                }
                /// <summary>
                /// 设置权重
                /// </summary>
                /// <param name="id"></param>
                /// <param name="weight"></param>
                public void SetWeight([NumValidate("rpc.bind.id.null", 1)] long id, [NumValidate("rpc.weight.error", 0)] int weight)
                {
                        this._Group.SetWeight(id, weight);
                }
                /// <summary>
                /// 查询绑定的节点状态
                /// </summary>
                /// <param name="rpcMerId"></param>
                /// <param name="query"></param>
                /// <param name="count"></param>
                /// <returns></returns>
                public RemoteServerBindState[] Query(
                        [NullValidate("rpc.mer.id.null")]
                long rpcMerId, PagingParam<QueryServiceParam> query, out long count)
                {
                        return this._Group.Query(rpcMerId, query, out count);
                }
        }
}
