using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshDictateLimit : IRpcEvent
    {
        private readonly IServerDictateLimitCollect _DictateLimit;
        public RefreshDictateLimit(IServerDictateLimitCollect dictateLimit)
        {
            this._DictateLimit = dictateLimit;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            _DictateLimit.Refresh(long.Parse(param["ServerId"]));
        }

    }
}
