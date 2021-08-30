using RpcModel;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务集群
        /// </summary>
        public class RpcMerController : HttpApiGateway.ApiController
        {
                private readonly IRpcMerService _RpcMer = null;
                public RpcMerController(IRpcMerService service)
                {
                        this._RpcMer = service;
                }
                /// <summary>
                /// 添加集群
                /// </summary>
                /// <param name="mer">集群信息</param>
                /// <returns>集群Id</returns>
                public long Add(RpcMerDatum mer)
                {
                        return this._RpcMer.AddMer(mer);
                }
                /// <summary>
                /// 获取集群信息
                /// </summary>
                /// <param name="id">集群Id</param>
                /// <returns>集群信息</returns>
                public RpcMer Get([NullValidate("rpc.mer.id.null")] long id)
                {
                        return this._RpcMer.GetRpcMer(id);
                }

                /// <summary>
                /// 查询集群
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="paging">分页</param>
                /// <param name="count">数据总量</param>
                /// <returns>集群列表</returns>
                public RpcMer[] Query(string name, BasicPage paging, out long count)
                {
                        return this._RpcMer.Query(name, paging, out count);
                }
                /// <summary>
                /// 设置集群信息
                /// </summary>
                /// <param name="id"></param>
                /// <param name="datum"></param>
                public void Set([NullValidate("rpc.mer.id.null")] long id, RpcMerSetParam datum)
                {
                        this._RpcMer.SetMer(id, datum);
                }
                /// <summary>
                /// 删除集群
                /// </summary>
                /// <param name="id">集群Id</param>
                public void Drop([NullValidate("rpc.mer.id.null")] long id)
                {
                        this._RpcMer.DropMer(id);
                }
        }
}
