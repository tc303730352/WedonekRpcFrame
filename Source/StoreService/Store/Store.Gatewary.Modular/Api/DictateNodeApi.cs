using RpcStore.RemoteModel.DictateNode.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.DictateNode;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 广播指令路由节点管理
    /// </summary>
    internal class DictateNodeApi : ApiController
    {
        private readonly IDictateNodeService _Service;
        public DictateNodeApi (IDictateNodeService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加广播指令路由节点
        /// </summary>
        /// <param name="datum">路由节点信息</param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.dictatenode.datum.null")] DictateNodeAdd datum)
        {
            return this._Service.AddDictateNode(datum);
        }

        /// <summary>
        /// 删除广播指令路由节点
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.dictatenode.id.error", 1)] long id)
        {
            this._Service.DeleteDictateNode(id);
        }

        /// <summary>
        /// 获取所有广播指令节点路由
        /// </summary>
        /// <returns></returns>
        public DictateNodeData[] GetAll ()
        {
            return this._Service.GetAllDictateNode();
        }

        /// <summary>
        /// 获取广播指令路由
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public DictateNodeData Get ([NumValidate("rpc.store.dictatenode.id.error", 1)] long id)
        {
            return this._Service.GetDictateNode(id);
        }

        /// <summary>
        /// 获取广播路由节点
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <returns></returns>
        public DictateNodeData[] Gets (long? parentId)
        {
            return this._Service.GetDictateNodes(parentId);
        }

        /// <summary>
        /// 获取所有广播指令节点路由树形结构
        /// </summary>
        /// <returns>树节点</returns>
        public TreeDictateNode[] GetTree ()
        {
            return this._Service.GetDictateNodeTree();
        }

        /// <summary>
        /// 设置广播指令节点路由名称
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetDictateName ([NullValidate("rpc.store.dictatenode.param.null")] UI_SetDictateName param)
        {
            this._Service.SetDictateName(param.Id, param.Name);
        }

    }
}
