using System.Net;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.IpBlack;
using RpcStore.RemoteModel.IpBlack.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.IpBlack;

namespace Store.Gatewary.Modular.Services
{
    internal class IpBlackService : IIpBlackService
    {
        public long AddIpBack (IpBlackAddData datum)
        {
            return new AddIpBack
            {
                Datum = new IpBlackAdd
                {
                    SystemType = datum.SystemType,
                    EndIp = datum.EndIp.IsNull() ? null : IPAddress.Parse(datum.EndIp).Address,
                    Ip = datum.IpType == RpcStore.RemoteModel.IpType.Ip6 ? 0 : IPAddress.Parse(datum.Ip).Address,
                    Ip6 = datum.Ip6,
                    IpType = datum.IpType,
                    Remark = datum.Remark,
                    RpcMerId = datum.RpcMerId
                },
            }.Send();
        }

        public void DropIpBack (long id)
        {
            new DropIpBack
            {
                Id = id,
            }.Send();
        }

        public IpBlackDatum GetIpBack (long id)
        {
            return new GetIpBack
            {
                Id = id,
            }.Send();
        }

        public IpBlack[] QueryIpBlack (IpBlackQuery query, IBasicPage paging, out int count)
        {
            return new QueryIpBlack
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetIpBlack (long id, IpBlackSet datum)
        {
            new SetIpBlack
            {
                Id = id,
                Datum = datum,
            }.Send();
        }

        public void SetIpBlackRemark (long id, string remark)
        {
            new SetIpBlackRemark
            {
                Id = id,
                Remark = remark,
            }.Send();
        }

    }
}
