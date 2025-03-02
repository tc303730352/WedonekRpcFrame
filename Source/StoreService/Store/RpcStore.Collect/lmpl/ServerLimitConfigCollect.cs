using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.LimitConfig.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerLimitConfigCollect : IServerLimitConfigCollect
    {
        private readonly IServerLimitConfigDAL _BasicDAL;
        public ServerLimitConfigCollect (IServerLimitConfigDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public ServerLimitConfigModel Get (long serverId, bool isAllowNull = false)
        {
            ServerLimitConfigModel limit = this._BasicDAL.Get(serverId);
            if (limit == null && !isAllowNull)
            {
                throw new ErrorException("rpc.store.limit.config.not.find");
            }
            return limit;
        }
        public void Delete (ServerLimitConfigModel config)
        {
            this._BasicDAL.Delete(config.ServerId);
        }

        public bool SyncConfig (LimitConfigDatum datum)
        {
            ServerLimitConfigModel config = this._BasicDAL.Get(datum.ServerId);
            if (config == null)
            {
                ServerLimitConfigModel add = datum.ConvertMap<LimitConfigDatum, ServerLimitConfigModel>();
                this._BasicDAL.Add(add);
            }
            else if (datum.IsEquals(config))
            {
                return false;
            }
            else
            {
                ServerLimitConfig set = datum.ConvertMap<LimitConfigDatum, ServerLimitConfig>();
                this._BasicDAL.Set(config.ServerId, set);
            }
            return true;
        }
    }
}
