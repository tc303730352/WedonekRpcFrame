using WeDonekRpc.CacheClient.Interface;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictItem.Model;

namespace RpcStore.Collect.lmpl
{
    internal class DictItemCollect : IDictItemCollect
    {
        private readonly IDictItemDAL _DictItem;
        private readonly ICacheController _Cache;

        public DictItemCollect (IDictItemDAL dictItem, ICacheController cache)
        {
            this._DictItem = dictItem;
            this._Cache = cache;
        }
        public DictItemDto[] GetDictItem (string code)
        {
            string key = "DictItem_" + code;
            if (this._Cache.TryGet(key, out DictItemDto[] items))
            {
                return items;
            }
            items = this._DictItem.GetDictItem(code);
            _ = this._Cache.Set(key, items);
            return items;
        }
        public DictItemModel[] GetItems (string code)
        {
            string key = "DictItem_" + code;
            if (this._Cache.TryGet(key, out DictItemModel[] items))
            {
                return items;
            }
            items = this._DictItem.GetItems(code);
            _ = this._Cache.Set(key, items);
            return items;
        }

        public Dictionary<string, string> GetItemName (string dictCode)
        {
            string key = "DictItemName_" + dictCode;
            if (this._Cache.TryGet(key, out Dictionary<string, string> name))
            {
                return name;
            }
            name = this._DictItem.GetItemName(dictCode);
            _ = this._Cache.Set(key, name);
            return name;
        }
    }
}
