using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Helper;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Module
{
    internal class MemoryOccupationModule : IRpcEventModule
    {
        private readonly IRpcEventLogService _Service;
        private readonly MemoryOccupationEvent[] _EventList;
        private readonly long _MinThreshold;
        private readonly int _MinTime;
        private Timer _CheckTimer;
        private bool _IsActive;
        private int _TriggerTime;
        private int _LastUploadTime;

        public MemoryOccupationModule ( ServiceSysEvent[] obj, IRpcEventLogService service )
        {
            this._Service = service;
            this._EventList = obj.Select(a => new MemoryOccupationEvent(a)).OrderByDescending(a => a.Threshold).ToArray();
            if ( this._EventList.Length > 0 )
            {
                MemoryOccupationEvent min = this._EventList[^1];
                this._MinThreshold = min.Threshold;
                this._MinTime = min.SustainTime;
            }

        }
        public string Module => "MemoryOccupation";
        public void Dispose ()
        {
            this._CheckTimer?.Dispose();
        }

        public void Init ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._CheckTimer = new Timer(new TimerCallback(this._Check), null, 1000, 1000);
            }
        }
        private MemoryOccupationEvent _Cur;
        private void _WriteLog ( int time, long memory )
        {
            if ( this._Cur == null || this._Cur.Threshold > memory )
            {
                this._Cur = this._EventList.Find(c => c.Threshold <= memory && c.SustainTime <= time);
            }
            else if ( ( time - this._LastUploadTime ) < this._Cur.Interval )
            {
                return;
            }
            this._LastUploadTime = time;
            string use = Tools.FormatStreamMB(memory);
            string limit = Tools.FormatStreamMB(this._Cur.Threshold);
            Dictionary<string, string> args = new Dictionary<string, string>
             {
                  { "duration", time.ToString() },
                  {"memory", use },
                  {"threshold", limit },
                  {"begintime", HeartbeatTimeHelper.GetTime(this._TriggerTime).ToString("yyyy-MM-dd HH:mm:ss")}
             };
            this._Service.AddLog(this._Cur, args);
        }
        private void _Check ( object state )
        {
            long memory = CurProcessHelper.WorkingSet64;
            if ( memory >= this._MinThreshold )
            {
                if ( !this._IsActive )
                {
                    this._Cur = null;
                    this._TriggerTime = HeartbeatTimeHelper.HeartbeatTime;
                    this._IsActive = true;
                    return;
                }
                int time = HeartbeatTimeHelper.HeartbeatTime - this._TriggerTime;
                if ( time < this._MinTime )
                {
                    return;
                }
                this._WriteLog(time, memory);
            }
            else if ( this._IsActive )
            {
                this._IsActive = false;
            }
        }
    }
}
