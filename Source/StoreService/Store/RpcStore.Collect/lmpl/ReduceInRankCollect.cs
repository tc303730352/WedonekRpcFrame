using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ReduceInRank.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ReduceInRankCollect : IReduceInRankCollect
    {
        private readonly IReduceInRankConfigDAL _BasicDAL;
        public ReduceInRankCollect (IReduceInRankConfigDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public ReduceInRankConfigModel Get (long id)
        {
            ReduceInRankConfigModel config = this._BasicDAL.Get(id);
            if (config == null)
            {
                throw new ErrorException("rpc.store.reduceInRank.config.not.find");
            }
            return config;
        }
        public ReduceInRankConfigModel Get (long rpcMerId, long serverId)
        {
            return this._BasicDAL.Get(rpcMerId, serverId);
        }
        public bool Sync (ReduceInRankAdd datum)
        {
            ReduceInRankConfigModel data = this._BasicDAL.Get(datum.RpcMerId, datum.ServerId);
            if (data != null)
            {
                if (datum.IsEquals(data))
                {
                    return false;
                }
                ReduceInRankDatum set = datum.ConvertMap<ReduceInRankAdd, ReduceInRankDatum>();
                this._BasicDAL.Set(data.Id, set);
            }
            else
            {
                ReduceInRankConfigModel add = datum.ConvertMap<ReduceInRankAdd, ReduceInRankConfigModel>();
                _ = this._BasicDAL.Add(add);
            }
            return true;
        }


        public void Delete (ReduceInRankConfigModel obj)
        {
            this._BasicDAL.Delete(obj.Id);
        }
    }
}
