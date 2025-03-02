using RpcStore.RemoteModel.ServerType.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.ServerType;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务类别管理
    /// </summary>
    internal class ServerTypeApi : ApiController
    {
        private readonly IServerTypeService _Service;
        public ServerTypeApi (IServerTypeService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务类别
        /// </summary>
        /// <param name="datum">类别资料</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.servertype.datum.null")] ServerTypeAdd datum)
        {
            return this._Service.AddServerType(datum);
        }

        /// <summary>
        /// 检查类别值是否重复
        /// </summary>
        /// <param name="typeVal">类别值</param>
        public void CheckVal ([NullValidate("rpc.store.servertype.typeVal.null")] string typeVal)
        {
            this._Service.CheckServerTypeVal(typeVal);
        }

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="id">数据ID</param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.servertype.id.error", 1)] long id)
        {
            this._Service.DeleteServerType(id);
        }

        /// <summary>
        /// 获取服务类别资料
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>服务类型</returns>
        public ServerType Get ([NumValidate("rpc.store.servertype.id.error", 1)] long id)
        {
            return this._Service.GetServerType(id);
        }

        /// <summary>
        /// 获取服务组下的类别
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>服务类型</returns>
        public ServerType[] GetByGroup ([NumValidate("rpc.store.servertype.groupId.error", 1)] long groupId)
        {
            return this._Service.GetServerTypeByGroup(groupId);
        }

        /// <summary>
        /// 查询服务类别
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<ServerTypeDatum> Query ([NullValidate("rpc.store.servertype.param.null")] UI_QueryServerType param)
        {
            ServerTypeDatum[] results = this._Service.QueryServerType(param.Query, param, out int count);
            return new PagingResult<ServerTypeDatum>(count, results);
        }

        /// <summary>
        /// 修改节点类型资料
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void Set ([NullValidate("rpc.store.servertype.param.null")] UI_SetServerType param)
        {
            this._Service.SetServerType(param.Id, param.Datum);
        }

    }
}
