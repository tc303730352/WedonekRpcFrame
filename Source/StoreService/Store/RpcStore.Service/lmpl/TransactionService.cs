using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.Tran;
using RpcStore.RemoteModel.Tran.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class TransactionService : ITransactionService
    {
        private readonly ITransactionCollect _Transaction;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerRegionCollect _Region;
        private readonly IServerCollect _Server;
        public TransactionService (ITransactionCollect transaction,
                IServerCollect server,
                IServerRegionCollect region,
                IServerTypeCollect serverType)
        {
            this._Region = region;
            this._Server = server;
            this._ServerType = serverType;
            this._Transaction = transaction;
        }

        public TransactionData Get (long id)
        {
            TransactionListModel tran = this._Transaction.Get(id);
            TransactionData data = tran.ConvertMap<TransactionListModel, TransactionData>();
            data.ServerName = this._Server.GetName(tran.ServerId);
            data.SystemTypeName = this._ServerType.GetName(tran.SystemType);
            data.Region = this._Region.GetName(tran.RegionId);
            return data;
        }

        private TransactionLog[] _Format (BasicTransaction[] trans)
        {
            Dictionary<string, string> types = this._ServerType.GetNames(trans.Distinct(a => a.SystemType));
            Dictionary<long, string> servers = this._Server.GetNames(trans.Distinct(a => a.ServerId));
            Dictionary<int, string> regions = this._Region.GetNames(trans.Distinct(a => a.RegionId));
            return trans.ConvertMap<BasicTransaction, TransactionLog>((a, b) =>
           {
               b.ServerName = servers.GetValueOrDefault(a.ServerId);
               b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
               b.Region = regions.GetValueOrDefault(a.RegionId);
               return b;
           });
        }

        public PagingResult<TransactionLog> QueryRoot (TransactionQuery query, IBasicPage paging)
        {
            BasicTransaction[] trans = this._Transaction.QueryRoot(query, paging, out int count);
            if (trans == null)
            {
                return null;
            }
            TransactionLog[] logs = this._Format(trans);
            return new PagingResult<TransactionLog>(logs, count);
        }
    }
}
