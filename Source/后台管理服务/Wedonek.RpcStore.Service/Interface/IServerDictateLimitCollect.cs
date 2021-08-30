using RpcModel;
using RpcModel.Model;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerDictateLimitCollect
        {
                ServerDictateLimitData GetDictateLimit(long id);
                long AddDictateLimit(AddDictateLimit add);
                void DropDictateLimit(long id);
                ServerDictateLimitData[] Query(long serverId, string dictate, IBasicPage paging, out long count);
                void SetDictateLimit(long id, ServerDictateLimit limit);
        }
}