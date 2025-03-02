using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Mer;
using RpcStore.RemoteModel.Mer.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class RpcMerEvent : IRpcApiService
    {
        private readonly IRpcMerService _Service;
        public RpcMerEvent (IRpcMerService service)
        {
            this._Service = service;
        }

        public long AddMer (AddMer mer)
        {
            return this._Service.AddMer(mer.Datum);
        }

        public void DeleteMer (DeleteMer obj)
        {
            this._Service.Delete(obj.RpcMerId);
        }
        public BasicRpcMer[] GetMerItems ()
        {
            return this._Service.GetBasic();
        }

        public RpcMerDatum GetMer (GetMer obj)
        {
            return this._Service.GetRpcMer(obj.RpcMerId);
        }

        public PagingResult<RpcMer> QueryMer (QueryMer query)
        {
            return this._Service.Query(query.Name, query.ToBasicPage());
        }

        public void SetMer (SetMer set)
        {
            this._Service.SetMer(set.RpcMerId, set.Datum);
        }
    }
}
