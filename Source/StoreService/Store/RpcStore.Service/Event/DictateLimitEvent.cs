using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.DictateLimit;
using RpcStore.RemoteModel.DictateLimit.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 服务指令限流配置
    /// </summary>
    internal class DictateLimitEvent : IRpcApiService
    {
        private IServerDictateLimitService _Service;
        public DictateLimitEvent(IServerDictateLimitService service)
        {
            _Service = service;
        }

        public long AddDictateLimit(AddDictateLimit add)
        {
            return _Service.Add(add.Datum);
        }

        public void DeleteDictateLimit(DeleteDictateLimit obj)
        {
            _Service.Delete(obj.Id);
        }

        public DictateLimit GetDictateLimit(GetDictateLimit obj)
        {
            return _Service.Get(obj.Id);
        }

        public PagingResult<DictateLimit> QueryDictateLimit(QueryDictateLimit query)
        {
            return _Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetDictateLimit(SetDictateLimit obj)
        {
            _Service.Set(obj.Id, obj.Datum);
        }
    }
}
