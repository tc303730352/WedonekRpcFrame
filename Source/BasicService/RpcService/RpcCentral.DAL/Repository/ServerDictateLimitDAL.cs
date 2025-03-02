using RpcCentral.Model.DB;
using WeDonekRpc.Model.Model;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    /// <summary>
    /// 服务指令限制
    /// </summary>
    internal class ServerDictateLimitDAL : IServerDictateLimitDAL
    {
        private readonly IRepository<ServerDictateLimitModel> _Db;
        public ServerDictateLimitDAL (IRepository<ServerDictateLimitModel> db)
        {
            this._Db = db;
        }

        public ServerDictateLimit[] GetDictateLimit (long serverId)
        {
            return this._Db.Gets<ServerDictateLimit>(c => c.ServerId == serverId);
        }
    }
}
