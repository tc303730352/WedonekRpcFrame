using System;

using Microsoft.Extensions.Logging;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
namespace WeDonekRpc.CacheClient.Log
{
    internal class MemcachedLog : ILogger
    {
        private readonly string _Name;

        public MemcachedLog(string name)
        {
            this._Name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return LogSystem.CheckIsRecord(this._GetLogGrade(logLevel));
        }

        private LogGrade _GetLogGrade(LogLevel level)
        {
            if (level == LogLevel.Trace)
            {
                return LogGrade.Trace;
            }
            else if (level == LogLevel.Debug)
            {
                return LogGrade.DEBUG;
            }
            else if (level == LogLevel.Error)
            {
                return LogGrade.ERROR;
            }
            else if (level == LogLevel.Warning)
            {
                return LogGrade.WARN;
            }
            else
            {
                return LogGrade.Critical;
            }
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception e, Func<TState, Exception, string> formatter)
        {
            LogGrade grade = this._GetLogGrade(logLevel);
            LogInfo log;
            if (e == null)
            {
                log = new LogInfo(grade, "Memcached")
                {
                    LogTitle = "memcached日志",
                    LogContent = _Name
                };
            }
            else
            {
                log = new LogInfo(e, "Memcached", grade)
                {
                    LogTitle = "memcached日志",
                    LogContent = _Name
                };
            }
            log.Add("eventId", eventId.Id);
            log.Add("name", eventId.Name);
            log.Add("state", state?.ToString());
            log.Save();
        }
    }
}
