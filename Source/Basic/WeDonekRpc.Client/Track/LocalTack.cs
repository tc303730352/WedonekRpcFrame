using System.Linq;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track.Config;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Track
{
    /// <summary>
    /// 本地链路记录器
    /// </summary>
    internal class LocalTack : ITack
    {
        private readonly IDelayDataSave<TrackBody> _TackList = null;
        private readonly LocalTrackConfig _Config;
        private readonly string _ServerName;
        public LocalTack ( LocalTrackConfig config )
        {
            this._Config = config ?? new LocalTrackConfig
            {
                Dictate = "SysTrace",
                SystemType = "sys.extend"
            };
            this._TackList = new DelayDataSave<TrackBody>(new SaveDelayData<TrackBody>(this._Save), 15, 10);
        }

        private void _Save ( ref TrackBody[] datas )
        {
            SysTraceLog[] logs = datas.ConvertAll(a =>
           {
               SysTraceLog add = new SysTraceLog
               {
                   Dictate = a.Dictate,
                   StageType = a.StageType,
                   Duration = a.Duration,
                   Show = a.Show,
                   TraceId = a.Trace.ToTraceId(),
                   ParentId = a.Trace.ParentId,
                   RemoteId = a.RemoteId,
                   SpanId = a.Trace.SpanId,
                   Begin = a.Time,
                   Args = a.Args.ToDictionary(a => a.Key, a => a.Value)
               };
               if ( a.RemoteIp.IsNotNull() && add.Args.ContainsKey("RemoteIp") )
               {
                   add.Args["RemoteIp"] = a.RemoteIp;
               }
               return add;
           });
            IRemoteConfig config = new IRemoteConfig(this._Config.Dictate, this._Config.SystemType, false, true)
            {
                IsProhibitTrace = true
            };
            if ( !RemoteCollect.Send<SysTrace>(config, new SysTrace
            {
                Logs = logs
            }, out string error) )
            {
                new WarnLog("Trace.submit.error", "链路跟踪数据提交失败!", "Rpc_Trace")
                                {
                                        { "Dictate",this._Config.Dictate},
                                        {"SystemType",this._Config.SystemType },
                                        {"Trace",logs.ToJson() }
                                }.Save();
            }
        }

        public void AddTrace ( TrackBody track )
        {
            this._TackList.AddData(track);
        }

        public void Dispose ()
        {
            this._TackList.Dispose();
        }
    }
}
