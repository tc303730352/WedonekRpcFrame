using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictItem.Model;

namespace RpcStore.Collect
{
    public interface IDictItemCollect
    {
        DictItemModel[] GetItems (string code);
        DictItemDto[] GetDictItem (string code);
        Dictionary<string, string> GetItemName (string dictCode);
    }
}