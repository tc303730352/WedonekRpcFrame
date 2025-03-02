namespace RpcSync.Model
{
    /// <summary>
    /// 节点事件
    /// </summary>
    public class EventSwitch : IEquatable<EventSwitch>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务ID
        /// </summary>
        public long ServerId { get; set; }
        public string EventKey
        {
            get;
            set;
        }
        /// <summary>
        /// 模块名
        /// </summary>
        public string Module
        {
            get;
            set;
        }

        /// <summary>
        /// 事件配置项
        /// </summary>
        public string EventConfig
        {
            get;
            set;
        }
        public bool Equals (EventSwitch? other)
        {
            return other.EventKey == this.EventKey;
        }
        public override int GetHashCode ()
        {
            return this.EventKey.GetHashCode();
        }
        public override bool Equals (object? obj)
        {
            if (obj is EventSwitch i)
            {
                return i.EventKey == this.EventKey;
            }
            return false;
        }
    }
}
