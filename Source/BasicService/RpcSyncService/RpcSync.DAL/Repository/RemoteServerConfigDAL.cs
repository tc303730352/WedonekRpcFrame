using RpcSync.Model;
using RpcSync.Model.DB;
using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class RemoteServerConfigDAL : IRemoteServerConfigDAL
    {
        private readonly IRepository<RemoteServerConfigModel> _BasicDAL;
        public RemoteServerConfigDAL (IRepository<RemoteServerConfigModel> dal)
        {
            this._BasicDAL = dal;
        }
        public RemoteServerConfig GetServer (long serverId)
        {
            return this._BasicDAL.Queryable.InnerJoin<RemoteServerTypeModel>((a, b) => a.SystemType == b.Id && a.Id == serverId).Select((a, b) => new
            {
                a.Id,
                a.GroupId,
                b.SystemName,
                a.ServerIp,
                a.RemoteIp,
                a.ServerName,
                a.ServerPort,
                a.ConIp
            }).InnerJoin<ServerGroupModel>((a, b) => a.GroupId == b.Id).Select((a, b) => new RemoteServerConfig
            {
                ServerId = a.Id,
                ConIp = a.ConIp,
                RemoteIp = a.RemoteIp,
                ServerIp = a.ServerIp,
                ServerName = a.ServerName,
                ServerPort = a.ServerPort,
                SystemType = a.SystemName,
                GroupName = b.GroupName
            }).First();
        }
        public ServerState GetServerState ()
        {
            return this._BasicDAL.Queryable.Select(c => new ServerState
            {
                Count = SqlFunc.RowCount(),
                MaxTime = SqlFunc.AggregateMax(c.AddTime)
            }).First();
        }

        public RemoteServer[] GetServer ()
        {
            DateTime time = HeartbeatTimeHelper.CurrentDate.AddDays(-1);
            return this._BasicDAL.Gets<RemoteServer>(c => ( c.ServiceState == RpcServiceState.正常 && c.IsOnline ) || ( c.IsOnline == false && c.LastOffliceDate >= time ));
        }

    }
}
