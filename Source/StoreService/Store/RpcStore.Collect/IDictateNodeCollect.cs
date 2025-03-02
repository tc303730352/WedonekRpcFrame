using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.Collect
{
    public interface IDictateNodeCollect
    {
        long Add(DictateNodeAdd add);
        void Delete(DictateNodeModel obj);
        DictateNodeModel Get(long id);
        DictateNodeModel[] Gets();
        DictateNodeModel[] Gets(long parentId);
        void Set(DictateNodeModel node, string name);
    }
}