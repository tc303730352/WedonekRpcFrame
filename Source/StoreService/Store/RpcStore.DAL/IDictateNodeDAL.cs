using RpcStore.Model.ExtendDB;

namespace RpcStore.DAL
{
    public interface IDictateNodeDAL
    {
        long Add(DictateNodeModel add);
        void Delete(long[] ids);
        DictateNodeModel Get(long id);
        DictateNodeModel[] Gets();
        DictateNodeModel[] Gets(long[] ids);
        void Set(long id, string name);
    }
}