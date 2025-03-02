using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel;
using RpcStore.RemoteModel.IpBlack.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class IpBlackListDAL : IIpBlackListDAL
    {
        private readonly IRpcExtendResource<IpBlackListModel> _BasicDAL;
        public IpBlackListDAL (IRpcExtendResource<IpBlackListModel> dal)
        {
            this._BasicDAL = dal;
        }

        public IpBlackListModel[] Query (IpBlackQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }
        public IpBlackListModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public bool CheckIp6IsExists (long rpcMerId, string sysType, string ip6)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId
            && c.SystemType == sysType
            && c.IpType == IpType.Ip6
            && c.Ip6 == ip6
            && c.IsDrop == false);
        }
        public bool CheckIsExists (long rpcMerId, string sysType, long ip)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId
            && c.SystemType == sysType
            && c.IpType == IpType.Ip4
            && ( ( c.Ip == ip && !c.EndIp.HasValue ) || ( c.EndIp.HasValue && c.Ip <= ip && ip <= c.EndIp.Value ) )
            && c.IsDrop == false);
        }
        public bool CheckIsExists (long rpcMerId, string sysType, long ip, long endIp)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId
            && c.SystemType == sysType
            && c.IpType == IpType.Ip4
            && c.EndIp.HasValue && c.Ip == ip && c.EndIp == endIp
            && c.IsDrop == false);
        }
        public void SetRemark (long id, string remark)
        {
            if (!this._BasicDAL.Update(a => a.Remark == remark, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.ipblack.set.fail");
            }
        }
        public void Add (IpBlackListModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }
        public void Set (IpBlackListModel black)
        {
            IpBlackListModel add = new IpBlackListModel
            {
                Id = IdentityHelper.CreateId(),
                EndIp = black.EndIp,
                Ip = black.Ip,
                Ip6 = black.Ip6,
                IpType = black.IpType,
                IsDrop = false,
                Remark = black.Remark,
                RpcMerId = black.RpcMerId,
                SystemType = black.SystemType
            };
            ISqlQueue<IpBlackListModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(c => c.Id == black.Id);
            queue.Insert(add);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.ipblack.set.fail");
            }
        }
        public void Drop (IpBlackListModel black)
        {
            IpBlackListModel add = new IpBlackListModel
            {
                Id = IdentityHelper.CreateId(),
                EndIp = black.EndIp,
                Ip = black.Ip,
                Ip6 = black.Ip6,
                IpType = black.IpType,
                IsDrop = true,
                RpcMerId = black.RpcMerId,
                SystemType = black.SystemType
            };
            ISqlQueue<IpBlackListModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(c => c.Id == black.Id);
            queue.Insert(add);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.ipblack.set.fail");
            }
        }
    }
}
