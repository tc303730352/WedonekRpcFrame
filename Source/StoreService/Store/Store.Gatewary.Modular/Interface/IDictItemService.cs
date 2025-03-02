using RpcStore.RemoteModel.DictItem.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IDictItemService
    {
        DictItemDto[] GetDictItem (string code);
        DictTreeItem[] GetTrees (string code);
    }
}