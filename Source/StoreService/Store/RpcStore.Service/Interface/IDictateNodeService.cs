using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.Service.Interface
{
    public interface IDictateNodeService
    {
        long Add(DictateNodeAdd add);
        void Delete(long id);
        DictateNodeData Get(long id);
        DictateNodeData[] GetAllDictateNode();
        DictateNodeData[] GetDictateNodes(long? parentId);
        TreeDictateNode[] GetTrees();
        void Set(long id, string name);
    }
}