using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IRpcMerCollect
        {
                void DropMer(long id);
                long AddMer(RpcMerDatum mer);
                RpcMer GetRpcMer(long id);
                RpcMer[] Query(string name, IBasicPage paging, out long count);
                void SetMer(long id, RpcMerSetParam datum);
        }
}