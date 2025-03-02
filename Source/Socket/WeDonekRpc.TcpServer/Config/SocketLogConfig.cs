using System.Collections.Concurrent;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.TcpServer.Config
{
    internal class SocketLogConfig
    {
        private static readonly IConfigSection _Config;
        private static LogGrade _DefLogGrade;
        private static readonly ConcurrentDictionary<string, LogGrade> _LogGrade = new ConcurrentDictionary<string, LogGrade>();
        static SocketLogConfig ()
        {
            _Config = LocalConfig.Local.GetSection("socket:Service:Log");
            _Config.AddRefreshEvent(_Refresh);
        }

        private static void _Refresh (IConfigSection section, string name)
        {
            if (name == string.Empty)
            {
                _DefLogGrade = section.GetValue("default", LogGrade.DEBUG);
                section.Keys.ForEach(c =>
                {
                    LogGrade grade = section.GetValue<LogGrade>(c);
                    _ = _LogGrade.TryAdd(c, grade);
                });
                return;
            }
            if (name == "default")
            {
                _DefLogGrade = section.GetValue("default", LogGrade.DEBUG);
                return;
            }
            LogGrade? grade = section.GetValue<LogGrade?>(name, null);
            if (grade.HasValue)
            {
                _LogGrade.AddOrSet(name, grade.Value);
            }
            else
            {
                _ = _LogGrade.TryRemove(name, out _);
            }
        }

        public static bool CheckIsRecord (string name, out LogGrade grade)
        {
            if (_LogGrade.TryGetValue(name, out grade))
            {
                return LogSystem.CheckIsRecord(grade);
            }
            return LogSystem.CheckIsRecord(_DefLogGrade);
        }
    }
}
