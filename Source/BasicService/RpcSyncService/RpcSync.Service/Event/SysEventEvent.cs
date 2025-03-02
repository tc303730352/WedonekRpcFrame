using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent;
using WeDonekRpc.ModularModel.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Msg;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 系统事件
    /// </summary>
    internal class SysEventEvent : IRpcApiService
    {
        private readonly ISysEventService _Service;
        public SysEventEvent (ISysEventService service)
        {
            this._Service = service;
        }
        public ServiceSysEvent[] GetEnableSysEvent (GetEnableSysEvent obj, MsgSource source)
        {
            return this._Service.GetEnableEvents(source, obj.Module);
        }

        public void RefreshRpcEvent (RefreshRpcEvent obj)
        {
            this._Service.Refresh(obj.RpcMerId, obj.Module);
        }

    }
}
