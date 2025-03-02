using RpcSync.Model;

namespace RpcSync.DAL
{
    public interface IRemoteServerConfigDAL
    {
        RemoteServer[] GetServer();
        RemoteServerConfig GetServer(long serverId);
        ServerState GetServerState();
    }
}