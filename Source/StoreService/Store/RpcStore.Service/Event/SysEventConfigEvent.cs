using WeDonekRpc.Client.Interface;
using RpcStore.Collect;
using RpcStore.RemoteModel.SysEventConfig;
using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.Service.Event
{
    internal class SysEventConfigEvent : IRpcApiService
    {
        private readonly ISystemEventConfigCollect _Service;

        public SysEventConfigEvent (ISystemEventConfigCollect service)
        {
            this._Service = service;
        }

        public SystemEventConfig GetSystemEvent (GetSystemEvent obj)
        {
            return this._Service.Get(obj.Id);
        }

        public SystemEventItem[] GetSysEventItems ()
        {
            return this._Service.Gets();
        }
    }
}
