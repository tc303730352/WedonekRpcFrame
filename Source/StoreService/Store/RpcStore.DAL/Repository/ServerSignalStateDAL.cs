using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerSignalStateDAL : IServerSignalStateDAL
    {
        private readonly IRepository<ServerSignalStateModel> _BasicDAL;
        public ServerSignalStateDAL (IRepository<ServerSignalStateModel> dal)
        {
            this._BasicDAL = dal;
        }

        public ServerSignalStateModel[] Gets (long serverId)
        {
            return this._BasicDAL.Gets(c => c.ServerId == serverId);
        }
        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(a => a.ServerId == serverId);
        }
        public void ClearRemote (long remoteId)
        {
            _ = this._BasicDAL.Delete(a => a.RemoteId == remoteId);
        }
    }
}
