using WeDonekRpc.Helper;
using System.Numerics;

namespace WeDonekRpc.ApiGateway.IpBlack.Model
{
    internal class RemoteIp
    {
        private readonly byte _Head;
        public RemoteIp(bool isIp6)
        {
            if (isIp6)
            {
                this._Head = 2;
                this.Size = 17;
            }
            else
            {
                this._Head = 1;
                this.Size = 5;
            }
        }
        public int Write(byte[] stream, BigInteger ip, int index)
        {
            stream[index] = this._Head;
            index += 1;
            index += BitHelper.WriteByte(ip, stream, index);
            return index;
        }
        public int Write(byte[] stream, long ip, int index)
        {
            stream[index] = this._Head;
            index += 1;
            index += BitHelper.WriteByte((uint)ip, stream, index);
            return index;
        }
        public int Size { get; }

    }
}
