using RpcStore.RemoteModel.DictItem;
using RpcStore.RemoteModel.DictItem.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class DictItemService : IDictItemService
    {
        public DictItemDto[] GetDictItem (string code)
        {
            return new GetDictItem
            {
                IndexCode = code
            }.Send();
        }

        public DictTreeItem[] GetTrees (string code)
        {
            return new GetDictItemTrees
            {
                IndexCode = code
            }.Send();
        }
    }
}
