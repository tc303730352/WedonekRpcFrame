namespace RpcCentral.Model
{
    /// <summary>
    /// 远程配置
    /// </summary>
    public class RemoteConfig : IEquatable<RemoteConfig>
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }

        public override bool Equals (object obj)
        {
            if (obj is RemoteConfig i)
            {
                return i.ServerId == this.ServerId;
            }
            return false;
        }

        public bool Equals (RemoteConfig other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ServerId == this.ServerId;
        }

        public override int GetHashCode ()
        {
            return this.ServerId.GetHashCode();
        }
    }
}
