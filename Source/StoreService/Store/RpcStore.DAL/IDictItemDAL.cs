using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictItem.Model;

namespace RpcStore.DAL
{
    public interface IDictItemDAL
    {
        DictItemModel[] GetItems (string code);
        DictItemDto[] GetDictItem (string code);
        Dictionary<string, string> GetItemName (string dictCode);
    }
}