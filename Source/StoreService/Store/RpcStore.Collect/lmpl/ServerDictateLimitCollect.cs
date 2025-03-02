using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerDictateLimitCollect : IServerDictateLimitCollect
    {
        private readonly IServerDictateLimitDAL _BasicDAL;
        public ServerDictateLimitCollect (IServerDictateLimitDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public long Add (DictateLimitAdd datum)
        {
            if (this._BasicDAL.CheckIsExists(datum.ServerId, datum.Dictate))
            {
                throw new ErrorException("rpc.store.dictate.limit.repeat");
            }
            ServerDictateLimitModel add = datum.ConvertMap<DictateLimitAdd, ServerDictateLimitModel>();
            return this._BasicDAL.Add(add);
        }
        public ServerDictateLimitModel Get (long id)
        {
            ServerDictateLimitModel limit = this._BasicDAL.Get(id);
            if (limit == null)
            {
                throw new ErrorException("rpc.store.dictate.not.find");
            }
            return limit;
        }
        public void Delete (ServerDictateLimitModel limit)
        {
            this._BasicDAL.Delete(limit.Id);
        }
        public bool SetDictateLimit (ServerDictateLimitModel source, DictateLimitSet limit)
        {
            if (limit.IsEquals(source))
            {
                return false;
            }
            this._BasicDAL.Set(source.Id, limit);
            return true;
        }
        public ServerDictateLimitModel[] Query (DictateLimitQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
    }
}
