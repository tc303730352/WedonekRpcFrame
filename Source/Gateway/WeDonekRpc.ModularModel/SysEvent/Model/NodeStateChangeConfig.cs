using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    public class NodeStateChangeConfig
    {
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
