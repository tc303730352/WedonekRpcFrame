using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Gateway.Service
{
        /// <summary>
        /// 服务区域
        /// </summary>
        internal class ServerRegionService : IServerRegionService
        {
                private readonly IServerRegionCollect _Region = null;
                private readonly IServerCollect _Server = null;
                public ServerRegionService(IServerRegionCollect region, IServerCollect server)
                {
                        this._Region = region;
                        this._Server = server;
                }
                /// <summary>
                /// 设置区域名
                /// </summary>
                /// <param name="id"></param>
                /// <param name="name"></param>
                public void SetRegion(int id, string name)
                {
                        this._Region.SetRegion(id, name);
                }
                /// <summary>
                /// 添加区域
                /// </summary>
                /// <param name="name">区域名</param>
                /// <returns>区域Id</returns>
                public int AddRegion(string name)
                {
                        return this._Region.AddRegion(name);
                }
                /// <summary>
                /// 删除区域
                /// </summary>
                /// <param name="id">区域Id</param>
                public void DropRegion(int id)
                {
                        if (this._Server.CheckRegion(id))
                        {
                                throw new ErrorException("rpc.server.region.exists");
                        }
                        this._Region.DropRegion(id);
                }
                /// <summary>
                /// 获取所有服务区域
                /// </summary>
                /// <returns></returns>
                public ServerRegion[] GetServerRegion()
                {
                        return this._Region.GetServerRegion();
                }
        }
}
