using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Modular.Config;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.ModularModel.Visit;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.Modular.Service
{
    internal class VisitService
    {
        private static Timer _UpTimer;
        private static readonly ConcurrentDictionary<string, DictateVisit> _Census = new ConcurrentDictionary<string, DictateVisit>();
        static VisitService ()
        {
            RpcClient.Config.GetSection("rpcassembly:Visit").AddRefreshEvent(_Refresh);
        }
        public static Action<bool> StatusRefresh;

        private static void _Enable ()
        {
            _UpTimer?.Dispose();
            int time = Config.Interval * 1000;
            _UpTimer = new Timer(new TimerCallback(_UpCensus), null, time, time);
            StatusRefresh(Config.IsEnable);
        }
        private static void _Stop ()
        {
            _UpTimer?.Dispose();
            StatusRefresh(Config.IsEnable);
        }
        private static void _Refresh (IConfigSection config, string name)
        {
            VisitConfig visit = config.GetValue<VisitConfig>(new VisitConfig());
            if (Config == null)
            {
                Config = visit;
                if (visit.IsEnable)
                {
                    _UpTimer?.Dispose();
                    int time = Config.Interval * 1000;
                    _UpTimer = new Timer(new TimerCallback(_UpCensus), null, time, time);
                }
            }
            else if (visit.IsEnable != Config.IsEnable)
            {
                Config = visit;
                if (visit.IsEnable)
                {
                    _Enable();
                }
                else
                {
                    _Stop();
                }
            }
            else if (visit.IsEnable && visit.Interval != Config.Interval)
            {
                Config = visit;
                int time = Config.Interval * 1000;
                if (!_UpTimer.Change(time, time))
                {
                    _UpTimer.Dispose();
                    _UpTimer = new Timer(new TimerCallback(_UpCensus), null, time, time);
                }
            }
        }
        public static VisitConfig Config
        {
            get;
            private set;
        }
        private static void _UpCensus (object state)
        {
            if (_Census.IsEmpty)
            {
                return;
            }
            string[] keys = _Census.Keys.ToArray();
            RpcDictateVisit[] list = keys.Convert(a => _Census[a].Sync(Config.Interval));
            if (list.Length > 0)
            {
                new AddVisitLog
                {
                    Logs = list
                }.Send();
            }
        }
        private static DictateVisit _Get (string dictate)
        {
            if (_Census.TryGetValue(dictate, out DictateVisit visit))
            {
                return visit;
            }
            visit = new DictateVisit(dictate);
            return _Census.GetOrAdd(dictate, visit);
        }
        public static void Success (string dictate)
        {
            DictateVisit obj = _Get(dictate);
            obj.Success();
        }
        public static void Failure (string dictate)
        {
            DictateVisit obj = _Get(dictate);
            obj.Failure();
        }
        public static void Regs (RpcVisit[] list)
        {
            new RegVisitNode
            {
                Visits = list,
            }.Send();
        }
        public static void Reg (RpcVisit visit)
        {
            new AddVisitNode
            {
                Visit = visit,
            }.Send();
        }
    }
}
