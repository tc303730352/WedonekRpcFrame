using System;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 负载范围
    /// </summary>
    public class TransmitRange : IEquatable<TransmitRange>
    {
        /// <summary>
        /// 开始位
        /// </summary>
        public long BeginRange
        {
            get;
            set;
        }
        /// <summary>
        /// 结束位
        /// </summary>
        public long EndRange
        {
            get;
            set;
        }
        /// <summary>
        /// 是否固定值
        /// </summary>
        public bool IsFixed
        {
            get;
            set;
        }
        private string _Key = null;

        /// <summary>
        /// 负载配置唯一键
        /// </summary>
        public string Key
        {
            get
            {
                if (this._Key == null)
                {
                    this._Key = string.Join("_", this.IsFixed ? 1 : 0, this.BeginRange, this.EndRange);
                }
                return this._Key;
            }
        }
        public override bool Equals (object obj)
        {
            if (obj is TransmitRange i)
            {
                return i.Key == this.Key;
            }
            return false;
        }
        public override int GetHashCode ()
        {
            return this.Key.GetHashCode();
        }

        public bool Equals (TransmitRange other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Key == this.Key;
        }
    }
}
