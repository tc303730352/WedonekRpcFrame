using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class RemoteServerGroupDAL : IRemoteServerGroupDAL
    {
        private readonly IRepository<RemoteServerGroupModel> _BasicDAL;
        public RemoteServerGroupDAL (IRepository<RemoteServerGroupModel> dal)
        {
            this._BasicDAL = dal;
        }

        public bool CheckIsExists (long rpcMerId, MsgSource source)
        {
            long[] mers = new long[]
            {
                rpcMerId,
                source.RpcMerId
            };
            return this._BasicDAL.IsExist(c => mers.Contains(c.RpcMerId) && c.ServerId == source.ServerId);
        }

        public MerServer[] GetAllServer (long merId)
        {
            return this._BasicDAL.Gets<MerServer>(c => c.RpcMerId == merId && c.IsHold);
        }

        public long[] GetHoldServerId (long merId)
        {
            return this._BasicDAL.Gets(a => a.RpcMerId == merId && a.IsHold, a => a.ServerId);
        }

        public string[] GetServerTypeVal (long merId)
        {
            return this._BasicDAL.Gets(c => c.RpcMerId == merId, c => c.TypeVal);
        }
    }
}
