using RpcModel;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务组
        /// </summary>
        internal class ServerGroupController : HttpApiGateway.ApiController
        {
                private readonly IServerGroupService _Group = null;

                public ServerGroupController(IServerGroupService group)
                {
                        this._Group = group;
                }
                /// <summary>
                /// 添加服务组
                /// </summary>
                /// <param name="add"></param>
                /// <returns></returns>
                public long Add(ServerGroupDatum add)
                {
                        return this._Group.AddGroup(add);
                }
                /// <summary>
                /// 检查是否重复
                /// </summary>
                /// <param name="typeVal">类别</param>
                /// <returns>是否重复</returns>
                public bool CheckIsRepeat(
                        [NullValidate("rpc.group.type.null")]
                [LenValidate("rpc.group.type.len", 4, 50)]
                [FormatValidate("rpc.group.type.error",  ValidateFormat.字母点)]
                string typeVal)
                {
                        return this._Group.CheckIsRepeat(typeVal);
                }
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="groupId">组别Id</param>
                public void Drop([NumValidate("rpc.group.id.error", 1)] long groupId)
                {
                        this._Group.DropGroup(groupId);
                }
                /// <summary>
                /// 获取组信息
                /// </summary>
                /// <param name="groupId">组Id</param>
                /// <returns>服务组信息</returns>
                public ServerGroup GetGroup([NumValidate("rpc.group.id.error", 1)] long groupId)
                {
                        return this._Group.GetGroup(groupId);
                }
                /// <summary>
                /// 获取所以服务组
                /// </summary>
                /// <returns>组信息</returns>
                public ServerGroup[] GetGroups()
                {
                        return this._Group.GetGroups();
                }
                /// <summary>
                /// 查询服务组
                /// </summary>
                /// <param name="name">组名</param>
                /// <param name="paging">分页信息</param>
                /// <param name="count">数据量</param>
                /// <returns>组信息列表</returns>
                public ServerGroup[] Query([LenValidate("rpc.group.name.len", 0, 50)] string name, BasicPage paging, out long count)
                {
                        return this._Group.Query(name, paging, out count);
                }
                /// <summary>
                /// 设置组
                /// </summary>
                /// <param name="groupId">组Id</param>
                /// <param name="name">组名</param>
                public void Set([NumValidate("rpc.group.id.error", 1)]
                long groupId,
                        [NullValidate("rpc.group.name.null")]
                        [LenValidate("rpc.group.name.len", 2, 50)]
                        string name)
                {
                        this._Group.SetGroup(groupId, name);
                }
        }
}
