using HttpApiGateway.Model;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IRemoteGroupService
        {
                void SetWeight(long id, int weight);
                void Drop(long id);
                BindRemoteServer[] Get(long rpcMerId);
                void SetBindGroup(BindServer set);
                RemoteServerBindState[] Query(long rpcMerId, PagingParam<QueryServiceParam> query, out long count);
        }
}