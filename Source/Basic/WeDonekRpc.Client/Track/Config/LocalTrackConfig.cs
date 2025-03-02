namespace WeDonekRpc.Client.Track.Config
{
    /// <summary>
    /// 本地链路配置
    /// </summary>
    public class LocalTrackConfig
    {
        /// <summary>
        /// 接收的指令
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }
        /// <summary>
        /// 接收的节点
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj is LocalTrackConfig i)
            {
                return i.Dictate == this.Dictate && this.SystemType == i.SystemType;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return string.Concat(this.Dictate, "_", this.SystemType).GetHashCode();
        }
    }
}
