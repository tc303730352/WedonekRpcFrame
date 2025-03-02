using System.Net;

namespace WeDonekRpc.ApiGateway.IpBlack.Model
{

    internal class IpBlackTo
    {
        private readonly long _BeginIp;
        private readonly long _EndIp;

        public IpBlackTo(IPAddress begin, IPAddress end)
        {
            this._BeginIp = begin.ScopeId;
            this._EndIp = end.ScopeId;
        }

        public bool IsDrop { get; set; }
        public bool IsLimit(long ip)
        {
            return this._BeginIp <= ip && ip <= this._EndIp;
        }

        public bool IsEqual(IpBlackTo obj)
        {
            return obj._BeginIp == this._BeginIp && obj._EndIp == this._EndIp;
        }
    }
}
