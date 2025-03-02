using System.Collections.Generic;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Module
{
    internal class RemoteStateChangeModule : IRpcEventModule
    {
        private readonly IRpcService _RpcService;
        private readonly IRpcEventLogService _Service;
        private readonly StateChangeEvent[] _EventList;

        public string Module => "RemoteStateChange";
        public RemoteStateChangeModule ( ServiceSysEvent[] obj, IRpcEventLogService service, IRpcService rpcService )
        {
            this._RpcService = rpcService;
            this._Service = service;
            this._EventList = obj.ConvertAll(a => new StateChangeEvent(a));
        }
        public void Dispose ()
        {
            this._RpcService.RemoteState -= this._RpcService_RemoteState;
        }

        private void _RpcService_RemoteState ( IRemote remote, UsableState oldState, UsableState state )
        {
            foreach ( StateChangeEvent c in this._EventList )
            {
                if ( c.CurState == state && c.OldState == oldState )
                {
                    this._WriteLog(remote, c);
                    break;
                }
            }
        }
        private void _WriteLog ( IRemote remote, StateChangeEvent ev )
        {
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                  { "remote_id", remote.ServerId.ToString() },
                  { "sourcestate", ev.OldState.ToString() },
                   {"verNum",RpcClient.CurrentSource.VerNum.ToString() },
                  {"newstate",ev.CurState.ToString() }
             };
            if ( ev.CurState == UsableState.降级 )
            {
                args.Add("reducetime", HeartbeatTimeHelper.GetTime(remote.ReduceTime).ToString("HH:mm:ss"));
            }
            this._Service.AddLog(ev, args);
        }

        public void Init ()
        {
            this._RpcService.RemoteState += this._RpcService_RemoteState;
        }
    }
}
