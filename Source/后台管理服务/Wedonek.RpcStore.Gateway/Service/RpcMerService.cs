using RpcModel;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class RpcMerService : IRpcMerService
        {
                private readonly IRpcMerCollect _RpcMer = null;
                public RpcMerService(IRpcMerCollect rpcMer)
                {
                        this._RpcMer = rpcMer;
                }

                public long AddMer(RpcMerDatum mer)
                {
                        return this._RpcMer.AddMer(mer);
                }

                public void DropMer(long id)
                {
                        this._RpcMer.DropMer(id);
                }

                public RpcMer GetRpcMer(long id)
                {
                        return this._RpcMer.GetRpcMer(id);
                }

                public RpcMer[] Query(string name, IBasicPage paging, out long count)
                {
                        return this._RpcMer.Query(name, paging, out count);
                }

                public void SetMer(long id, RpcMerSetParam datum)
                {
                        this._RpcMer.SetMer(id, datum);
                }
        }
}
