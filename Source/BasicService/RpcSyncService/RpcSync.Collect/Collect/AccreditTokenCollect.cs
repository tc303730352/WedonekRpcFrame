using RpcSync.DAL;
using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.Collect.Collect
{
    internal class AccreditTokenCollect : IAccreditTokenCollect
    {
        private readonly IAccreditTokenDAL _AccreditToken;

        public AccreditTokenCollect (IAccreditTokenDAL accreditToken)
        {
            this._AccreditToken = accreditToken;
        }

        public void Add (AccreditTokenModel add)
        {
            this._AccreditToken.Add(add);
        }

        public bool Delete (string id)
        {
            return this._AccreditToken.Delete(id);
        }

        public AccreditTokenDatum Get (string id)
        {
            return this._AccreditToken.Get(id);
        }

        public string[] GetSubId (string accreditId)
        {
            return this._AccreditToken.GetSubId(accreditId);
        }

        public bool Set (string accreditId, AccreditTokenSet set)
        {
            return this._AccreditToken.Set(accreditId, set);
        }

        public void SetOverTime (string accreditId, DateTime overTime)
        {
            this._AccreditToken.SetOverTime(accreditId, overTime);
        }
    }
}
