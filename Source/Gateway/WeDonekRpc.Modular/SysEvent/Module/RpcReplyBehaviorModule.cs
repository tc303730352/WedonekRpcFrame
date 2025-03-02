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
    internal class RpcReplyBehaviorModule : IRpcEventModule
    {
        [ThreadStatic]
        private static Stopwatch _RunTime;

        /// <summary>
        /// 监控配置
        /// </summary>
        private readonly BehaviorEvent[] _EventList;

        private readonly uint _MinThreshold = 0;

        private readonly IRpcEventLogService _Service;

        private readonly IRpcService _RpcService;

        public RpcReplyBehaviorModule ( ServiceSysEvent[] obj, IRpcEventLogService service, IRpcService rpcService )
        {
            this._RpcService = rpcService;
            this._Service = service;
            this._EventList = obj.Select(a => new BehaviorEvent(a)).OrderByDescending(a => a.Threshold).ToArray();
            if ( !this._EventList.IsNull() )
            {
                this._MinThreshold = this._EventList[^1].Threshold;
            }
        }
        public string Module => "RpcReplyBehavior";

        public void Dispose ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._RpcService.ReceiveMsg -= this._ReplyBegin;
                this._RpcService.ReceiveEnd -= this._ReplyEnd;
            }
        }

        public void Init ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._RpcService.ReceiveMsg += this._ReplyBegin;
                this._RpcService.ReceiveEnd += this._ReplyEnd;
            }
        }

        private void _ReplyEnd ( IMsg msg, TcpRemoteReply reply )
        {
            _RunTime.Stop();
            long time = _RunTime.ElapsedMilliseconds;
            if ( time >= this._MinThreshold )
            {
                this._WriteLog((uint)time, msg, reply);
            }
        }

        private void _ReplyBegin ( IMsg msg )
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

        private void _WriteLog ( uint time, IMsg msg, TcpRemoteReply result )
        {
            BehaviorEvent ev = this._EventList.Find(c => c.Threshold <= time);
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "duration", time.ToString() },
                { "dicate", msg.MsgKey },
                { "remote_id", msg.Source.ServerId.ToString() },
                { "msgid", msg.MsgId.ToString() },
            };
            if ( result != null )
            {
                args.Add("iserror", result.IsError.ToString());
                if ( result.IsError || ev.RecordRange == LogRecordRange.完整 )
                {
                    args.Add("reply", result.MsgBody);
                }
                if ( ev.RecordRange == LogRecordRange.完整 || ev.RecordRange == LogRecordRange.记录发送参数 )
                {
                    args.Add("receive", msg.MsgBody);
                }
            }
            this._Service.AddLog(ev, args);
        }

    }
}
