using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Collect.Model
{
    /// <summary>
    /// 服务负载
    /// </summary>
    public class Transmit : ServerTransmit, IEquatable<Transmit>
    {
        private string _Key;
        public string Key
        {
            get
            {
                if (this._Key == null)
                {
                    if (this.TransmitType == TransmitType.FixedType)
                    {
                        this._Key = string.Join('_', this.ServerCode, (int)this.TransmitType, this.Value).GetMd5();
                    }
                    else if (this.TransmitType == TransmitType.close)
                    {
                        this._Key = string.Concat(this.ServerCode, '_', (int)this.TransmitType).GetMd5();
                    }
                    else
                    {
                        StringBuilder str = new StringBuilder();
                        _ = str.AppendFormat("{0}_{1}_", this.ServerCode, (int)this.TransmitType);
                        this.Range.ForEach(c =>
                        {
                            _ = str.Append(c.Key);
                        });
                        this._Key = str.ToString().GetMd5();
                    }
                }
                return this._Key;
            }
        }
        /// <summary>
        /// 服务节点组别编号
        /// </summary>
        public string ServerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Ver { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get;
            set;
        }
        public override bool Equals (object obj)
        {
            if (obj is Transmit i)
            {
                return i.Scheme == this.Scheme && i.ServerCode == this.ServerCode && i.Ver == this.Ver;
            }
            return false;
        }

        public bool Equals (Transmit other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Scheme == this.Scheme && other.ServerCode == this.ServerCode && other.Ver == this.Ver;
        }

        public override int GetHashCode ()
        {
            return string.Concat(this.Scheme, "_", this.ServerCode, "_", this.Ver).GetHashCode();
        }
    }
}
