using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent;
namespace RpcSync.Service.Event
{
    internal class SysEventLogEvent : IRpcApiService
    {
        private readonly ISystemEventLogService _Service;

        public SysEventLogEvent (ISystemEventLogService service)
        {
            this._Service = service;
        }

        public void AddSysEventLog (AddSysEventLog add, MsgSource source)
        {
            this._Service.Add(add.Logs, source);
        }
    }
}
