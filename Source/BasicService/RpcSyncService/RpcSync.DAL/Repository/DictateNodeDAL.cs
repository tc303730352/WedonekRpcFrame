using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class DictateNodeDAL : IDictateNodeDAL
    {
        private readonly IRpcExtendResource<DictateNodeModel> _BasicDAL;
        public DictateNodeDAL (IRpcExtendResource<DictateNodeModel> dal)
        {
            this._BasicDAL = dal;
        }
        public DictateNode[] GetDictateNode ()
        {
            return this._BasicDAL.GetAll<DictateNode>();
        }
    }
}
