using WeDonekRpc.Helper;
using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class AccreditTokenDAL : IAccreditTokenDAL
    {
        private readonly IRpcExtendResource<AccreditTokenModel> _BasicDAL;
        public AccreditTokenDAL (IRpcExtendResource<AccreditTokenModel> dal)
        {
            this._BasicDAL = dal;
        }

        public void Add (AccreditTokenModel add)
        {
            this._BasicDAL.Insert(add);
        }
        public bool Delete (string id)
        {
            return this._BasicDAL.Delete(a => a.AccreditId == id);
        }

        public AccreditTokenDatum Get (string id)
        {
            return this._BasicDAL.Get<AccreditTokenDatum>(a => a.AccreditId == id);
        }

        public void SetOverTime (string accreditId, DateTime overTime)
        {
            if (!this._BasicDAL.Update(new AccreditTokenModel
            {
                OverTime = overTime,
                AccreditId = accreditId
            }, new string[]
            {
                "OverTime"
            }))
            {
                throw new ErrorException("accredit.db.set.fail");
            }
        }

        public string[] GetSubId (string accreditId)
        {
            return this._BasicDAL.Gets(a => a.PAccreditId == accreditId, a => a.AccreditId);
        }

        public bool Set (string accreditId, AccreditTokenSet set)
        {
            return this._BasicDAL.Update(set, a => a.AccreditId == accreditId && a.StateVer < set.StateVer);
        }
    }
}
