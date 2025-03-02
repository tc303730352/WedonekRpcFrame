using WeDonekRpc.Helper;
using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ServerCurConfigDAL : IServerCurConfigDAL
    {
        private readonly IRepository<ServerCurConfigModel> _BasicDAL;
        public ServerCurConfigDAL (IRepository<ServerCurConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public bool IsExists (long serverId)
        {
            return this._BasicDAL.IsExist(c => c.ServerId == serverId);
        }
        public void Set (ServerCurConfigModel set)
        {
            if (!this._BasicDAL.Update(set, new string[]
            {
                "CurConfig",
                "UpTime"
            }))
            {
                throw new ErrorException("rpc.server.cur.config.set.fail");
            }
        }
        public void Add (ServerCurConfigModel add)
        {
            this._BasicDAL.Insert(add);
        }
    }
}
