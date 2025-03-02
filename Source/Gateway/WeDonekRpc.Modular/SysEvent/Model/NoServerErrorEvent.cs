using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    internal class NoServerErrorEvent : BasicEvent
    {
        public NoServerErrorEvent (ServiceSysEvent obj) : base(obj)
        {
            NoServerErrorConfig config = obj.EventConfig.Json<NoServerErrorConfig>();
            this.SystemType = config.SystemType;
            this.IsLimit = !config.SystemType.IsNull();
            this.Interval = config.Interval == 0 ? 1 : config.Interval;
        }
        public bool IsLimit { get; }
        public string[] SystemType { get; }
        public int Interval { get; }
    }
}
