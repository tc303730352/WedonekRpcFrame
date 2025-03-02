using RpcStore.RemoteModel.DictItem.Model;

namespace RpcStore.Service.Interface
{
    public interface IDictItemService
    {
        DictItemDto[] GetDictItem (string code);
        DictTreeItem[] GetTrees (string indexCode);
    }
}