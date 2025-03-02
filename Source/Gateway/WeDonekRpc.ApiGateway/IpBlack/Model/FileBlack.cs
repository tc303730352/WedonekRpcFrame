using System.Collections.Generic;
using System.Numerics;

namespace WeDonekRpc.ApiGateway.IpBlack.Model
{
    internal class FileBlack
    {
        public FileCache File;
        public List<IpBlackTo> black = new List<IpBlackTo>();
        public List<long> ip4 = new List<long>();
        public List<long> dropIp4 = new List<long>();
        public List<BigInteger> ip6 = new List<BigInteger>();
        public List<BigInteger> dropIp6 = new List<BigInteger>();
    }
}
