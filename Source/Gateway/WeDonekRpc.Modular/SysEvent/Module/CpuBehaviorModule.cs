using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Helper;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Module
{
    internal class CpuBehaviorModule : IRpcEventModule
    {
        private readonly IRpcEventLogService _Service;
        private readonly CpuResourceEvent[] _EventList;
        private int _TriggerTime = 0;

        private bool _IsActive = false;
        private readonly int _MinThreshold = 0;

        private readonly int _MinTime = 0;

        private CpuResourceEvent _Cur;
        private int _LastUploadTime;

        private Timer _CheckCpu;


        public CpuBehaviorModule ( ServiceSysEvent[] obj, IRpcEventLogService service )
        {
            this._Service = service;
            this._EventList = obj.Select(a => new CpuResourceEvent(a)).OrderByDescending(a => a.Threshold).ToArray();
            if ( this._EventList.Length > 0 )
            {
                CpuResourceEvent min = this._EventList[^1];
                this._MinThreshold = min.Threshold;
                this._MinTime = min.SustainTime;
            }
        }
        public string Module => "CpuBehavior";
        public void Dispose ()
        {
            this._CheckCpu?.Dispose();
        }

        public void Init ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._CheckCpu = new Timer(new TimerCallback(this._Check), null, 1000, 1000);
            }
        }
        private void _WriteLog ( int time, double rate )
        {
            if ( this._Cur == null || this._Cur.Threshold > rate )
            {
                this._Cur = this._EventList.Find(c => c.Threshold <= rate && c.SustainTime <= time);
            }
            else if ( ( time - this._LastUploadTime ) < this._Cur.Interval )
            {
                return;
            }
            this._LastUploadTime = time;
            Dictionary<string, string> args = new Dictionary<string, string>
             {
                  { "duration", time.ToString() },
                  {"rate", rate.ToString() },
                  {"threshold", this._Cur.Threshold.ToString() },
                  {"begintime", HeartbeatTimeHelper.GetTime(this._TriggerTime).ToString("yyyy-MM-dd HH:mm:ss")}
             };
            this._Service.AddLog(this._Cur, args);
        }
        private void _Check ( object state )
        {
            double rate = Math.Round(CurProcessHelper.CpuRate / 100.0, 2);
            if ( rate >= this._MinThreshold )
            {
                if ( !this._IsActive )
                {
                    this._IsActive = true;
                    this._Cur = null;
                    this._TriggerTime = HeartbeatTimeHelper.HeartbeatTime;
                    return;
                }
                int time = HeartbeatTimeHelper.HeartbeatTime - this._TriggerTime;
                if ( time < this._MinTime )
                {
                    return;
                }
                this._WriteLog(time, rate);
            }
            else if ( this._IsActive )
            {
                this._IsActive = false;
            }
        }
    }
}
