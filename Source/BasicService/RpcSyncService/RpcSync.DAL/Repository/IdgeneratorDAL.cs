using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class IdgeneratorDAL : IIdgeneratorDAL
    {
        private readonly IRpcExtendResource<IdgeneratorModel> _BasicDAL;
        public IdgeneratorDAL (IRpcExtendResource<IdgeneratorModel> dal)
        {
            this._BasicDAL = dal;
        }

        public int GetWorkId (string mac, int index, long serverTypeId)
        {
            return this._BasicDAL.Get(c => c.SystemTypeId == serverTypeId && c.Mac == mac && c.ServerIndex == index, c => c.WorkId);
        }
        public void Add (IdgeneratorModel add)
        {
            this._BasicDAL.Insert(add);
        }

        public int GetMaxWorkId ()
        {
            return this._BasicDAL.Max(a => a.WorkId);
        }
    }
}
