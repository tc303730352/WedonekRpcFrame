using HttpApiGateway.Model;

using RpcModel;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务节点
        /// </summary>
        internal class ServerController : HttpApiGateway.ApiController
        {
                private readonly IServerService _Server = null;
                public ServerController(IServerService service)
                {
                        this._Server = service;
                }
                /// <summary>
                /// 设置服务状态
                /// </summary>
                /// <param name="id"></param>
                /// <param name="state"></param>
                public void SetState(
                        [NullValidate("rpc.server.id.null")] long id,
                        [EnumValidate("rpc.server.state.error",typeof(RpcServiceState))]
                        RpcServiceState state)
                {
                        this._Server.SetServiceState(id, state);
                }
                /// <summary>
                /// 添加节点
                /// </summary>
                /// <param name="add"></param>
                /// <returns></returns>
                public long Add(ServerConfigAddParam add)
                {
                        return this._Server.Add(add);
                }
                /// <summary>
                /// 删除节点
                /// </summary>
                /// <param name="id"></param>
                public void Drop(
                          [NullValidate("rpc.server.id.null")]
                long id)
                {
                        this._Server.Drop(id);
                }
                /// <summary>
                /// 获取节点信息
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public RemoteServerDatum Get(
                           [NullValidate("rpc.server.id.null")]
                        long id)
                {
                        return this._Server.Get(id);
                }
                /// <summary>
                /// 查询节点
                /// </summary>
                /// <param name="query">查询参数</param>
                /// <param name="count">节点总数</param>
                /// <returns>服务节点资料</returns>
                public RemoteServer[] Query(PagingParam<QueryServiceParam> query, out long count)
                {
                        return this._Server.Query(query.Param, query.ToBasicPaging(), out count);
                }
                /// <summary>
                /// 修改节点资料
                /// </summary>
                /// <param name="id">节点Id</param>
                /// <param name="set">节点配置</param>
                public void Set(
                        [NullValidate("rpc.server.id.null")]
                long id, ServerConfigSetParam set)
                {
                        this._Server.SetService(id, set);
                }
        }
}
