using RpcExtend.Model;
using RpcExtend.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class IpBlackListDAL : IIpBlackListDAL
    {
        private readonly IRepository<IpBlackList> _BasicDAL;
        public IpBlackListDAL (IRepository<IpBlackList> dal)
        {
            this._BasicDAL = dal;
        }

        public long GetMaxVer (long rpcMerId, string sysType)
        {
            return this._BasicDAL.Max(a => a.RpcMerId == rpcMerId && a.SystemType == sysType, c => c.Id);
        }

        public IpBlack[] GetIpBlacks (long rpcMerId, string sysType, long beginVer, long endVer)
        {
            return this._BasicDAL.Gets(c => c.RpcMerId == rpcMerId
            && c.SystemType == sysType
            && c.Id > beginVer
            && c.Id <= endVer,
            c => new IpBlack
            {
                EndIp = c.EndIp,
                Id = c.Id,
                Ip6 = c.Ip6,
                IpType = c.IpType,
                Ip = c.Ip,
                IsDrop = c.IsDrop
            });
        }
    }
}
