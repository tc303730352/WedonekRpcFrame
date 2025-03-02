using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.Collect.lmpl
{
    internal class IpBlackCollect : IIpBlackCollect
    {
        private readonly IIpBlackListDAL _BasicDAL;
        public IpBlackCollect (IIpBlackListDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public IpBlackListModel[] Query (IpBlackQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public IpBlackListModel Get (long id)
        {
            IpBlackListModel black = this._BasicDAL.Get(id);
            if (black == null || black.IsDrop)
            {
                throw new ErrorException("rpc.store.ipBlack.not.find");
            }
            return black;
        }
        public void SetRemark (IpBlackListModel black, string remark)
        {
            if (black.IsDrop)
            {
                throw new ErrorException("rpc.store.ipBlack.already.drop");
            }
            else if (black.Remark != remark)
            {
                this._BasicDAL.SetRemark(black.Id, remark);
            }
        }
        private void _Check (IpBlackListModel ipBlack)
        {
            bool isExists;
            if (ipBlack.IpType == RemoteModel.IpType.Ip6)
            {
                isExists = this._BasicDAL.CheckIp6IsExists(ipBlack.RpcMerId, ipBlack.SystemType, ipBlack.Ip6);
            }
            else if (ipBlack.EndIp.HasValue)
            {
                isExists = this._BasicDAL.CheckIsExists(ipBlack.RpcMerId, ipBlack.SystemType, ipBlack.Ip, ipBlack.EndIp.Value);
            }
            else
            {
                isExists = this._BasicDAL.CheckIsExists(ipBlack.RpcMerId, ipBlack.SystemType, ipBlack.Ip);
            }
            if (isExists)
            {
                throw new ErrorException("rpc.store.ipBlack.repeat");
            }
        }
        public IpBlackListModel Add (IpBlackAdd datum)
        {
            IpBlackListModel add = datum.ConvertMap<IpBlackAdd, IpBlackListModel>();
            this._Check(add);
            this._BasicDAL.Add(add);
            return add;
        }

        public bool Set (IpBlackListModel black, IpBlackSet set)
        {
            if (set.IsEquals(black))
            {
                return false;
            }
            else if (set.IpType != black.IpType
                 || ( set.IpType == RemoteModel.IpType.Ip4 && ( set.Ip != black.Ip || set.EndIp != black.EndIp ) )
                 || ( set.IpType == RemoteModel.IpType.Ip6 && set.Ip6 != black.Ip6 ))
            {
                black = black.ConvertInto(set);
                this._Check(black);
            }
            else
            {
                black = black.ConvertInto(set);
            }
            this._BasicDAL.Set(black);
            return true;
        }

        public void Drop (IpBlackListModel black)
        {
            this._BasicDAL.Drop(black);
        }
    }
}
