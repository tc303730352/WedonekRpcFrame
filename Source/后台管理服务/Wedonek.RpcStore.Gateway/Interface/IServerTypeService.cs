using HttpApiGateway.Model;

using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerTypeService
        {
                long Add(ServerTypeDatum add);
                bool CheckIsRepeat(string typeVal);
                void Drop(long id);
                ServerType Get(long id);
                ServerType[] Gets(long groupId);
                ServerTypeData[] Query(PagingParam<ServerTypeQueryParam> query, out long count);
                void Set(long id, ServerTypeSetParam param);
        }
}