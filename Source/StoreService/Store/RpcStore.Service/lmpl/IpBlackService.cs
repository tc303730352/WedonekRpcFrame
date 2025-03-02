using System.Net;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.IpBlack;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.IpBlack.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class IpBlackService : IIpBlackService
    {
        private readonly IIpBlackCollect _IpBlack;
        private readonly IServerTypeCollect _ServerType;
        public IpBlackService (IIpBlackCollect ipBlack,
            IServerTypeCollect serverType)
        {
            this._ServerType = serverType;
            this._IpBlack = ipBlack;
        }
        public long Add (IpBlackAdd add)
        {
            IpBlackListModel ipBlack = this._IpBlack.Add(add);
            this._Refresh(ipBlack);
            return ipBlack.Id;
        }

        public void Drop (long id)
        {
            IpBlackListModel black = this._IpBlack.Get(id);
            this._IpBlack.Drop(black);
            this._Refresh(black);
        }

        public IpBlackDatum Get (long id)
        {
            IpBlackListModel ipBlack = this._IpBlack.Get(id);
            return ipBlack.ConvertMap<IpBlackListModel, IpBlackDatum>();
        }

        public PagingResult<IpBlack> Query (IpBlackQuery query, IBasicPage paging)
        {
            IpBlackListModel[] list = this._IpBlack.Query(query, paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<IpBlack>();
            }
            Dictionary<string, string> sysTypes = this._ServerType.GetNames(list.Distinct(c => c.SystemType));
            IpBlack[] blacks = list.ConvertAll(a =>
            {
                string ip = a.Ip6;
                if (a.IpType == RemoteModel.IpType.Ip4)
                {
                    ip = a.EndIp.HasValue ? string.Format("{0}~{1}", new IPAddress(a.Ip).ToString(), new IPAddress(a.EndIp.Value).ToString()) : new IPAddress(a.Ip).ToString();
                }
                return new IpBlack
                {
                    Id = a.Id,
                    SystemType = a.SystemType,
                    SystemTypeName = sysTypes.GetValueOrDefault(a.SystemType),
                    IpType = a.IpType,
                    Remark = a.Remark,
                    Ip = ip
                };
            });
            return new PagingResult<IpBlack>(blacks, count);
        }

        public void Set (long id, IpBlackSet set)
        {
            IpBlackListModel black = this._IpBlack.Get(id);
            if (this._IpBlack.Set(black, set))
            {
                this._Refresh(black);
            }
        }

        public void SetRemark (long id, string remark)
        {
            IpBlackListModel black = this._IpBlack.Get(id);
            this._IpBlack.SetRemark(black, remark);
        }
        private void _Refresh (IpBlackListModel black)
        {
            new RefreshIpBlack
            {
                RpcMerId = black.RpcMerId,
                SystemType = black.SystemType
            }.Send(black.RpcMerId, null);
        }
    }
}
