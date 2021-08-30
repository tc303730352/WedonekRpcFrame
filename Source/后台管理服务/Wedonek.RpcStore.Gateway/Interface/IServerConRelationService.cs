using HttpApiGateway.Model;

using RpcModel;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerConRelationService
        {
                void Drop(long serviceId, long remoteId);
                ServerRelation[] QueryRelation(long serverId, PagingParam<QueryServiceParam> query, out long count);
                ServerRelation[] Query(long serverId, IBasicPage paging, out long count);
                void Set(long serviceId, long remoteId);
        }
}