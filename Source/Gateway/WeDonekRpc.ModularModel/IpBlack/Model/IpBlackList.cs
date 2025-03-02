using System.Numerics;

namespace WeDonekRpc.ModularModel.IpBlack.Model
{
    public class IpBlackList
    {
        /// <summary>
        /// 远程版本号
        /// </summary>
        public long Ver
        {
            get;
            set;
        }
        public int Count
        {
            get;
            set;
        }
        public long[] Ip4
        {
            get;
            set;
        }
        public BigInteger[] Ip6
        {
            get;
            set;
        }
        public BigInteger[] DropIp6
        {
            get;
            set;
        }
        public RemoteRangeBlackIp[] Range
        {
            get;
            set;
        }
        public DropBlackIp[] DropIp4
        {
            get;
            set;
        }
    }
}
