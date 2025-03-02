using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.DictItem;
using RpcStore.RemoteModel.DictItem.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class DictItemEvent : IRpcApiService
    {
        private readonly IDictItemService _Service;

        public DictItemEvent (IDictItemService service)
        {
            this._Service = service;
        }
        public DictTreeItem[] GetDictItemTrees (GetDictItemTrees obj)
        {
            return this._Service.GetTrees(obj.IndexCode);
        }
        public DictItemDto[] GetDictItem (GetDictItem obj)
        {
            return this._Service.GetDictItem(obj.IndexCode);
        }
    }
}
