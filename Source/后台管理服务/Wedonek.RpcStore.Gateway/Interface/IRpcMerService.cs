using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        public interface IRpcMerService
        {
                long AddMer(RpcMerDatum mer);
                RpcMer GetRpcMer(long id);
                RpcMer[] Query(string name, IBasicPage paging, out long count);
                void SetMer(long id, RpcMerSetParam datum);
                void DropMer(long id);
        }
}