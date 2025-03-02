using WeDonekRpc.Client;
using RpcManageClient;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictateLimit.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{

    internal class ServerDictateLimitService : IServerDictateLimitService
    {
        private readonly IServerDictateLimitCollect _Dictate;
        private IRpcServerCollect _RpcServer;

        public ServerDictateLimitService(IServerDictateLimitCollect server,
            IRpcServerCollect rpcServer)
        {
            _RpcServer = rpcServer;
            _Dictate = server;
        }

        public long Add(DictateLimitAdd add)
        {
            long id = _Dictate.Add(add);
            _RpcServer.RefreshDictateLimit(add.ServerId, add.Dictate);
            return id;
        }

        public void Delete(long id)
        {
            ServerDictateLimitModel limit = _Dictate.Get(id);
            _Dictate.Delete(limit);
            _RpcServer.RefreshDictateLimit(limit.ServerId, limit.Dictate);
        }
        public DictateLimit Get(long id)
        {
            ServerDictateLimitModel limit = _Dictate.Get(id);
            return limit.ConvertMap<ServerDictateLimitModel, DictateLimit>();
        }
        public PagingResult<DictateLimit> Query(DictateLimitQuery query, IBasicPage paging)
        {
            ServerDictateLimitModel[] list = _Dictate.Query(query, paging, out int count);
            return new PagingResultTo<ServerDictateLimitModel, DictateLimit>(count, list);
        }

        public void Set(long id, DictateLimitSet set)
        {
            ServerDictateLimitModel limit = _Dictate.Get(id);
            if (_Dictate.SetDictateLimit(limit, set))
            {
                _RpcServer.RefreshDictateLimit(limit.ServerId, limit.Dictate);
            }
        }
    }
}
