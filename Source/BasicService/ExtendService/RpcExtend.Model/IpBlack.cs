namespace RpcExtend.Model
{
    public enum IpType
    {
        Ip4 = 0,
        Ip6 = 1,
    }
    public class IpBlack : IEquatable<IpBlack>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        public IpType IpType
        {
            get;
            set;
        }
        public string Ip6
        {
            get;
            set;
        }
        /// <summary>
        /// 开始IP
        /// </summary>
        public long Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 结束Ip
        /// </summary>
        public long? EndIp
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDrop
        {
            get;
            set;
        }

        private int _Code = -1;
        public override int GetHashCode ()
        {
            if (this._Code == -1)
            {
                this._Code = string.Concat(this.Ip, "_", this.EndIp.GetValueOrDefault()).GetHashCode();
            }
            return this._Code;
        }
        public override bool Equals (object obj)
        {
            if (obj is IpBlack i)
            {
                return ( i.Ip == this.Ip && !i.EndIp.HasValue && !this.EndIp.HasValue ) || ( i.Ip == this.Ip && i.EndIp.HasValue && this.EndIp.HasValue && i.EndIp.Value == this.EndIp.Value );
            }
            return false;
        }

        public bool Equals (IpBlack other)
        {
            if (other == null)
            {
                return false;
            }
            return ( other.Ip == this.Ip && !other.EndIp.HasValue && !this.EndIp.HasValue ) || ( other.Ip == this.Ip && other.EndIp.HasValue && this.EndIp.HasValue && other.EndIp.Value == this.EndIp.Value );
        }
    }
}
