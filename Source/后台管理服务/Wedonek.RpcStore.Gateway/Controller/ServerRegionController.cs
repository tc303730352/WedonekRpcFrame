using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 服务区域管理
        /// </summary>
        internal class ServerRegionController : HttpApiGateway.ApiController
        {
                private readonly IServerRegionService _Service = null;
                public ServerRegionController(IServerRegionService service)
                {
                        this._Service = service;
                }
                /// <summary>
                /// 添加区域
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public int Add(
                        [NullValidate("rpc.region.name.null")]
                        [LenValidate("rpc.region.name.len",2,50)]
                string name)
                {
                        return this._Service.AddRegion(name);
                }
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="id"></param>
                public void Drop([NumValidate("rpc.region.id.error", 1, int.MaxValue)] int id)
                {
                        this._Service.DropRegion(id);
                }
                /// <summary>
                /// 获取列表
                /// </summary>
                /// <returns></returns>
                public ServerRegion[] Gets()
                {
                        return this._Service.GetServerRegion();
                }
                /// <summary>
                /// 修改
                /// </summary>
                /// <param name="obj"></param>
                public void Set(ServerRegion obj)
                {
                        this._Service.SetRegion(obj.Id, obj.RegionName);
                }
        }
}
