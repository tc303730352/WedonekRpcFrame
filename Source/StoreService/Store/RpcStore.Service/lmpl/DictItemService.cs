using WeDonekRpc.Helper;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictItem.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class DictItemService : IDictItemService
    {
        private readonly IDictItemCollect _DictItem;

        public DictItemService (IDictItemCollect dictItem)
        {
            this._DictItem = dictItem;
        }

        public DictItemDto[] GetDictItem (string code)
        {
            return this._DictItem.GetDictItem(code);
        }

        public DictTreeItem[] GetTrees (string code)
        {
            DictItemModel[] items = this._DictItem.GetItems(code);
            return items.Convert(c => c.PrtItemCode == "root", c =>
            {
                DictTreeItem item = new DictTreeItem
                {
                    ItemCode = c.ItemCode,
                    ItemText = c.ItemText
                };
                this._InitChildren(item, items);
                return item;
            });
        }
        private void _InitChildren (DictTreeItem prt, DictItemModel[] items)
        {
            prt.Children = items.Convert(c => c.PrtItemCode == prt.ItemCode, c =>
            {
                DictTreeItem item = new DictTreeItem
                {
                    ItemCode = c.ItemCode,
                    ItemText = c.ItemText
                };
                this._InitChildren(item, items);
                return item;
            });
        }
    }
}
