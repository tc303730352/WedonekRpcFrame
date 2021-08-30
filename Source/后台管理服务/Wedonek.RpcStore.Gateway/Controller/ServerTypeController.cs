using HttpApiGateway.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务类目
        /// </summary>
        internal class ServerTypeController : HttpApiGateway.ApiController
        {
                private readonly IServerTypeService _ServerType = null;

                public ServerTypeController(IServerTypeService type)
                {
                        this._ServerType = type;
                }
                /// <summary>
                /// 添加服务类别
                /// </summary>
                /// <param name="add">服务类别资料</param>
                /// <returns>新的类别Id</returns>
                public long Add(ServerTypeDatum add)
                {
                        return this._ServerType.Add(add);
                }
                /// <summary>
                /// 检查类别是否重复
                /// </summary>
                /// <param name="typeVal">类别值</param>
                /// <returns>是否重复</returns>
                public bool CheckIsRepeat([NullValidate("rpc.server.type.null")]
                [LenValidate("rpc.server.type.len", 4, 50)]
                [FormatValidate("rpc.server.type.error",  ValidateFormat.字母点)]string typeVal)
                {
                        return this._ServerType.CheckIsRepeat(typeVal);
                }
                /// <summary>
                /// 删除类别
                /// </summary>
                /// <param name="id">类别Id</param>
                public void Drop([NumValidate("rpc.server.type.id.error", 1)] long id)
                {
                        this._ServerType.Drop(id);
                }
                /// <summary>
                /// 获取服务类别
                /// </summary>
                /// <param name="id">类别Id</param>
                /// <returns>类别资料</returns>
                public ServerType Get([NumValidate("rpc.server.type.id.error", 1)] long id)
                {
                        return this._ServerType.Get(id);
                }
                /// <summary>
                /// 获取服务组下的类别
                /// </summary>
                /// <param name="groupId">服务组Id</param>
                /// <returns>类别列表</returns>
                public ServerType[] Gets([NumValidate("rpc.server.group.id.error", 1)] long groupId)
                {
                        return this._ServerType.Gets(groupId);
                }
                /// <summary>
                /// 查询服务类别
                /// </summary>
                /// <param name="query">查询分页参数</param>
                /// <param name="count">数据量</param>
                /// <returns>类别列表</returns>
                public ServerTypeData[] Query(PagingParam<ServerTypeQueryParam> query, out long count)
                {
                        return this._ServerType.Query(query, out count);
                }
                /// <summary>
                /// 设置服务类别
                /// </summary>
                /// <param name="id">类别Id</param>
                /// <param name="param">修改的信息</param>
                public void Set([NumValidate("rpc.server.type.id.error", 1)] long id, ServerTypeSetParam param)
                {
                        this._ServerType.Set(id, param);
                }
        }
}
