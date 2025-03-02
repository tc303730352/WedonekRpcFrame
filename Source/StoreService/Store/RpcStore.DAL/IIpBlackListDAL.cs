using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.DAL
{
    public interface IIpBlackListDAL
    {
        void Add (IpBlackListModel add);
        bool CheckIsExists (long rpcMerId, string sysType, long ip);
        bool CheckIp6IsExists (long rpcMerId, string sysType, string ip6);
        bool CheckIsExists (long rpcMerId, string sysType, long ip, long endIp);
        void Drop (IpBlackListModel black);
        IpBlackListModel Get (long id);
        IpBlackListModel[] Query (IpBlackQuery query, IBasicPage paging, out int count);
        void Set (IpBlackListModel black);
        void SetRemark (long id, string remark);
    }
}