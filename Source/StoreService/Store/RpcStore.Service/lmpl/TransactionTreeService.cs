using RpcStore.Collect;
using RpcStore.Model.Tran;
using RpcStore.RemoteModel.Tran.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace RpcStore.Service.lmpl
{
    internal class TransactionTreeService : ITransactionTreeService
    {
        private readonly ITransactionCollect _Transaction;
        private readonly IServerTypeCollect _ServerType;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IServerRegionCollect _Region;
        private readonly IServerCollect _Server;
        public TransactionTreeService (ITransactionCollect transaction,
                IServerCollect server,
                IRpcMerCollect rpcMer,
                IServerRegionCollect region,
                IServerTypeCollect serverType)
        {
            this._RpcMer = rpcMer;
            this._Region = region;
            this._Server = server;
            this._ServerType = serverType;
            this._Transaction = transaction;
        }
        private Dictionary<string, string> _SystemTypes;
        private Dictionary<long, string> _ServerName;
        private Dictionary<int, string> _RegionName;
        private Dictionary<long, string> _RpcMerName;
        public TransactionTree[] GetTree (long id)
        {
            BasicTransaction[] trans = this._Transaction.GetTranNode(id);
            if (trans.IsNull())
            {
                return null;
            }
            this._SystemTypes = this._ServerType.GetNames(trans.Distinct(a => a.SystemType));
            this._ServerName = this._Server.GetNames(trans.Distinct(a => a.ServerId));
            this._RegionName = this._Region.GetNames(trans.Distinct(a => a.RegionId));
            this._RpcMerName = this._RpcMer.GetNames(trans.Distinct(a => a.RpcMerId));
            return trans.Convert(a => a.ParentId == id, c => this._GetTree(c, trans));
        }
        private TransactionTree _GetTree (BasicTransaction tran, BasicTransaction[] list)
        {
            TransactionTree tree = tran.ConvertMap<BasicTransaction, TransactionTree>();
            tree.ServerName = this._ServerName.GetValueOrDefault(tran.ServerId);
            tree.SystemTypeName = this._SystemTypes.GetValueOrDefault(tran.SystemType);
            tree.Region = this._RegionName.GetValueOrDefault(tran.RegionId);
            tree.RpcMerName = this._RpcMerName.GetValueOrDefault(tran.RpcMerId);
            tree.Children = list.Convert(a => a.ParentId == tran.Id, c => this._GetTree(c, list));
            return tree;
        }
    }
}
