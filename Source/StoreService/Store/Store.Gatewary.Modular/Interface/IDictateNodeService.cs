using RpcStore.RemoteModel.DictateNode.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IDictateNodeService
    {
        /// <summary>
        /// 添加广播指令路由节点
        /// </summary>
        /// <param name="datum">路由节点信息</param>
        /// <returns></returns>
        long AddDictateNode(DictateNodeAdd datum);

        /// <summary>
        /// 删除广播指令路由节点
        /// </summary>
        /// <param name="id"></param>
        void DeleteDictateNode(long id);

        /// <summary>
        /// 获取所有广播指令节点路由
        /// </summary>
        /// <returns></returns>
        DictateNodeData[] GetAllDictateNode();

        /// <summary>
        /// 获取广播指令路由
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        DictateNodeData GetDictateNode(long id);

        /// <summary>
        /// 获取广播路由节点
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <returns></returns>
        DictateNodeData[] GetDictateNodes(long? parentId);

        /// <summary>
        /// 获取所有广播指令节点路由树形结构
        /// </summary>
        /// <returns>树节点</returns>
        TreeDictateNode[] GetDictateNodeTree();

        /// <summary>
        /// 设置广播指令节点路由名称
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="name">名称</param>
        void SetDictateName(long id, string name);

    }
}
