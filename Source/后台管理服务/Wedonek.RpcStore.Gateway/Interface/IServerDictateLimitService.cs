using HttpApiGateway.Model;

using RpcModel.Model;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerDictateLimitService
        {
                ServerDictateLimitData Get(long id);
                long Add(AddDictateLimit add);
                void Drop(long id);
                ServerDictateLimitData[] Query(PagingParam<DictateQueryParam> param, out long count);
                void Set(long id, ServerDictateLimit limit);
        }
}