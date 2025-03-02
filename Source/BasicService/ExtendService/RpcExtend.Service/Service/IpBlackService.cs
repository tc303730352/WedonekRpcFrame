using System.Net;
using System.Numerics;
using System.Text;
using RpcExtend.Collect;
using RpcExtend.Model;
using RpcExtend.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.IpBlack.Model;
namespace RpcExtend.Service.Service
{
    internal class IpBlackService : IIpBlackService
    {
        private readonly IIpBlackListCollect _IpBack;
        public IpBlackService (IIpBlackListCollect ipBack)
        {
            this._IpBack = ipBack;
        }

        public IpBlackList GetBlack (long ver, MsgSource source)
        {
            long endVer = this._IpBack.GetMaxVer(source.RpcMerId, source.SystemType);
            if (ver == endVer)
            {
                return new IpBlackList
                {
                    Count = 0,
                    Ver = ver
                };
            }
            IpBlack[] list = this._IpBack.GetIpBlacks(source.RpcMerId, source.SystemType, ver, endVer);
            if (list.Length == 0)
            {
                return new IpBlackList
                {
                    Count = 0,
                    Ver = endVer
                };
            }
            return new IpBlackList
            {
                Ver = endVer,
                Ip4 = list.Convert(c => c.IsDrop == false && c.IpType == IpType.Ip4 && !c.EndIp.HasValue, c => c.Ip),
                Count = list.Length,
                DropIp4 = list.Convert(c => c.IsDrop && c.IpType == IpType.Ip4, c => new DropBlackIp
                {
                    EndIp = c.EndIp,
                    Ip = c.Ip
                }),
                Range = list.Convert(c => c.IsDrop == false && c.EndIp.HasValue, c => new RemoteRangeBlackIp
                {
                    BeginIp = c.Ip,
                    EndIp = c.EndIp.Value
                }),
                Ip6 = list.Convert(c => c.IsDrop == false && c.IpType == IpType.Ip6, c => new BigInteger(IPAddress.Parse(c.Ip6).GetAddressBytes())),
                DropIp6 = list.Convert(c => c.IsDrop && c.IpType == IpType.Ip6, c => new BigInteger(IPAddress.Parse(c.Ip6).GetAddressBytes()))
            };

        }

        public void Refresh (long rpcMerId, string systemType)
        {
            this._IpBack.Refresh(rpcMerId, systemType);
        }
    }
}
