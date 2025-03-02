using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcManageClient;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMer;
using RpcStore.RemoteModel.Mer.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class RpcMerService : IRpcMerService
    {
        private readonly IRpcMerCollect _RpcMer;
        private readonly IRemoteGroupCollect _RemoteGroup;
        private readonly IRpcServerCollect _RpcServer;
        public RpcMerService (IRpcMerCollect rpcMer,
            IRpcServerCollect rpcServer,
            IRemoteGroupCollect remoteGroup)
        {
            this._RpcServer = rpcServer;
            this._RemoteGroup = remoteGroup;
            this._RpcMer = rpcMer;
        }

        public long AddMer (RpcMerAdd mer)
        {
            return this._RpcMer.AddMer(mer);
        }

        public void Delete (long id)
        {
            RpcMerModel mer = this._RpcMer.GetRpcMer(id);
            if (this._RemoteGroup.CheckIsExists(mer.Id))
            {
                throw new ErrorException("rpc.store.mer.bind.server");
            }
            this._RpcMer.Delete(mer);
            this._RpcServer.RefreshMer(id);
        }
        public BasicRpcMer[] GetBasic ()
        {
            return this._RpcMer.GetBasic();
        }
        public RpcMerDatum GetRpcMer (long id)
        {
            RpcMerModel model = this._RpcMer.GetRpcMer(id);
            return model.ConvertMap<RpcMerModel, RpcMerDatum>();
        }

        public PagingResult<RpcMer> Query (string name, IBasicPage paging)
        {
            RpcMerModel[] list = this._RpcMer.Query(name, paging, out int count);
            Dictionary<long, int> servers = this._RemoteGroup.GetServerNum(list.ConvertAll(c => c.Id));
            return new PagingResultTo<RpcMerModel, RpcMer>(count, list, (a, b) =>
            {
                b.ServerNum = servers.GetValueOrDefault(a.Id);
                return b;
            });
        }

        public void SetMer (long id, RpcMerSet datum)
        {
            RpcMerModel mer = this._RpcMer.GetRpcMer(id);
            if (this._RpcMer.SetMer(mer, datum.ConvertMap<RpcMerSet, RpcMerSetDatum>()))
            {
                this._RpcServer.RefreshMer(id);
            }
        }
    }
}
