using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface IRemoteServerCollect
    {
        RemoteServer[] GetContainerServer (long[] containerId);
        long[] GetServerId (int? regionId, List<long> sysTypeId);
        long[] GetAllServer (int? regionId);
        RemoteServer[] GetAllServers ();
        RemoteServerConfig GetServer (long serverId);
    }
}