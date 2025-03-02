using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictItem.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class DictItemDAL : IDictItemDAL
    {
        private readonly IRepository<DictItemModel> _BasicDAL;
        public DictItemDAL (IRepository<DictItemModel> dal)
        {
            this._BasicDAL = dal;
        }
        public DictItemModel[] GetItems (string code)
        {
            return this._BasicDAL.Gets(a => a.DictCode == code);
        }
        public DictItemDto[] GetDictItem (string code)
        {
            return this._BasicDAL.Gets<DictItemDto>(c => c.DictCode == code);
        }

        public Dictionary<string, string> GetItemName (string dictCode)
        {
            return this._BasicDAL.Gets(a => a.DictCode == dictCode, a => new
            {
                a.ItemCode,
                a.ItemText
            }).ToDictionary(a => a.ItemCode, a => a.ItemText);
        }
    }
}
