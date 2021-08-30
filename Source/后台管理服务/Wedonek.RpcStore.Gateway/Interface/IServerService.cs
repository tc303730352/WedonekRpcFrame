using RpcModel;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerService
        {
                void SetServiceState(long id, RpcServiceState state);
                long Add(ServerConfigAddParam add);
                void Drop(long id);
                RemoteServerDatum Get(long id);
                RemoteServer[] Query(QueryServiceParam query, IBasicPage paging, out long count);
                void SetService(long id, ServerConfigSetParam set);
        }
}