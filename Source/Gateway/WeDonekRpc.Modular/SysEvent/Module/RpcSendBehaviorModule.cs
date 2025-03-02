using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Module
{
    /// <summary>
    /// Rpc指令监控事件
    /// </summary>
    internal class RpcSendBehaviorModule : IRpcEventModule
    {
        [ThreadStatic]
        private static Stopwatch _RunTime;

        /// <summary>
        /// 监控配置
        /// </summary>
        private BehaviorEvent[] _EventList;

        private uint _MinThreshold = 0;

        private readonly IRpcEventLogService _Service;

        private readonly IRpcService _RpcService;

        public RpcSendBehaviorModule ( ServiceSysEvent[] obj, IRpcEventLogService service, IRpcService rpcService )
        {
            this._RpcService = rpcService;
            this._Service = service;
            this._EventList = obj.ConvertAll(a => new BehaviorEvent(a));

        }
        public string Module => "RpcSendBehavior";

        public void Dispose ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._RpcService.SendIng -= this._SendIng;
                this._RpcService.SendComplate -= this._SendComplate;
            }
        }

        public void Init ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._EventList = this._EventList.OrderByDescending(a => a.Threshold).ToArray();
                this._MinThreshold = this._EventList[^1].Threshold;
                this._RpcService.SendIng += this._SendIng;
                this._RpcService.SendComplate += this._SendComplate;
            }
        }
        private void _WriteLog ( uint time, ref SendBody send, IRemoteResult result )
        {
            BehaviorEvent ev = this._EventList.Find(c => c.Threshold <= time);
            Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "duration", time.ToString() },
                    { "dicate", send.dicate },
                    { "remote_id", result.RemoteServerId.ToString() },
                };
            if ( result != null )
            {
                args.Add("iserror", result.IsError.ToString());
                if ( result.IsError )
                {
                    args.Add("error", result.ErrorMsg);
                }
                else if ( ev.RecordRange == LogRecordRange.完整 )
                {
                    args.Add("reply", result.GetResult());
                }
                if ( ev.RecordRange == LogRecordRange.完整 || ev.RecordRange == LogRecordRange.记录发送参数 )
                {
                    args.Add("send", send.model.ToJson());
                }
            }
            this._Service.AddLog(ev, args);
        }
        private void _SendComplate ( ref SendBody send, IRemoteResult result )
        {
            _RunTime.Stop();
            long time = _RunTime.ElapsedMilliseconds;
            if ( time >= this._MinThreshold )
            {
                this._WriteLog((uint)time, ref send, result);
            }
        }

        private void _SendIng ( ref SendBody send, int sendNum )
        {
            if ( _RunTime == null )
            {
                _RunTime = new Stopwatch();
                _RunTime.Start();
            }
            else
            {
                _RunTime.Restart();
            }
        }
    }
}
