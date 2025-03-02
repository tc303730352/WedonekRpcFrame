using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    internal class StateChangeEvent : BasicEvent
    {
        public StateChangeEvent (ServiceSysEvent obj) : base(obj)
        {
            NodeStateChangeConfig config = obj.EventConfig.Json<NodeStateChangeConfig>();
            this.CurState = config.CurState;
            this.OldState = config.OldState;
        }
        /// <summary>
        /// 新状态
        /// </summary>
        public UsableState CurState { get; set; }
        /// <summary>
        /// 旧状态
        /// </summary>
        public UsableState OldState { get; set; }

    }
}
