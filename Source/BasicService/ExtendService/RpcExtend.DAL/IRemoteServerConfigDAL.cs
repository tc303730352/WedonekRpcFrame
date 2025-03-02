using RpcExtend.Model;

namespace RpcExtend.DAL
{
    public interface IRemoteServerConfigDAL
    {
        RemoteServerConfig GetServer(long serverId);
    }
}