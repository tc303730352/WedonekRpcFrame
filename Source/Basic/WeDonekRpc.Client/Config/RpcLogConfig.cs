using System.Collections.Concurrent;
using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Config
{
    internal class RpcLogConfig : IRpcLogConfig
    {
        private static readonly Dictionary<string, LocalLogSet> _defSet = new Dictionary<string, LocalLogSet>
        {
            {"ConfigRefresh", new LocalLogSet("Rpc_Config", LogGrade.Information)},
            {"RpcConLog", new LocalLogSet("Rpc_ConLog", LogGrade.Information)},
            {"RpcConFailLog", new LocalLogSet("Rpc_ConLog", LogGrade.WARN)},
            {"RpcRouteErrorLog", new LocalLogSet("Rpc_RouteLog", LogGrade.WARN)},
            {"RpcRouteLog", new LocalLogSet("Rpc_RouteLog", LogGrade.DEBUG)},
            {"RpcMsg", new LocalLogSet("Rpc_Msg", LogGrade.DEBUG)},
            {"kafka", new LocalLogSet("Rpc_Kafka", LogGrade.DEBUG)},
            {"CentralMsg", new LocalLogSet("Rpc_CentralMsg", LogGrade.Information)},
            {"RpcQueueError", new LocalLogSet("RpcQueueError", LogGrade.Information)},
            {"RpcErrorLog", new LocalLogSet("Rpc_Log", LogGrade.ERROR)},
            {"RpcLog", new LocalLogSet("Rpc_Log", LogGrade.Information)},
            {"RpcNodeLog", new LocalLogSet("Rpc_Log", LogGrade.WARN)},
            {"RpcLocalError", new LocalLogSet("Rpc_Log", LogGrade.ERROR)},
        };
        private static readonly ConcurrentDictionary<string, LocalLogSet> _LogSet;

        static RpcLogConfig ()
        {
            _LogSet = new ConcurrentDictionary<string, LocalLogSet>(_defSet);
        }
        public RpcLogConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("rpc:RpcLog");
            section.AddRefreshEvent(this.Config_RefreshEvent);
        }

        private void Config_RefreshEvent (IConfigSection config, string name)
        {
            if (name.IsNull())
            {
                Dictionary<string, LocalLogSet> logSet = config.GetValue<Dictionary<string, LocalLogSet>>(_defSet);
                if (logSet != null && logSet.Count > 0)
                {
                    logSet.ForEach((a, b) => this._SetLogConfig(a, b));
                }
            }
            else
            {
                int index = name.IndexOf(':');
                if (index != -1)
                {
                    name = name.Substring(0, index);
                }
                LocalLogSet set = config.GetValue<LocalLogSet>(name);
                this._SetLogConfig(name, set);
            }
        }
        private void _SetLogConfig (string name, LocalLogSet set)
        {
            if (_LogSet.TryGetValue(name, out LocalLogSet logSet))
            {
                logSet.LogGrade = set.LogGrade;
                logSet.LogGroup = set.LogGroup;
            }
            else
            {
                _ = _LogSet.TryAdd(name, set);
            }
        }

        public bool CheckIsRecord (string key, out LocalLogSet logSet)
        {
            if (_LogSet.TryGetValue(key, out logSet))
            {
                return LogSystem.CheckIsRecord(logSet.LogGrade);
            }
            return false;
        }
        public bool CheckIsRecord (string key, LogGrade grade, out LocalLogSet logSet)
        {
            if (_LogSet.TryGetValue(key, out logSet))
            {
                return LogSystem.CheckIsRecord(grade);
            }
            return false;
        }

        public bool CheckIsRecord (string key, ErrorException e, out LocalLogSet logSet)
        {
            if (_LogSet.TryGetValue(key, out logSet))
            {
                return LogSystem.CheckIsRecord(e.LogGrade >= logSet.LogGrade ? e.LogGrade : logSet.LogGrade);
            }
            return false;
        }
    }
    public class LocalLogSet
    {
        public LocalLogSet ()
        {
        }
        public LocalLogSet (string group, LogGrade grade)
        {
            this.LogGroup = group;
            this.LogGrade = grade;
        }
        public string LogGroup { get; set; }

        public LogGrade LogGrade { get; set; }
    }
}
