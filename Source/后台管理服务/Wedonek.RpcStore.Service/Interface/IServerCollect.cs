using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerCollect
        {
                void SetServiceState(long id, RpcServiceState state);
                ServerConfigDatum[] GetServices(long[] ids);
                long AddService(ServerConfigAddParam add);
                bool CheckIsExists(long sysTypeId);
                bool CheckIsOnline(long id);
                bool CheckServerPort(string mac, int serverPort);
                void DropService(long id);
                RemoteServerConfig GetService(long id);
                ServerConfigDatum[] QueryService(QueryServiceParam query, IBasicPage paging, out long count);
                void SetService(long id, ServerConfigSetParam set);
                bool CheckIsExistsByGroup(long groupId);
                bool CheckRegion(int regionId);
        }
}