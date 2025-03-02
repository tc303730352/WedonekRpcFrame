using RpcExtend.Model;

namespace RpcExtend.Collect
{
    public interface IRemoteServerCollect
    {
        RemoteServerConfig GetServer(long serverId);
    }
}