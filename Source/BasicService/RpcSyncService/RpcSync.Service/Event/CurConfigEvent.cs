using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.CurConfig;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 当前配置
    /// </summary>
    internal class CurConfigEvent : IRpcApiService
    {
        private readonly IServerCurConfigCollect _Service;

        public CurConfigEvent (IServerCurConfigCollect service)
        {
            this._Service = service;
        }
        public void SetCurConfig (SetCurConfig set, MsgSource source)
        {
            this._Service.Sync(source.ServerId, set.Config);
        }
    }
}
