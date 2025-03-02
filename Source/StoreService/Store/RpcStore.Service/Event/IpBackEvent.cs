using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.IpBlack;
using RpcStore.RemoteModel.IpBlack.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class IpBackEvent : IRpcApiService
    {
        private readonly IIpBlackService _Service;

        public IpBackEvent (IIpBlackService service)
        {
            this._Service = service;
        }

        public long AddIpBack (AddIpBack add)
        {
            return this._Service.Add(add.Datum);
        }

        public void DropIpBack (DropIpBack obj)
        {
            this._Service.Drop(obj.Id);
        }

        public IpBlackDatum GetIpBack (GetIpBack obj)
        {
            return this._Service.Get(obj.Id);
        }

        public PagingResult<IpBlack> QueryIpBlack (QueryIpBlack query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

        public void Set (SetIpBlack set)
        {
            this._Service.Set(set.Id, set.Datum);
        }

        public void SetIpBlackRemark (SetIpBlackRemark obj)
        {
            this._Service.SetRemark(obj.Id, obj.Remark);
        }
    }
}
