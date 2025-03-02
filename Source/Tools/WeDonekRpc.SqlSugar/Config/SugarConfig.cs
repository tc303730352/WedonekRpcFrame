using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.SqlSugar.Config
{
    internal class SugarConfig : ISugarConfig
    {
        private static readonly Dictionary<string, LogConfig> _defSet = new Dictionary<string, LogConfig>
        {
            {"Error", new LogConfig("SqlSugar", LogGrade.ERROR)},
            {"Log", new LogConfig("SqlSugar", LogGrade.Trace)}
        };
        private Dictionary<string, LogConfig> _LogConfig = [];
        public SugarConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("SqlSugar");
            this.IsLocalLog = section.GetValue<bool>("IsLocalLog", false);
            this.StringIsAutoEmpty = section.GetValue<bool>("StringIsAutoEmpty", true);
            this._LogConfig = section.GetValue("Log", _defSet);
            section.AddRefreshEvent(this._InitConfig);
        }

        private void _InitConfig (IConfigSection config, string name)
        {
            if (name == "IsLocalLog")
            {
                this.IsLocalLog = config.GetValue<bool>("IsLocalLog", false);
            }
            else if (name == "Log" || name.StartsWith("Log:"))
            {
                this._LogConfig = config.GetValue("Log", _defSet);
            }
            else if (name == "StringIsAutoEmpty")
            {
                this.StringIsAutoEmpty = config.GetValue<bool>("StringIsAutoEmpty", true);
            }
        }
        public bool StringIsAutoEmpty
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 是否本地日志
        /// </summary>
        public bool IsLocalLog
        {
            get;
            private set;
        } = false;

        public bool CheckIsRecord (string key, out LogConfig logSet)
        {
            if (this._LogConfig.TryGetValue(key, out logSet))
            {
                return LogSystem.CheckIsRecord(logSet.LogGrade);
            }
            return false;
        }

        public bool CheckIsRecord (string key, ErrorException e, out LogConfig logSet)
        {
            if (this._LogConfig.TryGetValue(key, out logSet))
            {
                return LogSystem.CheckIsRecord(e);
            }
            return false;
        }

    }
}
