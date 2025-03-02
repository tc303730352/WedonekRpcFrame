using RpcCentral.Model;
using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IRemoteServerDAL
    {
        int GetVerNum (long id);
        ServerCont FindService (ContainerGetArg arg);

        long FindServiceId (long systemType, string mac, int serverIndex);
        RemoteServerModel GetRemoteServer (long id);
        BasicServer[] GetRemoteServerConfig (long[] ids);
        long[] LoadServer (int serverIndex);
        bool ServerOffline (long serverId, int serverIndex);
        bool ServerOnline (long serverId, int serverIndex);
        void SetConIp (long serverId, string conIp);
        void SetApiVer (long serverId, int ver);
        bool CheckContainerPort (long contGroupId, int serverPort);
        long Add (RemoteServerConfig add, string typeVal);
        void SetContainerId (long serverId, long containerId);
    }
}