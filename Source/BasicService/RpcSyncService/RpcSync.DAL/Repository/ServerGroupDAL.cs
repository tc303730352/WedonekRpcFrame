using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ServerGroupDAL : IServerGroupDAL
    {
        private readonly IRepository<ServerGroupModel> _BasicDAL;
        public ServerGroupDAL (IRepository<ServerGroupModel> dal)
        {
            this._BasicDAL = dal;
        }
        public ServerGroup[] GetServerGroup ()
        {
            return this._BasicDAL.GetAll<ServerGroup>();
        }
    }
}
