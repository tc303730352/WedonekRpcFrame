using HttpApiGateway.Model;

using RpcModel.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务节点指令限流
        /// </summary>
        internal class ServerDictateController : HttpApiGateway.ApiController
        {
                private readonly IServerDictateLimitService _Service = null;
                public ServerDictateController(IServerDictateLimitService service)
                {
                        this._Service = service;
                }
                /// <summary>
                /// 添加限流
                /// </summary>
                /// <param name="add">添加信息</param>
                /// <returns></returns>
                public long Add(AddDictateLimit add)
                {
                        return this._Service.Add(add);
                }
                /// <summary>
                /// 删除限流
                /// </summary>
                /// <param name="id">限流Id</param>
                public void Drop([NullValidate("rpc.dictate.limit.id.null")] long id)
                {
                        this._Service.Drop(id);
                }
                /// <summary>
                /// 获取限流
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public ServerDictateLimitData Get([NullValidate("rpc.dictate.limit.id.null")] long id)
                {
                        return this._Service.Get(id);
                }
                /// <summary>
                /// 查询限流列表
                /// </summary>
                /// <param name="param"></param>
                /// <param name="count"></param>
                /// <returns></returns>
                public ServerDictateLimitData[] Query(PagingParam<DictateQueryParam> param, out long count)
                {
                        return this._Service.Query(param, out count);
                }
                /// <summary>
                /// 设置限流
                /// </summary>
                /// <param name="id"></param>
                /// <param name="limit"></param>
                public void Set([NullValidate("rpc.dictate.limit.id.null")] long id, ServerDictateLimit limit)
                {
                        this._Service.Set(id, limit);
                }
        }
}
