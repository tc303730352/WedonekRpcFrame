using RpcExtend.Model;
using RpcExtend.Model.DB;

namespace RpcExtend.DAL.Repository
{
    internal class RemoteServerConfigDAL : IRemoteServerConfigDAL
    {
        private IRpcBasicRepository<RemoteServerConfigModel> _BasicDAL;

        public RemoteServerConfigDAL(IRpcBasicRepository<RemoteServerConfigModel> dal)
        {
            _BasicDAL = dal;
        }
      
        public RemoteServerConfig GetServer(long serverId)
        {
            return _BasicDAL.Queryable.InnerJoin<RemoteServerType>((a, b) => a.SystemType == b.Id).Select((a,b)=> new { 
                a.Id,
                a.GroupId,
                b.SystemName,
                a.ServerIp,
                a.RemoteIp,
                a.ServerName,
                a.ServerPort,
                a.ConIp
            }).InnerJoin<ServerGroupModel>((a, b) => a.GroupId == b.Id).Select((a, b) => new RemoteServerConfig {
                ConIp = a.ConIp,
                RemoteIp = a.RemoteIp,
                ServerIp = a.ServerIp,
                ServerName = a.ServerName,
                ServerPort = a.ServerPort,
                SystemType=a.SystemName,
                GroupName=b.GroupName
            }).First();
        }
    }
}
